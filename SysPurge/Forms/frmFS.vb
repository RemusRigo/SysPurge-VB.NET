'--------------------------------------------------------------------------------------------------
' SysPurge: Main form
'    © 2026 Remus Rigo
'       v1.0.2026-06-12
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Win32

Public Class frmFS

   Private Const SYSMENU_ABOUT_ID As UInteger = 1000

   Dim grp As ListViewGroup = Nothing
   Dim log As New Logger(appName)

   '-----------------------------------------------------------------------------------------------
   ' onHandleCreated - Add "About" to system menu
   Protected Overrides Sub OnHandleCreated(e As EventArgs)
      MyBase.OnHandleCreated(e)
      Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
      ' Add a separator and then your custom item
      AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
      AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "About")
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' WndProc override to handle system menu commands
   Protected Overrides Sub WndProc(ByRef m As Message)
      MyBase.WndProc(m)
      If m.Msg = WM_SYSCOMMAND Then
         If CUInt(m.WParam) = SYSMENU_ABOUT_ID Then
            frmAbout.ShowDialog()
         End If
      End If
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' ResizeColumns: Adjusts the width of the ListView columns
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

   '-----------------------------------------------------------------------------------------------
   ' FormatBytes: Converts a byte count into a human-readable string with appropriate units
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

   '-----------------------------------------------------------------------------------------------
   ' Add ListView Group
   Private Sub LV_AddGroup(name As String)
      grp = New ListViewGroup(name)
      lvSysPurge.Groups.Add(grp)
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
      lvSysPurge.Items.Add(item)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      lvSysPurge.BeginUpdate()
      lvSysPurge.Items.Clear()
      lvSysPurge.Groups.Clear()

      ' Microsoft Windows » FileSystem ------------------------------------------------------------
      LV_AddGroup("Microsoft Windows » FileSystem")
      LV_AddItem("Jump List", True)
      LV_AddItem("Log files (inside Windows)", True)
      LV_AddItem("Log files (System drive)", False)
      LV_AddItem("Prefetch files", True)
      LV_AddItem("Recent Items", True)
      LV_AddItem("Temp folder(s)", True)
      If IsAppElevated() Then LV_AddItem("Windows Update cache", False)

      ' Microsoft Windows » Apps ------------------------------------------------------------------
      LV_AddGroup("Microsoft Windows » Apps")
      If IsAppElevated() Then LV_AddItem("EventViewer logs", True)

      '--------------------------------------------------------------------------------------------

      LV_AddGroup("Microsoft Teams")
      LV_AddItem("Cache", True)

      LV_AddGroup("Microsoft PowerShell")
      LV_AddItem("Console Host History", True)

      LV_AddGroup("Microsoft .NET")
      LV_AddItem("Telemetry data", True)

      LV_AddGroup("Microsoft Visual Studio")
      LV_AddItem("Telemetry data", True)

      '============================================================================================

      LV_AddGroup("Google")
      LV_AddItem("Crash Reports", True)

      LV_AddGroup("Google Chrome")
      LV_AddItem("Crash Reports", True)
      LV_AddItem("Software Reporter Tool: Logs", True)

      '============================================================================================

      LV_AddGroup("Java")
      LV_AddItem("cache", True)

      LV_AddGroup("privacy.sexy")
      LV_AddItem("runs", True)
      LV_AddItem("logs", True)

      '--------------------------------------------------------------------------------------------

      ResizeColumns()
      lvSysPurge.EndUpdate()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Process Actions
   Private Async Sub ProcessActions(itemsToProcess As List(Of ListViewItem))
      For Each item As ListViewItem In itemsToProcess
         Dim grp = item.Group
         If grp Is Nothing Then Continue For

         Select Case grp.Header
            Case "Microsoft Windows » FileSystem" '------------------------------------------------
               Select Case item.Text

                  Case "Jump List"
                     log.Msg.Info("Clean: Microsoft Windows » FileSystem: Jump List")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\Windows\Recent\AutomaticDestinations"),
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\Windows\Recent\CustomDestinations")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.automaticDestinations-ms", False, False)

                  Case "Log files (inside Windows)"
                     log.Msg.Info("Clean: Microsoft Windows » FileSystem: Log files (inside Windows)")
                     Dim pathsToClean As String() = {
                        Environment.GetEnvironmentVariable("SystemRoot")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.log", True, True)

                  Case "Log files (System drive)"
                     log.Msg.Info("Clean: Microsoft Windows » FileSystem: Log files (System drive)")
                     Dim pathsToClean As String() = {
                        Environment.GetEnvironmentVariable("SystemDrive")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.log", True, True)

                  Case "Prefetch files"
                     log.Msg.Info("Clean: Microsoft Windows » FileSystem: Prefetch files")
                     Dim pathsToClean As String() = {Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "Prefetch")}
                     TaskCleanFolders(item, pathsToClean, "*.pf", False, False)

                  Case "Recent files"
                     log.Msg.Info("Clean: Microsoft Windows » FileSystem: Recent Items")
                     Dim pathsToClean As String() = {Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\Windows\Recent")}
                     TaskCleanFolders(item, pathsToClean, "*.*", False, False)

                  Case "Temp folder(s)"
                     log.Msg.Info("Clean: Microsoft Windows » FileSystem: Temp folder(s)")
                     Dim pathsToClean As String() = {
                        Environment.GetEnvironmentVariable("TEMP"),
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "Temp")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", True, True)

                  Case "Windows Update cache"
                     log.Msg.Info("Clean: Microsoft Windows » FileSystem: Windows Update cache")
                     StopService("wuauserv")
                     StopService("bits")
                     StopService("cryptsvc")
                     StopService("msiserver")
                     Await Task.Delay(5000)
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "SoftwareDistribution\Download"),
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "SoftwareDistribution\DataStore")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", True, True)
                     StartService("msiserver")
                     StartService("cryptsvc")
                     StartService("bits")
                     StartService("wuauserv")

               End Select

            '--------------------------------------------------------------------------------------
            Case "Microsoft Windows » Apps"
               Select Case item.Text

                  Case "EventViewer logs"
                     log.Msg.Info("Clean: Microsoft Windows » Apps: EventViewer logs")
                     StopService("eventlog")
                     Await Task.Delay(5000)
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "System32\winevt\Logs")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.evtx", False, False)
                     StartService("eventlog")
               End Select

            '--------------------------------------------------------------------------------------
            Case "Microsoft Teams"
               Select Case item.Text
                  Case "Cache"
                     log.Msg.Info("Clean: Microsoft Teams: Cache")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\Teams"),
                        Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "Packages\MSTeams_8wekyb3d8bbwe\LocalCache\Microsoft\MSTeams")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", True, True)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Microsoft PowerShell"
               Select Case item.Text
                  Case "Console Host History" ' ConsoleHost_history.txt | history_YYYYMMDD.json
                     log.Msg.Info("Clean: Microsoft PowerShell: Console Host History")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\Windows\PowerShell\PSReadLine"),
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Microsoft\PowerShell\PSReadLine")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", False, False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Microsoft .NET"
               Select Case item.Text
                  Case "Telemetry data"
                     log.Msg.Info("Clean: Microsoft .NET: Telemetry data")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".dotnet\TelemetryStorageService")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", False, False)
               End Select

            '--------------------------------------------------------------------------------------
            Case "Microsoft Visual Studio"
               Select Case item.Text
                  Case "Telemetry data"
                     log.Msg.Info("Clean: Microsoft Visual Studio: Telemetry data")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "vstelemetry"),
                        Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Temp\VSTelem"),
                        Path.Combine(Environment.GetEnvironmentVariable("%PROGRAMDATA"), "vstelemetry")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", False, False)
               End Select

            '======================================================================================
            Case "Google"
               Select Case item.Text
                  Case "Crash Reports"
                     log.Msg.Info("Clean: Google: Crash Reports")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Google\CrashReports")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.log", False, False)
               End Select

            Case "Google Chrome"
               Select Case item.Text
                  Case "Crash Reports"
                     log.Msg.Info("Clean: Google Chrome: Crash Reports")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Google\Chrome\User Data\Crashpad\reports")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.log", False, False)
                  Case "Software Reporter Tool: Logs"
                     log.Msg.Info("Clean: Google Chrome: Software Reporter Tool: Logs")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Google\Software Reporter Tool")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.log", False, False)
               End Select

            '======================================================================================
            Case "Java"
               Select Case item.Text
                  Case "cache"
                     log.Msg.Info("Clean: Java: cache")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Sun\Java\Deployment\cache")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", False, False)

               End Select

            Case "privacy.sexy"
               Select Case item.Text
                  Case "runs"
                     log.Msg.Info("Clean: privacy.sexy: runs")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "privacy.sexy\runs")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", False, False)
                  Case "logs"
                     log.Msg.Info("Clean: privacy.sexy: logs")
                     Dim pathsToClean As String() = {
                        Path.Combine(Environment.GetEnvironmentVariable("appdata"), "privacy.sexy\logs")
                     }
                     TaskCleanFolders(item, pathsToClean, "*.*", False, False)
               End Select

         End Select
      Next
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' SetTaskProgressBytes: Updates the progress bytes for a task
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

   '-----------------------------------------------------------------------------------------------
   ' SetTaskProgressCount: Updates the progress count for a task
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

   '-----------------------------------------------------------------------------------------------
   ' Task: Clean Folders
   Private Sub TaskCleanFolders(item As ListViewItem, folderPaths As IEnumerable(Of String), mask As String, recursive As Boolean, deleteFolders As Boolean)
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
               SetTaskProgressBytes(item, totalDeletedBytes, prog)
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

      SetTaskProgressBytes(item, totalDeletedBytes, 100)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSysPurge: onLoad
   Private Sub frmSysPurge_Load(sender As Object, e As EventArgs) Handles Me.Load
      Me.Text = appName & " " & appVersion & " " & appAuthor
      SendMessage(lvSysPurge.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, CType(LVS_EX_DOUBLEBUFFER, IntPtr), CType(LVS_EX_DOUBLEBUFFER, IntPtr))

      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' btnTSPurge: onClick
   Private Async Sub btnTSPurge_Click(sender As Object, e As EventArgs) Handles btnTSPurge.Click
      ' 1. Gather the items to process on the UI thread
      Dim itemsToProcess As New List(Of ListViewItem)()
      For Each item As ListViewItem In lvSysPurge.Items
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
   Private Sub lvSysPurge_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles lvSysPurge.DrawColumnHeader
      ' draw column headers with default style
      e.DrawDefault = True
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawItem
   Private Sub lvSysPurge_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles lvSysPurge.DrawItem
      ' draw items with default style (except subitem 2 which is handled in DrawSubItem)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawSubItem
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

End Class
