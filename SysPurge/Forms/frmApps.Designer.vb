<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmApps
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmApps))
        Me.lvApps = New BufferedListView()
        Me.tsApps = New System.Windows.Forms.ToolStrip()
        Me.btnAppsRun = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsApps.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvApps
        '
        Me.lvApps.CheckBoxes = True
        Me.lvApps.FullRowSelect = True
        Me.lvApps.HideSelection = False
        Me.lvApps.Location = New System.Drawing.Point(0, 24)
        Me.lvApps.Name = "lvApps"
        Me.lvApps.OwnerDraw = True
        Me.lvApps.Size = New System.Drawing.Size(692, 275)
        Me.lvApps.TabIndex = 0
        Me.lvApps.UseCompatibleStateImageBehavior = False
        Me.lvApps.View = System.Windows.Forms.View.Details
        '
        'tsApps
        '
        Me.tsApps.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAppsRun})
        Me.tsApps.Location = New System.Drawing.Point(0, 0)
        Me.tsApps.Name = "tsApps"
        Me.tsApps.Size = New System.Drawing.Size(693, 25)
        Me.tsApps.TabIndex = 1
        Me.tsApps.Text = "ToolStrip1"
        '
        'btnAppsRun
        '
        Me.btnAppsRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAppsRun.Image = CType(resources.GetObject("btnAppsRun.Image"), System.Drawing.Image)
        Me.btnAppsRun.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAppsRun.Name = "btnAppsRun"
        Me.btnAppsRun.Size = New System.Drawing.Size(23, 22)
        Me.btnAppsRun.Text = "ToolStripButton1"
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
        'frmApps
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(693, 321)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.tsApps)
        Me.Controls.Add(Me.lvApps)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmApps"
        Me.Text = "SysPurge"
        Me.tsApps.ResumeLayout(False)
        Me.tsApps.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvApps As BufferedListView
    Friend WithEvents tsApps As ToolStrip
    Friend WithEvents btnAppsRun As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
