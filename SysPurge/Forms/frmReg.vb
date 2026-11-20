'--------------------------------------------------------------------------------------------------
' SysPurge: frmReg.vb - Registry cleaner
'    © 2026 Remus Rigo
'       v1.2.20261120
'--------------------------------------------------------------------------------------------------

Imports Microsoft.Win32

Public Class frmReg

   Dim log As New Logger(appName)
   Dim cl As CleanerList

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      cl.Build(
         Sub()
            '-------------------------------------------------------------
            cl.AddGroup("MRU")

            ' Run MRU
            cl.AddRegItem("Run MRU", True, Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU")

            '-------------------------------------------------------------
            cl.AddGroup("Misc")

            ' Shared DLLs
            If IsAppElevated() Then cl.AddItem("Shared DLLs", False)
         End Sub)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmReg: onLoad
   Private Sub frmReg_Load(sender As Object, e As EventArgs) Handles Me.Load
      Me.Text = appName & " " & appVersion & " " & appAuthor

      cl = New CleanerList(lvReg, log)
      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' btnRegRun: onClick
   Private Async Sub btnRegRun_Click(sender As Object, e As EventArgs) Handles btnRegRun.Click
      btnRegRun.Enabled = False
      Try
         Await cl.RunAsync()
      Finally
         btnRegRun.Enabled = True
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmReg: onClosing
   Private Sub frmReg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
      cl?.CancelSizeScan()
   End Sub

End Class
