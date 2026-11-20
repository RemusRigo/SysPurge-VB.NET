'--------------------------------------------------------------------------------------------------
' SysPurge: frmApps.vb - Clean Known/Installed Applications
'    © 2026 Remus Rigo
'       v1.2.20261120
'--------------------------------------------------------------------------------------------------

Imports System.IO
Imports Microsoft.Win32

Public Class frmApps

   Dim log As New Logger(appName)
   Dim cl As CleanerList

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      Dim appData As String = Environment.GetEnvironmentVariable("AppData")
      Dim localAppData As String = Environment.GetEnvironmentVariable("LocalAppData")

      ' SteamPath uses forward slashes (e.g. "c:/program files (x86)/steam"); empty if Steam isn't installed
      Dim steamPath As String = RegReadSZ(Registry.CurrentUser, "SOFTWARE\Valve\Steam", "SteamPath").Replace("/"c, "\"c)
      Dim steamDefault As String = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "Steam")

      ' Steam sub-folder in the default install folder and in SteamPath (when set, so an empty value
      ' never turns into a relative path). Duplicates are removed by AddItem.
      Dim steamFolders = Function(subFolder As String) As CleanTarget()
                            Dim list As New List(Of CleanTarget) From {New CleanTarget(Path.Combine(steamDefault, subFolder))}
                            If steamPath <> "" Then list.Add(New CleanTarget(Path.Combine(steamPath, subFolder)))
                            Return list.ToArray()
                         End Function

      cl.Build(
         Sub()
            '-------------------------------------------------------------
            cl.AddGroup("Adobe")

            ' Adobe recent file history
            cl.AddRegItem("Adobe recent file history", True, Registry.CurrentUser, "Software\Adobe\MediaBrowser\MRU")

            '-------------------------------------------------------------
            cl.AddGroup("Google")

            ' Crash Reports
            cl.AddItem("Crash Reports", True,
               New CleanTarget(Path.Combine(localAppData, "Google\CrashReports"), "*.log", False, False))

            ' Software Reporter Tool: logs
            cl.AddItem("Software Reporter Tool logs", True,
               New CleanTarget(Path.Combine(localAppData, "Google\Software Reporter Tool"), "*.log", False, False))

            '-------------------------------------------------------------
            cl.AddGroup("Google Chrome")

            ' Crash Reports
            cl.AddItem("Crash Reports", True,
               New CleanTarget(Path.Combine(localAppData, "Google\Chrome\User Data\Crashpad\reports"), "*.log", False, False))

            '-------------------------------------------------------------
            cl.AddGroup("Java")

            ' Cache
            cl.AddItem("cache", True,
               New CleanTarget(Path.Combine(appData, "Sun\Java\Deployment\cache"), "*.*", False, False))

            '-------------------------------------------------------------
            cl.AddGroup("Media Player Classic")

            ' Recent File List
            cl.AddRegItem("Recent File List", True, Registry.CurrentUser, "Software\Gabest\Media Player Classic\Recent File List")

            '-------------------------------------------------------------
            cl.AddGroup("privacy.sexy")

            ' runs
            cl.AddItem("runs", True,
               New CleanTarget(Path.Combine(appData, "privacy.sexy\runs"), "*.*", False, False))

            ' logs
            cl.AddItem("logs", True,
               New CleanTarget(Path.Combine(appData, "privacy.sexy\logs"), "*.*", False, False))

            '-------------------------------------------------------------
            cl.AddGroup("Steam")

            ' AppCache
            cl.AddItem("appcache", True,
               steamFolders("appcache").Concat({New CleanTarget(Path.Combine(localAppData, "Steam\htmlcache"))}).ToArray())

            ' Dumps
            cl.AddItem("Dumps", True, steamFolders("Dumps"))

            ' Traces
            cl.AddItem("Traces", True, steamFolders("Traces"))
         End Sub)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmApps: onLoad
   Private Sub frmApps_Load(sender As Object, e As EventArgs) Handles Me.Load
      Me.Text = appName & " " & appVersion & " " & appAuthor

      cl = New CleanerList(lvApps, log)
      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' btnAppsRun: onClick
   Private Async Sub btnAppsRun_Click(sender As Object, e As EventArgs) Handles btnAppsRun.Click
      btnAppsRun.Enabled = False
      Try
         Await cl.RunAsync()
      Finally
         btnAppsRun.Enabled = True
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmApps: onClosing
   Private Sub frmApps_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
      cl?.CancelSizeScan()
   End Sub

End Class
