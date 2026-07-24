'--------------------------------------------------------------------------------------------------
' SysPurge: Tasks.vb
'    © 2026 Remus Rigo
'       v1.0.2026-07-24
'--------------------------------------------------------------------------------------------------

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
   ' Task: Clean Folders
   Public Sub TaskCleanFolders(lv As ListView, item As ListViewItem, folderPaths As IEnumerable(Of String), mask As String, recursive As Boolean, deleteFolders As Boolean)
      Dim totalDeletedBytes As Long = 0
      Dim lastUpdate As Integer = Environment.TickCount

      ' Convert to list to avoid multiple enumerations
      Dim pathsList = folderPaths.ToList()

      For Each folderPath As String In pathsList
         If Not Directory.Exists(folderPath) Then Continue For

         Dim search = If(recursive, SearchOption.AllDirectories, SearchOption.TopDirectoryOnly)
         Dim files() As String
         Try
            files = Directory.GetFiles(folderPath, mask, search)
         Catch
            Continue For
         End Try

         For i = 0 To files.Length - 1
            Try
               Dim fi = New FileInfo(files(i))
               Dim size = fi.Length
               File.Delete(files(i))
               totalDeletedBytes += size
            Catch ex As Exception
               log.Msg.Error("{ex.Message} : {files(i)}")
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
            Try
               Dim folders() As String = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories)
               For i = folders.Length - 1 To 0 Step -1
                  Try
                     Directory.Delete(folders(i), False)
                  Catch ex As Exception
                     log.Msg.Error("{ex.Message} : {folders(i)}")
                  End Try
               Next
            Catch
            End Try
         End If
      Next

      SetTaskProgressBytes(lv, item, totalDeletedBytes, 100)
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
