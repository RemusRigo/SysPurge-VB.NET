<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFS
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFS))
        Me.lvFS = New SPListView()
        Me.tsFS = New System.Windows.Forms.ToolStrip()
        Me.tsFS_btnRun = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsFS.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvFS
        '
        Me.lvFS.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvFS.CheckBoxes = True
        Me.lvFS.FullRowSelect = True
        Me.lvFS.HideSelection = False
        Me.lvFS.Location = New System.Drawing.Point(0, 24)
        Me.lvFS.Name = "lvFS"
        Me.lvFS.OwnerDraw = True
        Me.lvFS.Size = New System.Drawing.Size(692, 275)
        Me.lvFS.TabIndex = 0
        Me.lvFS.UseCompatibleStateImageBehavior = False
        Me.lvFS.View = System.Windows.Forms.View.Details
        '
        'tsFS
        '
        Me.tsFS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsFS_btnRun})
        Me.tsFS.Location = New System.Drawing.Point(0, 0)
        Me.tsFS.Name = "tsFS"
        Me.tsFS.Size = New System.Drawing.Size(693, 25)
        Me.tsFS.TabIndex = 1
        Me.tsFS.Text = "ToolStrip1"
        '
        'tsFS_btnRun
        '
        Me.tsFS_btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsFS_btnRun.Image = CType(resources.GetObject("tsFS_btnRun.Image"), System.Drawing.Image)
        Me.tsFS_btnRun.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsFS_btnRun.Name = "tsFS_btnRun"
        Me.tsFS_btnRun.Size = New System.Drawing.Size(23, 22)
        Me.tsFS_btnRun.Text = "ToolStripButton1"
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
        'frmFS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(693, 321)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.tsFS)
        Me.Controls.Add(Me.lvFS)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFS"
        Me.Text = "SysPurge"
        Me.tsFS.ResumeLayout(False)
        Me.tsFS.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvFS As SPListView
    Friend WithEvents tsFS As ToolStrip
    Friend WithEvents tsFS_btnRun As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
