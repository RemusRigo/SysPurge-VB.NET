'--------------------------------------------------------------------------------------------------
' SysPurge: frmMSApps.vb - Clean Microsoft Apps
'    © 2026 Remus Rigo
'       v1.2.20261120
'--------------------------------------------------------------------------------------------------

Imports System.IO
Imports Microsoft.Win32

Public Class frmMSApps

   Dim log As New Logger(appName)
   Dim cl As CleanerList

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      Dim appData As String = Environment.GetEnvironmentVariable("AppData")
      Dim localAppData As String = Environment.GetEnvironmentVariable("LocalAppData")
      Dim userProfile As String = Environment.GetEnvironmentVariable("UserProfile")
      Dim shellFolders As String = "Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders"

      cl.Build(
         Sub()
            '-------------------------------------------------------------
            cl.AddGroup(".NET")

            ' Telemetry data
            cl.AddItem("Telemetry data", True,
               New CleanTarget(Path.Combine(userProfile, ".dotnet\TelemetryStorageService"), "*.*", False, False))

            '-------------------------------------------------------------
            cl.AddGroup("EventViewer")

            ' Logs
            ' The EventLog service can't be stopped (the .evtx files stay locked), so clear each log through the Event Log API instead (same as "wevtutil cl").
            If IsAppElevated() Then
               cl.AddCustomItem("Logs", True, AddressOf TaskClearEventLogs, AddressOf CleanerList.GetEventLogsSize)
            End If

            '-------------------------------------------------------------
            cl.AddGroup("DirectX")

            ' Shader Cache
            cl.AddItem("Shader Cache", True,
               New CleanTarget(Path.Combine(localAppData, "Microsoft\DirectX Shader Cache"), "*.*", False, False))
            cl.AddRegItem("Direct3D: Most Recent Application", True, Registry.CurrentUser, "Software\Microsoft\Direct3D\MostRecentApplication")

            '-------------------------------------------------------------
            cl.AddGroup("Internet Explorer")

            ' Cache
            cl.AddItem("Cache", True,
               New CleanTarget(Path.Combine(localAppData, "Microsoft\Windows\INetCache\IE")),
               New CleanTarget(Path.Combine(localAppData, "Microsoft\Windows\WebCache")))

            ' Cookies
            cl.AddItem("Cookies", True,
               New CleanTarget(Path.Combine(localAppData, "Microsoft\Windows\INetCookies")),
               New CleanTarget(RegReadSZ(Registry.CurrentUser, shellFolders, "Cookies")))

            ' Temporary Internet Files
            cl.AddItem("Temporary Internet Files", True,
               New CleanTarget(Path.Combine(localAppData, "Microsoft\Windows\INetCache")),
               New CleanTarget(Path.Combine(localAppData, "Microsoft\Windows\Temporary Internet Files")),
               New CleanTarget(Path.Combine(localAppData, "Temporary Internet Files")),
               New CleanTarget(Path.Combine(userProfile, "Local Settings\Temporary Internet Files")),
               New CleanTarget(RegReadSZ(Registry.CurrentUser, shellFolders, "Cache")))

            ' Typed URLs
            With cl.AddRegItem("Typed URLs", True, Registry.CurrentUser, "Software\Microsoft\Internet Explorer\TypedURLs")
               ' TypedURLsTime only holds timestamps for the TypedURLs entries: clear it silently, so the row shows the TypedURLs count
               .BeforeClean = Sub() RegDeleteAllValues(Registry.CurrentUser, "Software\Microsoft\Internet Explorer\TypedURLsTime", False)
            End With

            '-------------------------------------------------------------
            cl.AddGroup("Microsoft Management Console")

            ' Recent File List
            cl.AddRegItem("Recent File List", True, Registry.CurrentUser, "Software\Microsoft\Microsoft Management Console\Recent File List")

            '-------------------------------------------------------------
            cl.AddGroup("OneDrive")

            ' Temp folder
            If IsAppElevated() Then
               Dim oneDriveTmpPath As String = Path.Combine(Environment.GetEnvironmentVariable("SystemDrive") & "\", "OneDriveTemp")
               With cl.AddItem("Temp folder", True, New CleanTarget(oneDriveTmpPath, includeParent:=True))
                  .BeforeClean = Sub()
                                    If Not Directory.Exists(oneDriveTmpPath) Then Return
                                    If IsProcessRunning("OneDrive") Then KillProcessByName("OneDrive")
                                    If IsProcessRunning("FileCoAuth") Then KillProcessByName("FileCoAuth")
                                    TakeOwnership(oneDriveTmpPath)
                                    GrantFullControl(oneDriveTmpPath)
                                 End Sub
               End With
            End If

            '--------------------------------------------------------------
            cl.AddGroup("PowerShell")

            ' Console Host History
            ' ConsoleHost_history.txt | history_YYYYMMDD.json
            cl.AddItem("Console Host History", True,
               New CleanTarget(Path.Combine(appData, "Microsoft\Windows\PowerShell\PSReadLine"), "*.*", False, False),
               New CleanTarget(Path.Combine(appData, "Microsoft\PowerShell\PSReadLine"), "*.*", False, False))

            '--------------------------------------------------------------
            cl.AddGroup("Teams")

            ' Cache
            cl.AddItem("Cache", True,
               New CleanTarget(Path.Combine(appData, "Microsoft\Teams")),
               New CleanTarget(Path.Combine(localAppData, "Packages\MSTeams_8wekyb3d8bbwe\LocalCache\Microsoft\MSTeams")))

            '--------------------------------------------------------------
            cl.AddGroup("Visual Studio")

            ' Telemetry data
            cl.AddItem("Telemetry data", True,
               New CleanTarget(Path.Combine(appData, "vstelemetry"), "*.*", False, False),
               New CleanTarget(Path.Combine(localAppData, "Temp\VSTelem"), "*.*", False, False),
               New CleanTarget(Path.Combine(Environment.GetEnvironmentVariable("ProgramData"), "vstelemetry"), "*.*", False, False))

            '--------------------------------------------------------------
            cl.AddGroup("Windows Media Player")

            ' Recent File List
            cl.AddRegItem("Recent File List", True, Registry.CurrentUser, "Software\Microsoft\MediaPlayer\Player\RecentFileList")

            ' Recent URLs List
            cl.AddRegItem("Recent URLs List", True, Registry.CurrentUser, "Software\Microsoft\MediaPlayer\Player\RecentURLList")
         End Sub)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmMSApps: onLoad
   Private Sub frmMSApps_Load(sender As Object, e As EventArgs) Handles Me.Load
      cl = New CleanerList(lvMSApps, log)
      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' btnMSAppsRun: onClick
   Private Async Sub btnMSAppsRun_Click(sender As Object, e As EventArgs) Handles btnMSAppsRun.Click
      btnMSAppsRun.Enabled = False
      Try
         Await cl.RunAsync()
      Finally
         btnMSAppsRun.Enabled = True
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmMSApps: onClosing
   Private Sub frmMSApps_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
      cl?.CancelSizeScan()
   End Sub

End Class
