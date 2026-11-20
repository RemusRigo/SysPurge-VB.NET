'--------------------------------------------------------------------------------------------------
' SysPurge: Tasks.vb
'    © 2026 Remus Rigo
'       v1.2.20261120
'--------------------------------------------------------------------------------------------------

Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports SysPurge.API

Module Tasks

   Dim log As New Logger(appName)

   '-----------------------------------------------------------------------------------------------
   ' FormatBytes: Converts a byte count into a human-readable string with appropriate units
   Public Function FormatBytes(b As Long) As String
      If b >= 1073741824L Then
         Return String.Format("{0:F2} GB", b / 1073741824.0R)
      ElseIf b >= 1048576L Then
         Return String.Format("{0:F2} MB", b / 1048576.0R)
      ElseIf b >= 1024L Then
         Return String.Format("{0:F1} KB", b / 1024.0R)
      Else
         Return String.Format("{0} B", b)
      End If
   End Function

   '-----------------------------------------------------------------------------------------------
   ' SetTaskProgressBytes: Updates the progress bytes for a task
   Public Sub SetTaskProgressBytes(lv As ListView, item As ListViewItem, bytes As Long, progress As Integer)
      If lv.InvokeRequired Then
         ' BeginInvoke: the worker doesn't wait for the paint; messages still arrive in order
         lv.BeginInvoke(New Action(Of ListView, ListViewItem, Long, Integer)(AddressOf SetTaskProgressBytes), lv, item, bytes, progress)
         Return
      End If

      UpdateTaskCell(lv, item, FormatBytes(bytes), progress)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' SetTaskProgressCount: Updates the progress count for a task
   Private Sub SetTaskProgressCount(lv As ListView, item As ListViewItem, count As Integer, progress As Integer)
      If lv.InvokeRequired Then
         lv.BeginInvoke(New Action(Of ListView, ListViewItem, Integer, Integer)(AddressOf SetTaskProgressCount), lv, item, count, progress)
         Return
      End If

      UpdateTaskCell(lv, item, $"{count} entries", progress)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' UpdateTaskCell (UI thread): updates the Deleted text and the progress bar, repainting only
   ' what actually changed. Columns are resized once, when the task reaches 100%.
   Private Sub UpdateTaskCell(lv As ListView, item As ListViewItem, text As String, progress As Integer)
      If item.ListView Is Nothing OrElse item.SubItems.Count < 4 Then Return  ' removed, or not a 4-column row

      Dim p As Integer = Math.Max(0, Math.Min(100, progress))
      Dim oldP As Integer = If(TypeOf item.Tag Is Integer, CInt(item.Tag), -1)
      Dim textChanged As Boolean = item.SubItems(2).Text <> text

      If Not textChanged AndAlso p = oldP Then Return

      If textChanged Then
         item.SubItems(2).Text = text                       ' ListView repaints this cell itself
      End If

      If p <> oldP Then
         item.Tag = p
         lv.Invalidate(item.SubItems(3).Bounds)             ' repaint ONLY the progress cell
      End If

      If p >= 100 Then ResizeListViewColumns(lv)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' GetTargetSize: Total size of the files a CleanTarget would delete (same rules as SafeEnumerate)
   Public Function GetTargetSize(t As CleanTarget, token As Threading.CancellationToken) As Long
      If String.IsNullOrEmpty(t.Path) OrElse Not Directory.Exists(t.Path) Then Return 0

      Dim total As Long = 0
      Dim pending As New Stack(Of DirectoryInfo)
      pending.Push(New DirectoryInfo(t.Path))

      While pending.Count > 0
         If token.IsCancellationRequested Then Return total
         Dim dir As DirectoryInfo = pending.Pop()

         Try
            For Each fi As FileInfo In dir.EnumerateFiles(t.Mask)
               total += fi.Length
            Next
         Catch ex As UnauthorizedAccessException
         Catch ex As IOException
         End Try

         If Not t.Recursive Then Continue While

         Try
            For Each di As DirectoryInfo In dir.EnumerateDirectories()
               If (di.Attributes And FileAttributes.ReparsePoint) = 0 Then pending.Push(di)
            Next
         Catch ex As UnauthorizedAccessException
         Catch ex As IOException
         End Try
      End While

      Return total
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Task: Clean Targets - cleans several CleanTargets, keeping one running total of deleted bytes
   Public Sub TaskCleanTargets(lv As ListView, item As ListViewItem, targets As IEnumerable(Of CleanTarget))
      Dim total As Long = 0
      For Each t As CleanTarget In targets
         If String.IsNullOrEmpty(t.Path) Then Continue For
         total = TaskCleanFolders(lv, item, {t.Path}, t.Mask, t.Recursive, t.DeleteFolders, t.IncludeParent, total)
      Next
      SetTaskProgressBytes(lv, item, total, 100)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' SafeEnumerate: Collects files (matching mask) and subfolders under root, skipping folders
   ' that can't be read and junctions/symlinks (avoids loops and escaping into other trees).
   ' Folders are returned parent-before-child, so delete them in reverse order.
   Private Sub SafeEnumerate(root As String, mask As String, recursive As Boolean, files As List(Of String), dirs As List(Of String))
      Dim pending As New Stack(Of String)
      pending.Push(root)

      While pending.Count > 0
         Dim dir As String = pending.Pop()

         Try
            files.AddRange(Directory.GetFiles(dir, mask))
         Catch ex As UnauthorizedAccessException
         Catch ex As IOException
         End Try

         If Not recursive Then Continue While

         Try
            For Each di As DirectoryInfo In New DirectoryInfo(dir).EnumerateDirectories()
               If (di.Attributes And FileAttributes.ReparsePoint) = 0 Then
                  dirs.Add(di.FullName)
                  pending.Push(di.FullName)
               End If
            Next
         Catch ex As UnauthorizedAccessException
         Catch ex As IOException
         End Try
      End While
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Task: Clean Folders - returns the running total of deleted bytes (startBytes + this call)
   Public Function TaskCleanFolders(lv As ListView, item As ListViewItem, folderPaths As IEnumerable(Of String), mask As String,
      recursive As Boolean, deleteFolders As Boolean, Optional includeParent As Boolean = False,
      Optional startBytes As Long = 0) As Long
      Dim totalDeletedBytes As Long = startBytes
      Dim lastUpdate As Integer = Environment.TickCount

      ' Convert to list to avoid multiple enumerations
      Dim pathsList = folderPaths.ToList()

      For Each folderPath As String In pathsList
         If Not Directory.Exists(folderPath) Then Continue For

         Dim fileList As New List(Of String)
         Dim dirList As New List(Of String)
         SafeEnumerate(folderPath, mask, recursive, fileList, dirList)
         Dim files() As String = fileList.ToArray()

         For i = 0 To files.Length - 1
            Try
               Dim fi = New FileInfo(files(i))
               Dim size = fi.Length
               File.Delete(files(i))
               totalDeletedBytes += size
            Catch ex As Exception
               log.Msg.Error($"{ex.Message} : {files(i)}")
            End Try

            ' Update progress periodically
            Dim now = Environment.TickCount
            If now - lastUpdate >= 100 Then
               ' Calculate progress based on the current file index within the current folder
               Dim prog = CInt((i + 1) * 100.0 / Math.Max(files.Length, 1))
               SetTaskProgressBytes(lv, item, totalDeletedBytes, prog)
               lastUpdate = now
            End If
         Next

         ' Cleanup folders if requested
         If deleteFolders AndAlso recursive Then
            For i = dirList.Count - 1 To 0 Step -1
               Try
                  Directory.Delete(dirList(i), False)
               Catch ex As Exception
                  log.Msg.Error($"{ex.Message} : {dirList(i)}")
               End Try
            Next
         End If

         ' Delete the parent folder itself
         If includeParent AndAlso deleteFolders Then
            Try
               If Directory.Exists(folderPath) Then
                  Dim hasFiles = Directory.EnumerateFiles(folderPath).Any()
                  Dim hasDirs = Directory.EnumerateDirectories(folderPath).Any()

                  If Not hasFiles AndAlso Not hasDirs Then
                     Directory.Delete(folderPath, False)
                  End If
               End If
            Catch ex As Exception
               log.Msg.Error($"{ex.Message} : {folderPath}")
            End Try
         End If

      Next

      SetTaskProgressBytes(lv, item, totalDeletedBytes, 100)
      Return totalDeletedBytes
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Task: Clear all Windows event logs (equivalent of "wevtutil cl" for every log). Requires elevation.
   ' Logs that can't be cleared (e.g. enabled analytic/debug logs) are skipped.
   Public Sub TaskClearEventLogs(lv As ListView, item As ListViewItem)
      SetTaskProgressCount(lv, item, 0, 0)

      Dim cleared As Integer = 0
      Try
         Using session As New EventLogSession()
            Dim logNames As List(Of String) = session.GetLogNames().ToList()
            Dim lastUpdate As Integer = Environment.TickCount

            For i As Integer = 0 To logNames.Count - 1
               Try
                  session.ClearLog(logNames(i))
                  cleared += 1
               Catch
                  ' not clearable / access denied: skip
               End Try

               Dim now = Environment.TickCount
               If now - lastUpdate >= 40 Then
                  SetTaskProgressCount(lv, item, cleared, CInt((i + 1) * 100.0 / logNames.Count))
                  lastUpdate = now
               End If
            Next
         End Using
      Catch ex As Exception
         log.Msg.Error($"Clear event logs: {ex.Message}")
      End Try

      SetTaskProgressCount(lv, item, cleared, 100)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Task: Clean Registry Values
   Public Sub TaskCleanRegValues(lv As ListView, item As ListViewItem, root As RegistryKey, keyPath As String, includeDefault As Boolean)
      SetTaskProgressCount(lv, item, 0, 0)

      Try
         If root Is Nothing Then
            SetTaskProgressCount(lv, item, 0, 100)
            Exit Sub
         End If

         ' Open the subkey directly from the provided root RegistryKey with write permissions
         Using subKey As RegistryKey = root.OpenSubKey(keyPath, writable:=True)

            If subKey Is Nothing Then
               SetTaskProgressCount(lv, item, 0, 100)
               Exit Sub
            End If

            Dim names As String() = subKey.GetValueNames()
            Dim deleted As Integer = 0
            Dim lastUpdate As Integer = Environment.TickCount

            For i As Integer = 0 To names.Length - 1

               Dim name As String = names(i)

               If includeDefault Then
                  subKey.DeleteValue(name, False)
                  deleted += 1
               Else
                  ' Skip default value ("")
                  If name <> "" Then
                     subKey.DeleteValue(name, False)
                     deleted += 1
                  End If
               End If

               ' Throttle UI updates
               Dim now = Environment.TickCount
               If now - lastUpdate >= 40 Then
                  Dim progress = CInt((i + 1) * 100.0 / names.Length)
                  SetTaskProgressCount(lv, item, deleted, progress)
                  lastUpdate = now
               End If
            Next

            ' Final update
            SetTaskProgressCount(lv, item, deleted, 100)

         End Using

      Catch
         SetTaskProgressCount(lv, item, 0, 100)
      End Try
   End Sub


   '-----------------------------------------------------------------------------------------------
   ' GetRecycleBinInfo: size and item count of the Recycle Bin on all drives
   Public Function GetRecycleBinInfo(ByRef size As Long, ByRef count As Long) As Boolean
      size = 0 : count = 0

      If IntPtr.Size = 8 Then
         Dim info As New SHQUERYRBINFO64 With {.cbSize = Marshal.SizeOf(GetType(SHQUERYRBINFO64))}
         If SHQueryRecycleBin64(Nothing, info) <> 0 Then Return False
         size = info.i64Size : count = info.i64NumItems
      Else
         Dim info As New SHQUERYRBINFO32 With {.cbSize = Marshal.SizeOf(GetType(SHQUERYRBINFO32))}
         If SHQueryRecycleBin32(Nothing, info) <> 0 Then Return False
         size = info.i64Size : count = info.i64NumItems
      End If

      Return True
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Size column text for the Recycle Bin row (signature fits CleanerList.AddCustomItem)
   Public Function GetRecycleBinSizeText(token As Threading.CancellationToken) As String
      Dim size, count As Long
      If Not GetRecycleBinInfo(size, count) Then Return "?"
      Return FormatBytes(size)
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Task: Empty the Recycle Bin (all drives). The API works in one go, so progress jumps 0 -> 100.
   Public Sub TaskEmptyRecycleBin(lv As ListView, item As ListViewItem)
      SetTaskProgressBytes(lv, item, 0, 0)

      Dim sizeBefore, countBefore As Long
      GetRecycleBinInfo(sizeBefore, countBefore)

      If countBefore > 0 Then
         Dim hr As Integer = SHEmptyRecycleBin(IntPtr.Zero, Nothing,
                                               SHERB_NOCONFIRMATION Or SHERB_NOPROGRESSUI Or SHERB_NOSOUND)
         If hr <> 0 Then log.Msg.Warning($"Recycle Bin: SHEmptyRecycleBin returned 0x{hr:X8}")
      End If

      ' deleted = what is gone (some items may be locked and stay in the bin)
      Dim sizeAfter, countAfter As Long
      GetRecycleBinInfo(sizeAfter, countAfter)

      SetTaskProgressBytes(lv, item, Math.Max(0, sizeBefore - sizeAfter), 100)
   End Sub

End Module
