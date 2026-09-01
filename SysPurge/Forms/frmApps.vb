'--------------------------------------------------------------------------------------------------
' SysPurge: frmApps.vb - Clean Known/Installed Applications
'    © 2026 Remus Rigo
'       v1.1.20260724
'--------------------------------------------------------------------------------------------------

Imports System.IO
Imports Microsoft.Win32

Public Class frmApps

   Dim grp As ListViewGroup = Nothing
   Dim log As New Logger(appName)

   '-----------------------------------------------------------------------------------------------
   ' Add ListView Group
   Private Sub LV_AddGroup(name As String)
      grp = New ListViewGroup(name)
      lvApps.Groups.Add(grp)
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
      lvApps.Items.Add(item)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      lvApps.BeginUpdate()
      lvApps.Items.Clear()
      lvApps.Groups.Clear()

      LV_AddGroup("Adobe")
      LV_AddItem("Adobe recent file history", True)

      LV_AddGroup("Google")
      LV_AddItem("Crash Reports", True)

      LV_AddGroup("Google Chrome")
      LV_AddItem("Crash Reports", True)
      LV_AddItem("Software Reporter Tool: Logs", True)

      LV_AddGroup("Java")
      LV_AddItem("cache", True)

      LV_AddGroup("Media Player Classic")
      LV_AddItem("Recent File List", True)

      LV_AddGroup("privacy.sexy")
      LV_AddItem("runs", True)
      LV_AddItem("logs", True)

      LV_AddGroup("Steam")
      LV_AddItem("appcache", True)
      LV_AddItem("Dumps", True)
      LV_AddItem("Traces", True)

      ResizeListViewColumns(lvApps)
      lvApps.EndUpdate()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Process Actions
   Private Async Sub ProcessActions(itemsToProcess As List(Of ListViewItem))
      For Each item As ListViewItem In itemsToProcess
         Dim grp = item.Group
         If grp Is Nothing Then Continue For

         Await Task.Delay(1)

         Select Case grp.Header

            Case "Adobe"
               Select Case item.Text
                  Case "Adobe recent file history"
                     log.Msg.Info("Clean: Adobe: Adobe recent file history")
                     TaskCleanRegValues(lvApps, item, Registry.CurrentUser, "Software\Adobe\MediaBrowser\MRU", False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Google"
               Select Case item.Text
                  Case "Crash Reports"
                     log.Msg.Info("Clean: Google: Crash Reports")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Google\CrashReports")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.log", False, False)
               End Select

            Case "Google Chrome"
               Select Case item.Text
                  Case "Crash Reports"
                     log.Msg.Info("Clean: Google Chrome: Crash Reports")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Google\Chrome\User Data\Crashpad\reports")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.log", False, False)

                  Case "Software Reporter Tool: Logs"
                     log.Msg.Info("Clean: Google Chrome: Software Reporter Tool: Logs")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Google\Software Reporter Tool")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.log", False, False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Java"
               Select Case item.Text
                  Case "cache"
                     log.Msg.Info("Clean: Java: cache")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Sun\Java\Deployment\cache")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.*", False, False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Media Player Classic"
               Select Case item.Text
                  Case "Recent File List"
                     log.Msg.Info("Clean: Media Player Classic: Recent File List")
                     TaskCleanRegValues(lvApps, item, Registry.CurrentUser, "Software\Gabest\Media Player Classic\Recent File List", False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "privacy.sexy"
               Select Case item.Text
                  Case "runs"
                     log.Msg.Info("Clean: privacy.sexy: runs")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "privacy.sexy\runs")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.*", False, False)

                  Case "logs"
                     log.Msg.Info("Clean: privacy.sexy: logs")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "privacy.sexy\logs")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.*", False, False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Steam"
               Select Case item.Text
                  Case "appcache"
                     log.Msg.Info("Clean: Steam: Cache")
                     Dim steamPath As String = RegReadSZ(Registry.CurrentUser, "SOFTWARE\Valve\Steam", "SteamPath")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "Steam\appcache"),
                        Path.Combine(steamPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar), "appcache"),
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Steam\htmlcache")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.*", True, True)

                  Case "Dumps"
                     log.Msg.Info("Clean: Steam: Dumps")
                     Dim steamPath As String = RegReadSZ(Registry.CurrentUser, "SOFTWARE\Valve\Steam", "SteamPath")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "Steam\Dumps"),
                        Path.Combine(steamPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar), "Dumps")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.*", True, True)

                  Case "Traces"
                     log.Msg.Info("Clean: Steam: Traces")
                     Dim steamPath As String = RegReadSZ(Registry.CurrentUser, "SOFTWARE\Valve\Steam", "SteamPath")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "Steam\Traces"),
                        Path.Combine(steamPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar), "Traces")
                     }
                     TaskCleanFolders(lvApps, item, pathsToClean, "*.*", True, True)
               End Select

         End Select
      Next
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSysPurge: onLoad
   Private Sub frmSysPurge_Load(sender As Object, e As EventArgs) Handles Me.Load
      Me.Text = appName & " " & appVersion & " " & appAuthor
      SendMessage(lvApps.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, CType(LVS_EX_DOUBLEBUFFER, IntPtr), CType(LVS_EX_DOUBLEBUFFER, IntPtr))

      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' btnTSPurge: onClick
   Private Async Sub btnTSPurge_Click(sender As Object, e As EventArgs) Handles btnAppsRun.Click
      ' 1. Gather the items to process on the UI thread
      Dim itemsToProcess As New List(Of ListViewItem)()
      For Each item As ListViewItem In lvApps.Items
         If item.Checked AndAlso item.Group IsNot Nothing Then
            itemsToProcess.Add(item)
         End If
      Next

      Try
         'Pass the gathered items to the background worker
         Await Task.Run(Sub() ProcessActions(itemsToProcess))
      Finally
         'toolBtnPurge.Enabled = True
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawColumnHeader
   Private Sub lvSysPurge_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles lvApps.DrawColumnHeader
      ' draw column headers with default style
      e.DrawDefault = True
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawItem
   Private Sub lvSysPurge_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles lvApps.DrawItem
      ' draw items with default style (except subitem 2 which is handled in DrawSubItem)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawSubItem
   Private Sub lvSysPurge_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles lvApps.DrawSubItem
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
      SendMessage(lvApps.Handle, LVM_GETSUBITEMRECT, CType(e.ItemIndex, IntPtr), r)

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
         TextRenderer.DrawText(g, text, lvApps.Font, rect, Color.Black,
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
      End If
   End Sub

End Class
