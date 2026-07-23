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
      btnFS = New Button()
      Button1 = New Button()
      Button2 = New Button()
      Button3 = New Button()
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
      scSysPurge.Panel1.Controls.Add(Button3)
      scSysPurge.Panel1.Controls.Add(Button2)
      scSysPurge.Panel1.Controls.Add(Button1)
      scSysPurge.Panel1.Controls.Add(btnFS)
      scSysPurge.Panel1.Controls.Add(tvOptions)
      scSysPurge.Size = New Size(800, 450)
      scSysPurge.SplitterDistance = 74
      scSysPurge.TabIndex = 0
      ' 
      ' tvOptions
      ' 
      tvOptions.Location = New Point(3, 318)
      tvOptions.Name = "tvOptions"
      TreeNode1.Name = "Node0"
      TreeNode1.Text = "File System"
      TreeNode2.Name = "Node1"
      TreeNode2.Text = "Registry"
      TreeNode3.Name = "Node2"
      TreeNode3.Text = "Microsoft Windows"
      tvOptions.Nodes.AddRange(New TreeNode() {TreeNode3})
      tvOptions.Size = New Size(56, 204)
      tvOptions.TabIndex = 0
      ' 
      ' btnFS
      ' 
      btnFS.Image = My.Resources.Resources.Explorer
      btnFS.Location = New Point(3, 3)
      btnFS.Name = "btnFS"
      btnFS.Size = New Size(64, 64)
      btnFS.TabIndex = 1
      btnFS.UseVisualStyleBackColor = True
      ' 
      ' Button1
      ' 
      Button1.Image = My.Resources.Resources.Registry
      Button1.Location = New Point(3, 73)
      Button1.Name = "Button1"
      Button1.Size = New Size(64, 64)
      Button1.TabIndex = 2
      Button1.UseVisualStyleBackColor = True
      ' 
      ' Button2
      ' 
      Button2.Image = My.Resources.Resources.Windows
      Button2.Location = New Point(3, 143)
      Button2.Name = "Button2"
      Button2.Size = New Size(64, 64)
      Button2.TabIndex = 3
      Button2.UseVisualStyleBackColor = True
      ' 
      ' Button3
      ' 
      Button3.Image = My.Resources.Resources.Apps
      Button3.Location = New Point(3, 213)
      Button3.Name = "Button3"
      Button3.Size = New Size(64, 64)
      Button3.TabIndex = 4
      Button3.UseVisualStyleBackColor = True
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
   Friend WithEvents btnFS As Button
   Friend WithEvents Button3 As Button
   Friend WithEvents Button2 As Button
   Friend WithEvents Button1 As Button
End Class
