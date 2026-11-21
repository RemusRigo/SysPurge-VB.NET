'--------------------------------------------------------------------------------------------------
' SysPurge: frmFS.vb - Clean File System
'    © 2026 Remus Rigo
'       v1.2.20261120
'--------------------------------------------------------------------------------------------------

Imports System.IO
Imports System.Threading
Imports Microsoft.Win32

Public Class frmFS

   Dim log As New Logger(appName)
   Dim cl As CleanerList

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      Dim winDir As String = Environment.GetEnvironmentVariable("SystemRoot")
      Dim appData As String = Environment.GetEnvironmentVariable("AppData")
      Dim localAppData As String = Environment.GetEnvironmentVariable("LocalAppData")

      cl.Build(
         Sub()

            '-------------------------------------------------------------
            cl.AddGroup("Temporary/Junk files")

            ' User Temp folder
            cl.AddItem("User Temp folder", True,
               New CleanTarget(Environment.GetEnvironmentVariable("TEMP")),
               New CleanTarget(Path.Combine(localAppData, "Temp"))
               )

            ' System Temp folder
            cl.AddItem("System Temp folder", True,
               New CleanTarget(Path.Combine(winDir, "Temp")))

            ' Log files
            cl.AddItem("Log files", True,
               New CleanTarget(winDir, "*.log", False, False),
               New CleanTarget(Path.Combine(winDir, "Debug"), "*.log", True, False),
               New CleanTarget(Path.Combine(winDir, "Logs"), "*.log", True, False),
               New CleanTarget(Path.Combine(winDir, "Panther"), "*.log", True, False),
               New CleanTarget(Path.Combine(winDir, "System32\LogFiles"), "*.log", True, False))

            ' Previous Windows installation
            If IsAppElevated() Then
               Dim prevWinDir As String = Path.Combine(Environment.GetEnvironmentVariable("SystemDrive") & "\", "Windows.old")
               With cl.AddItem("Previous Windows installation", True,
                     New CleanTarget(prevWinDir, includeParent:=True))
                  .BeforeClean = Sub()
                                    If Not Directory.Exists(prevWinDir) Then Return
                                    Dim failed As Integer = PrepareTreeForDeletion(prevWinDir, Sub(p, ex) log.Msg.Warning($"{ex.Message} : {p}"))
                                    If failed > 0 Then log.Msg.Warning($"Windows.old: {failed} item(s) could not be prepared for deletion")
                                 End Sub
               End With
            End If

            '-------------------------------------------------------------
            cl.AddGroup("Microsoft Windows FileSystem")

            ' Jump List
            cl.AddItem("Jump List", False,
               New CleanTarget(Path.Combine(appData, "Microsoft\Windows\Recent\AutomaticDestinations"), "*.automaticDestinations-ms", False, False),
               New CleanTarget(Path.Combine(appData, "Microsoft\Windows\Recent\CustomDestinations"), "*.customDestinations-ms", False, False))

            ' Prefetch files
            cl.AddItem("Prefetch files", True,
               New CleanTarget(Path.Combine(winDir, "Prefetch"), "*.pf", False, False))

            ' Recent Items
            cl.AddItem("Recent Items", False,
               New CleanTarget(Path.Combine(appData, "Microsoft\Windows\Recent"), "*.*", False, False),
               New CleanTarget(RegReadSZ(Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", "Recent"), "*.*", False, False))

            ' Recycle Bin
            cl.AddCustomItem("Recycle Bin", False, AddressOf TaskEmptyRecycleBin, AddressOf GetRecycleBinSizeText)

            ' Windows Update cache
            If IsAppElevated() Then
               With cl.AddItem("Windows Update cache", False,
                     New CleanTarget(Path.Combine(winDir, "SoftwareDistribution\Download")),
                     New CleanTarget(Path.Combine(winDir, "SoftwareDistribution\DataStore")))
                  .BeforeClean = Sub()
                                    StopService("wuauserv")
                                    StopService("bits")
                                    StopService("cryptsvc")
                                    StopService("msiserver")
                                    Thread.Sleep(5000)
                                 End Sub
                  .AfterClean = Sub()
                                   StartService("msiserver")
                                   StartService("cryptsvc")
                                   StartService("bits")
                                   StartService("wuauserv")
                                End Sub
               End With
            End If
         End Sub)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmFS: onLoad
   Private Sub frmFS_Load(sender As Object, e As EventArgs) Handles Me.Load
      Me.Text = appName & " " & appVersion & " " & appAuthor

      cl = New CleanerList(lvFS, log)
      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tsFS_btnRun: onClick
   Private Async Sub tsFS_btnRun_Click(sender As Object, e As EventArgs) Handles tsFS_btnRun.Click
      tsFS_btnRun.Enabled = False
      Try
         Await cl.RunAsync()
      Finally
         tsFS_btnRun.Enabled = True
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmFS: onClosing
   Private Sub frmFS_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
      cl?.CancelSizeScan()
   End Sub

End Class
