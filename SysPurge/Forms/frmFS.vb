'--------------------------------------------------------------------------------------------------
' SysPurge: frmFS.vb - Clean File System
'    © 2026 Remus Rigo
'       v1.1.20260828
'--------------------------------------------------------------------------------------------------

Imports Microsoft.Win32
Imports System.IO

Public Class frmFS

   Dim grp As ListViewGroup = Nothing
   Dim log As New Logger(appName)

   '-----------------------------------------------------------------------------------------------
   ' Add ListView Group
   Private Sub LV_AddGroup(name As String)
      grp = New ListViewGroup(name)
      lvFS.Groups.Add(grp)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Add ListView item
   Private Sub LV_AddItem(name As String, isChecked As Boolean)
      Dim item As New ListViewItem(name)
      item.SubItems.Add("")
      item.SubItems.Add("")
      item.Checked = isChecked
      item.Tag = 0
      item.Group = grp
      lvFS.Items.Add(item)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      lvFS.BeginUpdate()
      lvFS.Items.Clear()
      lvFS.Groups.Clear()

      LV_AddGroup("Temporary/Junk files")
      LV_AddItem("User Temp folder", True)
      LV_AddItem("System Temp folder", True)
      LV_AddItem("Log files (inside Windows)", True)
      LV_AddItem("Log files (System drive)", False)
      If IsAppElevated() Then LV_AddItem("Previous Windows installation", True)

      LV_AddGroup("Microsoft Windows FileSystem")
      LV_AddItem("Jump List", True)
      LV_AddItem("Prefetch files", True)
      LV_AddItem("Recent Items", True)
      If IsAppElevated() Then LV_AddItem("Windows Update cache", False)

      '--------------------------------------------------------------------------------------------

      ResizeListViewColumns(lvFS)
      lvFS.EndUpdate()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Process Actions
   Private Sub ProcessActions(itemsToProcess As List(Of ListViewItem))
      For Each item As ListViewItem In itemsToProcess
         Dim grp = item.Group
         If grp Is Nothing Then Continue For

         Select Case grp.Header
            '--------------------------------------------------------------------------------------
            Case "Temporary/Junk files"
               Select Case item.Text
                  Case "User Temp folder"
                     log.Msg.Info("Clean: Microsoft Windows: User Temp folder")
                     Dim pathsToClean As String() = {
                        Environment.GetEnvironmentVariable("TEMP")
                     }
                     TaskCleanFolders(lvFS, item, pathsToClean, "*.*", True, True)

                  Case "System Temp folder"
                     log.Msg.Info("Clean: Microsoft Windows: System Temp folder")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "Temp")
                     }
                     TaskCleanFolders(lvFS, item, pathsToClean, "*.*", True, True)

                  Case "Log files (inside Windows)"
                     log.Msg.Info("Clean: Temporary/Junk files: Log files (inside Windows)")
                     Dim pathsToClean As String() = {
                        Environment.GetEnvironmentVariable("SystemRoot")
                     }
                     TaskCleanFolders(lvFS, item, pathsToClean, "*.log", True, True)

                  Case "Log files (System drive)"
                     log.Msg.Info("Clean: Temporary/Junk files: Log files (System drive)")
                     Dim pathsToClean As String() = {
                        Environment.GetEnvironmentVariable("SystemDrive")
                     }
                     TaskCleanFolders(lvFS, item, pathsToClean, "*.log", True, True)

                  Case "Previous Windows installation"
                     log.Msg.Info("Clean: Temporary/Junk files: Previous Windows installation")
                     Dim prevWinDir As String = Path.Combine(Environment.GetEnvironmentVariable("SystemDrive"), "Windows.old")
                     Try
                        TakeOwnership(prevWinDir)
                        GrantFullControl(prevWinDir)
                        Dim pathsToClean As String() = {prevWinDir}
                        TaskCleanFolders(lvFS, item, pathsToClean, "*.*", True, True, True)
                     Catch ex As Exception
                        log.Msg.Warning(ex.Message)
                     End Try
               End Select

            '--------------------------------------------------------------------------------------
            Case "Microsoft Windows FileSystem"
               Select Case item.Text

                  Case "Jump List"
                     log.Msg.Info("Clean: Microsoft Windows FileSystem: Jump List")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\Windows\Recent\AutomaticDestinations"),
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\Windows\Recent\CustomDestinations")
                     }
                     TaskCleanFolders(lvFS, item, pathsToClean, "*.automaticDestinations-ms", False, False)

                  Case "Prefetch files"
                     log.Msg.Info("Clean: Microsoft Windows FileSystem: Prefetch files")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "Prefetch")
                     }
                     TaskCleanFolders(lvFS, item, pathsToClean, "*.pf", False, False)

                  Case "Recent files"
                     log.Msg.Info("Clean: Microsoft Windows FileSystem: Recent Items")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\Windows\Recent"),
                        RegReadSZ(Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", "Recent")
                     }
                     TaskCleanFolders(lvFS, item, pathsToClean, "*.*", False, False)

                  Case "Windows Update cache"
                     log.Msg.Info("Clean: Microsoft Windows FileSystem: Windows Update cache")
                     StopService("wuauserv")
                     StopService("bits")
                     StopService("cryptsvc")
                     StopService("msiserver")
                     System.Threading.Thread.Sleep(5000)
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "SoftwareDistribution\Download"),
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "SoftwareDistribution\DataStore")
                     }
                     TaskCleanFolders(lvFS, item, pathsToClean, "*.*", True, True)
                     StartService("msiserver")
                     StartService("cryptsvc")
                     StartService("bits")
                     StartService("wuauserv")

               End Select

         End Select
      Next
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSysPurge: onLoad
   Private Sub frmSysPurge_Load(sender As Object, e As EventArgs) Handles Me.Load
      Me.Text = appName & " " & appVersion & " " & appAuthor
      SendMessage(lvFS.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, CType(LVS_EX_DOUBLEBUFFER, IntPtr), CType(LVS_EX_DOUBLEBUFFER, IntPtr))

      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsFS_btnRun: onClick
   Private Sub tsFS_btnRun_Click(sender As Object, e As EventArgs) Handles tsFS_btnRun.Click
      tsFS_btnRun.Enabled = False
      Try
         Dim itemsToProcess As New List(Of ListViewItem)()
         For Each item As ListViewItem In lvFS.Items
            If item.Checked AndAlso item.Group IsNot Nothing Then
               itemsToProcess.Add(item)
            End If
         Next
         ProcessActions(itemsToProcess)
      Finally
         tsFS_btnRun.Enabled = True
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawColumnHeader
   Private Sub lvSysPurge_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles lvFS.DrawColumnHeader
      ' draw column headers with default style
      e.DrawDefault = True
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawItem
   Private Sub lvSysPurge_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles lvFS.DrawItem
      ' draw items with default style (except subitem 2 which is handled in DrawSubItem)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawSubItem
   Private Sub lvSysPurge_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles lvFS.DrawSubItem
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
      SendMessage(lvFS.Handle, LVM_GETSUBITEMRECT, CType(e.ItemIndex, IntPtr), r)

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
         TextRenderer.DrawText(g, text, lvFS.Font, rect, Color.Black,
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
      End If
   End Sub

End Class
