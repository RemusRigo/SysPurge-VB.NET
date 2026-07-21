Public Class frmSysPurge

   Public Sub ProcessOptions(path As String)
      Dim frmChild As Form = Nothing
      scSysPurge.Panel2.Controls.Clear()
      Select Case path
         Case "Microsoft Windows\File System"
            frmChild = New frmFS

         Case "Microsoft Windows\Registry"
            frmChild = New frmReg

         Case Else
      End Select

      If frmChild IsNot Nothing Then
         frmChild.TopLevel = False
         frmChild.FormBorderStyle = FormBorderStyle.None
         frmChild.Dock = DockStyle.Fill
         scSysPurge.Panel2.Controls.Add(frmChild)
         frmChild.Show()
      End If
   End Sub

   Private Sub tvOptions_DoubleClick(sender As Object, e As EventArgs) Handles tvOptions.DoubleClick
      ProcessOptions(tvOptions.SelectedNode.FullPath)
   End Sub

   Private Sub tvOptions_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvOptions.NodeMouseDoubleClick
      e.Node.Toggle()
   End Sub

   Private Sub frmSysPurge_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      Me.Text = appTitle
   End Sub

End Class