'--------------------------------------------------------------------------------------------------
' SysPurge: Tasks.vb
'    © 2026 Remus Rigo
'       v1.0.20261110
'--------------------------------------------------------------------------------------------------

Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports Microsoft.Win32

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
      Dim formatted = FormatBytes(bytes)
      Dim p = Math.Max(0, Math.Min(100, progress))

      If lv.InvokeRequired Then
         lv.Invoke(New Action(Of ListView, ListViewItem, Long, Integer)(AddressOf SetTaskProgressBytes), lv, item, bytes, progress)
         Return
      End If

      item.SubItems(1).Text = formatted
      item.Tag = p
      ResizeListViewColumns(lv)
      lv.Invalidate(item.Bounds)
      lv.Update()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' SetTaskProgressCount: Updates the progress count for a task
   Private Sub SetTaskProgressCount(lv As ListView, item As ListViewItem, count As Integer, progress As Integer)
      Dim formatted = String.Format("{0} entries", count)
      Dim p = Math.Max(0, Math.Min(100, progress))

      If lv.InvokeRequired Then
         lv.Invoke(New Action(Of ListView, ListViewItem, Integer, Integer)(AddressOf SetTaskProgressCount), lv, item, count, progress)
         Return
      End If

      item.SubItems(1).Text = formatted
      item.Tag = p
      ResizeListViewColumns(lv)
      lv.Invalidate(item.Bounds)
      lv.Update()
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
   ' Task: Clean Folders
   Public Sub TaskCleanFolders(lv As ListView, item As ListViewItem, folderPaths As IEnumerable(Of String), mask As String,
      recursive As Boolean, deleteFolders As Boolean, Optional includeParent As Boolean = False)
      Dim totalDeletedBytes As Long = 0
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
            If now - lastUpdate >= 25 Then
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
   End Sub

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


End Module
