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
        Me.scSysPurge = New System.Windows.Forms.SplitContainer()
        Me.btnApps = New System.Windows.Forms.Button()
        Me.btnMSApps = New System.Windows.Forms.Button()
        Me.btnReg = New System.Windows.Forms.Button()
        Me.btnFS = New System.Windows.Forms.Button()
        CType(Me.scSysPurge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scSysPurge.Panel1.SuspendLayout()
        Me.scSysPurge.SuspendLayout()
        Me.SuspendLayout()
        '
        'scSysPurge
        '
        Me.scSysPurge.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scSysPurge.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scSysPurge.IsSplitterFixed = True
        Me.scSysPurge.Location = New System.Drawing.Point(0, 0)
        Me.scSysPurge.Name = "scSysPurge"
        '
        'scSysPurge.Panel1
        '
        Me.scSysPurge.Panel1.Controls.Add(Me.btnApps)
        Me.scSysPurge.Panel1.Controls.Add(Me.btnMSApps)
        Me.scSysPurge.Panel1.Controls.Add(Me.btnReg)
        Me.scSysPurge.Panel1.Controls.Add(Me.btnFS)
        Me.scSysPurge.Size = New System.Drawing.Size(843, 400)
        Me.scSysPurge.SplitterDistance = 118
        Me.scSysPurge.SplitterWidth = 3
        Me.scSysPurge.TabIndex = 0
        '
        'btnApps
        '
        Me.btnApps.Image = CType(resources.GetObject("btnApps.Image"), System.Drawing.Image)
        Me.btnApps.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApps.Location = New System.Drawing.Point(5, 91)
        Me.btnApps.Name = "btnApps"
        Me.btnApps.Size = New System.Drawing.Size(103, 29)
        Me.btnApps.TabIndex = 4
        Me.btnApps.Text = "Apps"
        Me.btnApps.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApps.UseVisualStyleBackColor = True
        '
        'btnMSApps
        '
        Me.btnMSApps.Image = CType(resources.GetObject("btnMSApps.Image"), System.Drawing.Image)
        Me.btnMSApps.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMSApps.Location = New System.Drawing.Point(5, 62)
        Me.btnMSApps.Name = "btnMSApps"
        Me.btnMSApps.Size = New System.Drawing.Size(103, 29)
        Me.btnMSApps.TabIndex = 3
        Me.btnMSApps.Text = "MS Apps"
        Me.btnMSApps.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMSApps.UseVisualStyleBackColor = True
        '
        'btnReg
        '
        Me.btnReg.Image = CType(resources.GetObject("btnReg.Image"), System.Drawing.Image)
        Me.btnReg.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReg.Location = New System.Drawing.Point(5, 32)
        Me.btnReg.Name = "btnReg"
        Me.btnReg.Size = New System.Drawing.Size(103, 29)
        Me.btnReg.TabIndex = 2
        Me.btnReg.Text = "Registry"
        Me.btnReg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReg.UseVisualStyleBackColor = True
        '
        'btnFS
        '
        Me.btnFS.Image = CType(resources.GetObject("btnFS.Image"), System.Drawing.Image)
        Me.btnFS.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFS.Location = New System.Drawing.Point(5, 3)
        Me.btnFS.Name = "btnFS"
        Me.btnFS.Size = New System.Drawing.Size(103, 29)
        Me.btnFS.TabIndex = 1
        Me.btnFS.Text = "File System"
        Me.btnFS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFS.UseVisualStyleBackColor = True
        '
        'frmSysPurge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(843, 400)
        Me.Controls.Add(Me.scSysPurge)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSysPurge"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SysPurge"
        Me.scSysPurge.Panel1.ResumeLayout(False)
        CType(Me.scSysPurge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scSysPurge.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents scSysPurge As SplitContainer
   Friend WithEvents btnFS As Button
   Friend WithEvents btnApps As Button
   Friend WithEvents btnMSApps As Button
   Friend WithEvents btnReg As Button
End Class
