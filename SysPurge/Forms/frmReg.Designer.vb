<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReg
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Dispose(disposing As Boolean)
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
   <System.Diagnostics.DebuggerStepThrough()>
   Private Sub InitializeComponent()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReg))
        Me.lvReg = New BufferedListView()
        Me.btnTSReg = New System.Windows.Forms.ToolStrip()
        Me.btnRegRun = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.btnTSReg.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvReg
        '
        Me.lvReg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvReg.CheckBoxes = True
        Me.lvReg.FullRowSelect = True
        Me.lvReg.HideSelection = False
        Me.lvReg.Location = New System.Drawing.Point(0, 24)
        Me.lvReg.Name = "lvReg"
        Me.lvReg.OwnerDraw = True
        Me.lvReg.Size = New System.Drawing.Size(692, 275)
        Me.lvReg.TabIndex = 0
        Me.lvReg.UseCompatibleStateImageBehavior = False
        Me.lvReg.View = System.Windows.Forms.View.Details
        '
        'btnTSReg
        '
        Me.btnTSReg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnRegRun})
        Me.btnTSReg.Location = New System.Drawing.Point(0, 0)
        Me.btnTSReg.Name = "btnTSReg"
        Me.btnTSReg.Size = New System.Drawing.Size(693, 25)
        Me.btnTSReg.TabIndex = 1
        Me.btnTSReg.Text = "ToolStrip1"
        '
        'btnRegRun
        '
        Me.btnRegRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnRegRun.Image = CType(resources.GetObject("btnRegRun.Image"), System.Drawing.Image)
        Me.btnRegRun.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRegRun.Name = "btnRegRun"
        Me.btnRegRun.Size = New System.Drawing.Size(23, 22)
        Me.btnRegRun.Text = "ToolStripButton1"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 299)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 12, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(693, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'frmReg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(693, 321)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnTSReg)
        Me.Controls.Add(Me.lvReg)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmReg"
        Me.Text = "SysPurge"
        Me.btnTSReg.ResumeLayout(False)
        Me.btnTSReg.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvReg As BufferedListView
    Friend WithEvents btnTSReg As ToolStrip
    Friend WithEvents btnRegRun As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
