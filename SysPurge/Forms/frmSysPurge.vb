'--------------------------------------------------------------------------------------------------
' SysPurge: Cleaner for Windows
'    © 2026 Remus Rigo
'       v1.1.20260724
'--------------------------------------------------------------------------------------------------

Public Class frmSysPurge

   Public Sub ProcessOptions(frm As Form)
      scSysPurge.Panel2.Controls.Clear()
      Dim frmChild As Form = frm

      If frmChild IsNot Nothing Then
         frmChild.TopLevel = False
         frmChild.FormBorderStyle = FormBorderStyle.None
         frmChild.Dock = DockStyle.Fill
         scSysPurge.Panel2.Controls.Add(frmChild)
         frmChild.Show()
      End If
   End Sub

   Private Sub frmSysPurge_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      Me.Text = appTitle
   End Sub

   Private Sub btnFS_Click(sender As Object, e As EventArgs) Handles btnFS.Click
      ProcessOptions(frmFS)
   End Sub

   Private Sub btnReg_Click(sender As Object, e As EventArgs) Handles btnReg.Click
      ProcessOptions(frmReg)
   End Sub

   Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnMSApps.Click
      ProcessOptions(frmMSApps)
   End Sub

   Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnApps.Click
      ProcessOptions(frmApps)
   End Sub
End Class