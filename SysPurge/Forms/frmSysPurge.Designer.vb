<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSysPurge
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()> _
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
         If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
      Dim TreeNode1 As TreeNode = New TreeNode("File System")
      Dim TreeNode2 As TreeNode = New TreeNode("Registry")
      Dim TreeNode3 As TreeNode = New TreeNode("Microsoft Windows", New TreeNode() {TreeNode1, TreeNode2})
      scSysPurge = New SplitContainer()
      tvOptions = New TreeView()
      CType(scSysPurge, ComponentModel.ISupportInitialize).BeginInit()
      scSysPurge.Panel1.SuspendLayout()
      scSysPurge.SuspendLayout()
      SuspendLayout()
      ' 
      ' scSysPurge
      ' 
      scSysPurge.Dock = DockStyle.Fill
      scSysPurge.Location = New Point(0, 0)
      scSysPurge.Name = "scSysPurge"
      ' 
      ' scSysPurge.Panel1
      ' 
      scSysPurge.Panel1.Controls.Add(tvOptions)
      scSysPurge.Size = New Size(800, 450)
      scSysPurge.SplitterDistance = 162
      scSysPurge.TabIndex = 0
      ' 
      ' tvOptions
      ' 
      tvOptions.Dock = DockStyle.Fill
      tvOptions.Location = New Point(0, 0)
      tvOptions.Name = "tvOptions"
      TreeNode1.Name = "Node0"
      TreeNode1.Text = "File System"
      TreeNode2.Name = "Node1"
      TreeNode2.Text = "Registry"
      TreeNode3.Name = "Node2"
      TreeNode3.Text = "Microsoft Windows"
      tvOptions.Nodes.AddRange(New TreeNode() {TreeNode3})
      tvOptions.Size = New Size(162, 450)
      tvOptions.TabIndex = 0
      ' 
      ' frmSysPurge
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(scSysPurge)
      Name = "frmSysPurge"
      Text = "Form1"
      scSysPurge.Panel1.ResumeLayout(False)
      CType(scSysPurge, ComponentModel.ISupportInitialize).EndInit()
      scSysPurge.ResumeLayout(False)
      ResumeLayout(False)
   End Sub

   Friend WithEvents scSysPurge As SplitContainer
   Friend WithEvents tvOptions As TreeView
End Class
