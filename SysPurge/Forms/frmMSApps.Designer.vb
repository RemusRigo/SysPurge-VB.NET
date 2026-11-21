<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMSApps
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMSApps))
        Me.lvMSApps = New SPListView()
        Me.tsMSApps = New System.Windows.Forms.ToolStrip()
        Me.btnMSAppsRun = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsMSApps.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvMSApps
        '
        Me.lvMSApps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvMSApps.CheckBoxes = True
        Me.lvMSApps.FullRowSelect = True
        Me.lvMSApps.HideSelection = False
        Me.lvMSApps.Location = New System.Drawing.Point(0, 24)
        Me.lvMSApps.Name = "lvMSApps"
        Me.lvMSApps.OwnerDraw = True
        Me.lvMSApps.Size = New System.Drawing.Size(692, 275)
        Me.lvMSApps.TabIndex = 0
        Me.lvMSApps.UseCompatibleStateImageBehavior = False
        Me.lvMSApps.View = System.Windows.Forms.View.Details
        '
        'tsMSApps
        '
        Me.tsMSApps.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnMSAppsRun})
        Me.tsMSApps.Location = New System.Drawing.Point(0, 0)
        Me.tsMSApps.Name = "tsMSApps"
        Me.tsMSApps.Size = New System.Drawing.Size(693, 25)
        Me.tsMSApps.TabIndex = 1
        Me.tsMSApps.Text = "ToolStrip1"
        '
        'btnMSAppsRun
        '
        Me.btnMSAppsRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnMSAppsRun.Image = CType(resources.GetObject("btnMSAppsRun.Image"), System.Drawing.Image)
        Me.btnMSAppsRun.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnMSAppsRun.Name = "btnMSAppsRun"
        Me.btnMSAppsRun.Size = New System.Drawing.Size(23, 22)
        Me.btnMSAppsRun.Text = "ToolStripButton1"
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
        'frmMSApps
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(693, 321)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.tsMSApps)
        Me.Controls.Add(Me.lvMSApps)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMSApps"
        Me.Text = "SysPurge"
        Me.tsMSApps.ResumeLayout(False)
        Me.tsMSApps.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvMSApps As SPListView
    Friend WithEvents tsMSApps As ToolStrip
    Friend WithEvents btnMSAppsRun As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
