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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSysPurge))
      scSysPurge = New SplitContainer()
      btnApps = New Button()
      btnMSApps = New Button()
      btnReg = New Button()
      btnFS = New Button()
      CType(scSysPurge, ComponentModel.ISupportInitialize).BeginInit()
      scSysPurge.Panel1.SuspendLayout()
      scSysPurge.SuspendLayout()
      SuspendLayout()
      ' 
      ' scSysPurge
      ' 
      scSysPurge.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      scSysPurge.FixedPanel = FixedPanel.Panel1
      scSysPurge.IsSplitterFixed = True
      scSysPurge.Location = New Point(0, 0)
      scSysPurge.Name = "scSysPurge"
      ' 
      ' scSysPurge.Panel1
      ' 
      scSysPurge.Panel1.Controls.Add(btnApps)
      scSysPurge.Panel1.Controls.Add(btnMSApps)
      scSysPurge.Panel1.Controls.Add(btnReg)
      scSysPurge.Panel1.Controls.Add(btnFS)
      scSysPurge.Size = New Size(984, 461)
      scSysPurge.SplitterDistance = 128
      scSysPurge.TabIndex = 0
      ' 
      ' btnApps
      ' 
      btnApps.Image = CType(resources.GetObject("btnApps.Image"), Image)
      btnApps.ImageAlign = ContentAlignment.MiddleRight
      btnApps.Location = New Point(6, 105)
      btnApps.Name = "btnApps"
      btnApps.Size = New Size(120, 34)
      btnApps.TabIndex = 4
      btnApps.Text = "Apps"
      btnApps.TextAlign = ContentAlignment.MiddleLeft
      btnApps.UseVisualStyleBackColor = True
      ' 
      ' btnMSApps
      ' 
      btnMSApps.Image = CType(resources.GetObject("btnMSApps.Image"), Image)
      btnMSApps.ImageAlign = ContentAlignment.MiddleRight
      btnMSApps.Location = New Point(6, 71)
      btnMSApps.Name = "btnMSApps"
      btnMSApps.Size = New Size(120, 34)
      btnMSApps.TabIndex = 3
      btnMSApps.Text = "MS Apps"
      btnMSApps.TextAlign = ContentAlignment.MiddleLeft
      btnMSApps.UseVisualStyleBackColor = True
      ' 
      ' btnReg
      ' 
      btnReg.Image = CType(resources.GetObject("btnReg.Image"), Image)
      btnReg.ImageAlign = ContentAlignment.MiddleRight
      btnReg.Location = New Point(6, 37)
      btnReg.Name = "btnReg"
      btnReg.Size = New Size(120, 34)
      btnReg.TabIndex = 2
      btnReg.Text = "Registry"
      btnReg.TextAlign = ContentAlignment.MiddleLeft
      btnReg.UseVisualStyleBackColor = True
      ' 
      ' btnFS
      ' 
      btnFS.Image = CType(resources.GetObject("btnFS.Image"), Image)
      btnFS.ImageAlign = ContentAlignment.MiddleRight
      btnFS.Location = New Point(6, 3)
      btnFS.Name = "btnFS"
      btnFS.Size = New Size(120, 34)
      btnFS.TabIndex = 1
      btnFS.Text = "File System"
      btnFS.TextAlign = ContentAlignment.MiddleLeft
      btnFS.UseVisualStyleBackColor = True
      ' 
      ' frmSysPurge
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(984, 461)
      Controls.Add(scSysPurge)
      Name = "frmSysPurge"
      Text = "SysPurge"
      scSysPurge.Panel1.ResumeLayout(False)
      CType(scSysPurge, ComponentModel.ISupportInitialize).EndInit()
      scSysPurge.ResumeLayout(False)
      ResumeLayout(False)
   End Sub

   Friend WithEvents scSysPurge As SplitContainer
   Friend WithEvents btnFS As Button
   Friend WithEvents btnApps As Button
   Friend WithEvents btnMSApps As Button
   Friend WithEvents btnReg As Button
End Class
