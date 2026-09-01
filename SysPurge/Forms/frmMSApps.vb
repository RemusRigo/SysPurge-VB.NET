'--------------------------------------------------------------------------------------------------
' SysPurge: frmMSApps.vb - Clean Microsoft Apps
'    © 2026 Remus Rigo
'       v1.1.20260724
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Configuration
Imports System.IO
Imports System.Runtime
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Win32

Public Class frmMSApps

   Dim grp As ListViewGroup = Nothing
   Dim log As New Logger(appName)

   '-----------------------------------------------------------------------------------------------
   ' Add ListView Group
   Private Sub LV_AddGroup(name As String)
      grp = New ListViewGroup(name)
      lvMSApps.Groups.Add(grp)
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
      lvMSApps.Items.Add(item)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      lvMSApps.BeginUpdate()
      lvMSApps.Items.Clear()
      lvMSApps.Groups.Clear()

      LV_AddGroup(".NET")
      LV_AddItem("Telemetry data", True)

      LV_AddGroup("EventViewer")
      LV_AddItem("Logs", True)

      LV_AddGroup("DirectX")
      LV_AddItem("Shader Cache", True)
      LV_AddItem("Direct3D: Most Recent Application", True)

      LV_AddGroup("Internet Explorer")
      LV_AddItem("Cache", True)
      LV_AddItem("Cookies", True)
      LV_AddItem("Temporary Internet Files", True)
      LV_AddItem("Typed URLs", True)

      LV_AddGroup("Microsoft Management Console")
      LV_AddItem("Recent File List", True)

      LV_AddGroup("OneDrive")
      If IsAppElevated() Then LV_AddItem("Temp folder", True)

      LV_AddGroup("PowerShell")
      LV_AddItem("Console Host History", True)

      LV_AddGroup("Teams")
      LV_AddItem("Cache", True)

      LV_AddGroup("Visual Studio")
      LV_AddItem("Telemetry data", True)

      LV_AddGroup("Windows Media Player")
      LV_AddItem("Recent File List", True)
      LV_AddItem("Recent URLs List", True)

      ResizeListViewColumns(lvMSApps)
      lvMSApps.EndUpdate()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Process Actions
   Private Async Sub ProcessActions(itemsToProcess As List(Of ListViewItem))
      For Each item As ListViewItem In itemsToProcess
         Dim grp = item.Group
         If grp Is Nothing Then Continue For

         Select Case grp.Header

            '--------------------------------------------------------------------------------------
            Case ".NET"
               Select Case item.Text
                  Case "Telemetry data"
                     log.Msg.Info("Clean: Microsoft .NET: Telemetry data")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("UserProfile"), ".dotnet\TelemetryStorageService")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", False, False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "DirectX"
               Select Case item.Text
                  Case "Shader Cache"
                     log.Msg.Info("Clean: Microsoft DirectX: Shader Cache")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Microsoft\DirectX Shader Cache")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", False, False)

                  Case "Direct3D: Most Recent Application"
                     log.Msg.Info("Clean: Microsoft DirectX: Direct3D:Most Recent Application")
                     TaskCleanRegValues(lvMSApps, item, Registry.CurrentUser, "Software\Microsoft\Direct3D\MostRecentApplication", False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "EventViewer"
               Select Case item.Text

                  Case "logs"
                     log.Msg.Info("Clean: Microsoft EventViewer: logs")
                     StopService("eventlog")
                     Await Task.Delay(5000)
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "System32\winevt\Logs")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.evtx", False, False)
                     StartService("eventlog")
               End Select

            '--------------------------------------------------------------------------------------
            Case "Internet Explorer"
               Select Case item.Text

                  Case "Cache"
                     log.Msg.Info("Clean: Microsoft Internet Explorer: Cache")

                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Microsoft\Windows\INetCache\IE"),
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Microsoft\Windows\WebCache")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", True, True)

                  Case "Cookies"
                     log.Msg.Info("Clean: Microsoft Internet Explorer: Cookies")

                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Microsoft\Windows\INetCookies"),
                        RegReadSZ(Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", "Cookies")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", True, True)

                  Case "Temporary Internet Files"
                     log.Msg.Info("Clean: Microsoft Internet Explorer: Temporary Internet Files")

                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Microsoft\Windows\INetCache"),
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Microsoft\Windows\Temporary Internet Files"),
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Temporary Internet Files"),
                        Path.Combine(Environment.GetEnvironmentVariable("UserProfile"), "Local Settings\Temporary Internet Files"),
                        RegReadSZ(Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", "Cache")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", True, True)

                  Case "Typed URLs"
                     log.Msg.Info("Clean: Microsoft Internet Explorer: Typed URLs")
                     TaskCleanRegValues(lvMSApps, item, Registry.CurrentUser, "Software\Microsoft\Internet Explorer\TypedURLs", False)
                     TaskCleanRegValues(lvMSApps, item, Registry.CurrentUser, "Software\Microsoft\Internet Explorer\TypedURLsTime", False)

               End Select

            '--------------------------------------------------------------------------------------
            Case "Microsoft Management Console"
               Select Case item.Text
                  Case "Recent File List"
                     log.Msg.Info("Clean: Microsoft Management Console: Recent File List")
                     TaskCleanRegValues(lvMSApps, item, Registry.CurrentUser, "Software\Microsoft\Microsoft Management Console\Recent File List", False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "OneDrive"
               Select Case item.Text
                  Case "Temp folder"
                     log.Msg.Info("Clean: Microsoft OneDrive: Temp folder")

                     Dim oneDriveTmpPath As String = Path.Combine(Environment.GetEnvironmentVariable("SystemDrive"), "OneDriveTemp")

                     If IsProcessRunning("OneDrive") Then
                        KillProcessByName("OneDrive")
                     End If
                     If IsProcessRunning("FileCoAuth") Then
                        KillProcessByName("FileCoAuth")
                     End If

                     TakeOwnership(oneDriveTmpPath)
                     GrantFullControl(oneDriveTmpPath)

                     Dim pathsToClean As String() = {
                        oneDriveTmpPath
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", True, True, True)
               End Select

            '--------------------------------------------------------------------------------------
            Case "PowerShell"
               Select Case item.Text
                  Case "Console Host History" ' ConsoleHost_history.txt | history_YYYYMMDD.json
                     log.Msg.Info("Clean: Microsoft PowerShell: Console Host History")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("AppData"), "Microsoft\Windows\PowerShell\PSReadLine"),
                        Path.Combine(Environment.GetEnvironmentVariable("AppData"), "Microsoft\PowerShell\PSReadLine")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", False, False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Teams"
               Select Case item.Text
                  Case "Cache"
                     log.Msg.Info("Clean: Microsoft Teams: Cache")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("AppData"), "Microsoft\Teams"),
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Packages\MSTeams_8wekyb3d8bbwe\LocalCache\Microsoft\MSTeams")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", True, True)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Visual Studio"
               Select Case item.Text
                  Case "Telemetry data"
                     log.Msg.Info("Clean: Microsoft Visual Studio: Telemetry data")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("AppData"), "vstelemetry"),
                        Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Temp\VSTelem"),
                        Path.Combine(Environment.GetEnvironmentVariable("ProgramData"), "vstelemetry")
                     }
                     TaskCleanFolders(lvMSApps, item, pathsToClean, "*.*", False, False)
               End Select

          '--------------------------------------------------------------------------------------
            Case "Windows Media Player"
               Select Case item.Text
                  Case "Recent File List"
                     log.Msg.Info("Clean: Microsoft Windows Media Player: Recent File List")
                     TaskCleanRegValues(lvMSApps, item, Registry.CurrentUser, "Software\Microsoft\MediaPlayer\Player\RecentFileList", False)
               End Select

               Select Case item.Text
                  Case "Recent URLs List"
                     log.Msg.Info("Clean: Microsoft Windows Media Player: Recent URLs List")
                     TaskCleanRegValues(lvMSApps, item, Registry.CurrentUser, "Software\Microsoft\MediaPlayer\Player\RecentURLList", False)
               End Select
         End Select
      Next
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSysPurge: onLoad
   Private Sub frmSysPurge_Load(sender As Object, e As EventArgs) Handles Me.Load
      SendMessage(lvMSApps.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, CType(LVS_EX_DOUBLEBUFFER, IntPtr), CType(LVS_EX_DOUBLEBUFFER, IntPtr))
      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' btnTSPurge: onClick
   Private Async Sub btnTSPurge_Click(sender As Object, e As EventArgs) Handles btnMSAppsRun.Click
      ' 1. Gather the items to process on the UI thread
      Dim itemsToProcess As New List(Of ListViewItem)()
      For Each item As ListViewItem In lvMSApps.Items
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
   Private Sub lvSysPurge_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles lvMSApps.DrawColumnHeader
      ' draw column headers with default style
      e.DrawDefault = True
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawItem
   Private Sub lvSysPurge_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles lvMSApps.DrawItem
      ' draw items with default style (except subitem 2 which is handled in DrawSubItem)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawSubItem
   Private Sub lvSysPurge_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles lvMSApps.DrawSubItem
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
      SendMessage(lvMSApps.Handle, LVM_GETSUBITEMRECT, CType(e.ItemIndex, IntPtr), r)

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
         TextRenderer.DrawText(g, text, lvMSApps.Font, rect, Color.Black,
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
      End If
   End Sub

End Class
