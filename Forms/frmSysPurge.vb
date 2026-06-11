Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Win32

Public Class frmSysPurge

   Dim grp As ListViewGroup = Nothing

   Public Sub ResizeColumns()
      Dim w As Integer

      ' Setting column width to -1 in WinForms ListView auto-sizes the column
      lvSysPurge.Columns(0).Width = -1
      lvSysPurge.Columns(1).Width = -1

      ' Retrieve column widths via P/Invoke
      w = SendMessage(lvSysPurge.Handle, LVM_GETCOLUMNWIDTH, New IntPtr(0), IntPtr.Zero).ToInt32()
      w += SendMessage(lvSysPurge.Handle, LVM_GETCOLUMNWIDTH, New IntPtr(1), IntPtr.Zero).ToInt32()

      ' Calculate and set the width of the third column (index 2)
      lvSysPurge.Columns(2).Width = lvSysPurge.ClientSize.Width - w - GetSystemMetrics(SM_CXVSCROLL)
   End Sub

   Private Function FormatBytes(b As Long) As String
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

   Private Sub LV_AddGroup(name As String)
      grp = New ListViewGroup(name)
      lvSysPurge.Groups.Add(grp)
   End Sub

   Private Sub LV_AddItem(name As String)
      Dim item As New ListViewItem(name)
      item.SubItems.Add("")
      item.SubItems.Add("")
      item.Tag = 0
      item.Group = grp
      lvSysPurge.Items.Add(item)
   End Sub

   Public Sub BuildOptions()
      lvSysPurge.BeginUpdate()
      lvSysPurge.Items.Clear()
      lvSysPurge.Groups.Clear()

      LV_AddGroup("Microsoft Windows FileSystem")
      If IsAppElevated() Then LV_AddItem("EventViewer logs")
      LV_AddItem("Log files (inside Windows)")
      LV_AddItem("Log files (System drive)")
      LV_AddItem("Prefetch files")
      LV_AddItem("Temp files (Current User)")
      LV_AddItem("Temp files (Windows)")
      If IsAppElevated() Then LV_AddItem("Windows Update cache")

      LV_AddGroup("Microsoft Windows Registry")
      LV_AddItem("MRU list: Run")
      If IsAppElevated() Then LV_AddItem("Shared DLL's)")

      ResizeColumns()
      lvSysPurge.EndUpdate()
   End Sub

   Private Sub ProcessActions(itemsToProcess As List(Of ListViewItem))
      For Each item As ListViewItem In itemsToProcess
         Dim grp = item.Group
         If grp Is Nothing Then Continue For

         Select Case grp.Header
            Case "Microsoft Windows FileSystem"
               Select Case item.Text
                  Case "EventViewer logs"
                     '
                  Case "Log files (inside Windows)"
                     '
                  Case "Log files (System drive)"
                     '
                  Case "Prefetch files"
                     '
                  Case "Temp files (Current User)"
                     TaskCleanFolder(item, Environment.GetEnvironmentVariable("TEMP"), "*.*", True, True)
                     'MessageBox.Show(Environment.GetEnvironmentVariable("TEMP"))
                  Case "Temp files (Windows)"
                     '
                  Case "Windows Update cache"
                     '
               End Select

            Case "Microsoft Windows Registry"
               Select Case item.Text
                  Case "MRU list: Run"
                     TaskCleanRegValues(item, Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU", False)
                  Case "Shared DLL's)"
                     '
               End Select
         End Select
      Next
   End Sub

   Private Sub SetTaskProgressBytes(item As ListViewItem, bytes As Long, progress As Integer)
      Dim formatted = FormatBytes(bytes)
      Dim p = Math.Max(0, Math.Min(100, progress))

      If lvSysPurge.InvokeRequired Then
         lvSysPurge.Invoke(New Action(Of ListViewItem, Long, Integer)(AddressOf SetTaskProgressBytes), item, bytes, progress)
         Return
      End If

      item.SubItems(1).Text = formatted
      item.Tag = p
      ResizeColumns()
      lvSysPurge.Invalidate(item.Bounds)
      lvSysPurge.Update()
   End Sub

   Private Sub SetTaskProgressCount(item As ListViewItem, count As Integer, progress As Integer)
      Dim formatted = String.Format("{0} entries", count)
      Dim p = Math.Max(0, Math.Min(100, progress))

      If lvSysPurge.InvokeRequired Then
         lvSysPurge.Invoke(New Action(Of ListViewItem, Integer, Integer)(AddressOf SetTaskProgressCount), item, count, progress)
         Return
      End If

      item.SubItems(1).Text = formatted
      item.Tag = p
      ResizeColumns()
      lvSysPurge.Invalidate(item.Bounds)
      lvSysPurge.Update()
   End Sub

   Private Sub TaskCleanFolder(item As ListViewItem, folderPath As String, mask As String, recursive As Boolean, deleteFolders As Boolean)
      Dim deletedBytes As Long = 0
      Dim lastUpdate As Integer = Environment.TickCount

      SetTaskProgressBytes(item, deletedBytes, 0)

      If Not Directory.Exists(folderPath) Then
         SetTaskProgressBytes(item, deletedBytes, 100)
         Return
      End If

      Dim search = If(recursive, SearchOption.AllDirectories, SearchOption.TopDirectoryOnly)
      Dim files() As String

      Try
         files = Directory.GetFiles(folderPath, mask, search)
      Catch
         SetTaskProgressBytes(item, deletedBytes, 100)
         Return
      End Try

      For i = 0 To files.Length - 1
         Try
            Dim fi = New FileInfo(files(i))
            Dim size = fi.Length
            File.Delete(files(i))
            deletedBytes += size
         Catch
            ' ignore
         End Try

         Dim now = Environment.TickCount
         If now - lastUpdate >= 25 Then
            Dim prog = CInt((i + 1) * 100.0 / files.Length)
            SetTaskProgressBytes(item, deletedBytes, prog)
            lastUpdate = now
         End If
      Next

      SetTaskProgressBytes(item, deletedBytes, 100)

      If deleteFolders AndAlso recursive Then
         Dim folders() As String = {}
         Try
            folders = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories)
         Catch
         End Try

         For i = folders.Length - 1 To 0 Step -1
            Try
               Directory.Delete(folders(i), False)
            Catch
            End Try
         Next
      End If
   End Sub

   Private Sub TaskCleanRegValues(item As ListViewItem, root As RegistryKey, keyPath As String, includeDefault As Boolean)
      SetTaskProgressCount(item, 0, 0)

      Try
         If root Is Nothing Then
            SetTaskProgressCount(item, 0, 100)
            Exit Sub
         End If

         ' Open the subkey directly from the provided root RegistryKey with write permissions
         Using subKey As RegistryKey = root.OpenSubKey(keyPath, writable:=True)

            If subKey Is Nothing Then
               SetTaskProgressCount(item, 0, 100)
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
                  SetTaskProgressCount(item, deleted, progress)
                  lastUpdate = now
               End If
            Next

            ' Final update
            SetTaskProgressCount(item, deleted, 100)

         End Using

      Catch
         SetTaskProgressCount(item, 0, 100)
      End Try
   End Sub

   Private Sub frmSysPurge_Load(sender As Object, e As EventArgs) Handles Me.Load
      BuildOptions()

      SendMessage(lvSysPurge.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, CType(LVS_EX_DOUBLEBUFFER, IntPtr), CType(LVS_EX_DOUBLEBUFFER, IntPtr))
   End Sub

   Private Async Sub btnTSPurge_Click(sender As Object, e As EventArgs) Handles btnTSPurge.Click
      ' 1. Gather the items to process on the UI thread
      Dim itemsToProcess As New List(Of ListViewItem)()
      For Each item As ListViewItem In lvSysPurge.Items
         If item.Checked AndAlso item.Group IsNot Nothing Then
            itemsToProcess.Add(item)
         End If
      Next

      Try
         ' 2. Pass the gathered items to the background worker
         Await Task.Run(Sub() ProcessActions(itemsToProcess))
      Finally
         'toolBtnPurge.Enabled = True
      End Try
   End Sub

   Private Sub lvSysPurge_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles lvSysPurge.DrawSubItem
      ' column 3 (index 2)
      If e.ColumnIndex <> 2 Then
         e.DrawDefault = True
         Return
      End If

      e.DrawBackground()

      Const COLOR_FILL_ACTIVE As Integer = &HD07800
      Const COLOR_FILL_DONE As Integer = &H50B000
      Const COLOR_BORDER As Integer = &HAAAAAA
      Const PADDING_H As Integer = 3
      Const PADDING_V As Integer = 4

      Dim r As RECT
      r.Top = e.ColumnIndex
      r.Left = LVIR_BOUNDS
      SendMessage(lvSysPurge.Handle, LVM_GETSUBITEMRECT, CType(e.ItemIndex, IntPtr), r)

      Dim rect = Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom)
      rect.Inflate(-PADDING_H, -PADDING_V)

      Dim progress As Integer = 0
      If e.Item.Tag IsNot Nothing Then
         progress = CInt(e.Item.Tag)
      End If

      Dim g = e.Graphics

      Using bg As New SolidBrush(SystemColors.Window)
         g.FillRectangle(bg, rect)
      End Using

      If progress > 0 Then
         Dim fillWidth = CInt(rect.Width * progress / 100.0)
         Dim fillRect = New Rectangle(rect.Left, rect.Top, fillWidth, rect.Height)

         Dim fillColor = If(progress >= 100, ColorTranslator.FromWin32(COLOR_FILL_DONE),
                                            ColorTranslator.FromWin32(COLOR_FILL_ACTIVE))
         Using br As New SolidBrush(fillColor)
            g.FillRectangle(br, fillRect)
         End Using
      End If

      Using pen As New Pen(ColorTranslator.FromWin32(COLOR_BORDER))
         g.DrawRectangle(pen, rect)
      End Using

      If progress > 0 Then
         Dim text = progress.ToString() & "%"
         TextRenderer.DrawText(g, text, lvSysPurge.Font, rect, Color.Black,
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
      End If
   End Sub

   Private Sub lvSysPurge_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles lvSysPurge.DrawColumnHeader
      ' draw column headers with default style
      e.DrawDefault = True
   End Sub

   Private Sub lvSysPurge_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles lvSysPurge.DrawItem
      ' draw items with default style (except subitem 2 which is handled in DrawSubItem)
   End Sub
End Class
