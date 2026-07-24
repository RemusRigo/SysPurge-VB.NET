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
      lvApps = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      ColumnHeader3 = New ColumnHeader()
      tsApps = New ToolStrip()
      btnAppsRun = New ToolStripButton()
      StatusStrip1 = New StatusStrip()
      tsApps.SuspendLayout()
      SuspendLayout()
      ' 
      ' lvApps
      ' 
      lvApps.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      lvApps.CheckBoxes = True
      lvApps.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2, ColumnHeader3})
      lvApps.FullRowSelect = True
      lvApps.Location = New Point(0, 28)
      lvApps.Name = "lvApps"
      lvApps.OwnerDraw = True
      lvApps.Size = New Size(807, 317)
      lvApps.TabIndex = 0
      lvApps.UseCompatibleStateImageBehavior = False
      lvApps.View = View.Details
      ' 
      ' ColumnHeader1
      ' 
      ColumnHeader1.Text = "Action"
      ' 
      ' ColumnHeader2
      ' 
      ColumnHeader2.Text = "Result"
      ' 
      ' ColumnHeader3
      ' 
      ColumnHeader3.Text = "Progress"
      ' 
      ' tsApps
      ' 
      tsApps.Items.AddRange(New ToolStripItem() {btnAppsRun})
      tsApps.Location = New Point(0, 0)
      tsApps.Name = "tsApps"
      tsApps.Size = New Size(809, 25)
      tsApps.TabIndex = 1
      tsApps.Text = "ToolStrip1"
      ' 
      ' btnAppsRun
      ' 
      btnAppsRun.DisplayStyle = ToolStripItemDisplayStyle.Image
      btnAppsRun.Image = CType(resources.GetObject("btnAppsRun.Image"), Image)
      btnAppsRun.ImageTransparentColor = Color.Magenta
      btnAppsRun.Name = "btnAppsRun"
      btnAppsRun.Size = New Size(23, 22)
      btnAppsRun.Text = "ToolStripButton1"
      ' 
      ' StatusStrip1
      ' 
      StatusStrip1.Location = New Point(0, 348)
      StatusStrip1.Name = "StatusStrip1"
      StatusStrip1.Size = New Size(809, 22)
      StatusStrip1.TabIndex = 2
      StatusStrip1.Text = "StatusStrip1"
      ' 
      ' frmApps
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(809, 370)
      Controls.Add(StatusStrip1)
      Controls.Add(tsApps)
      Controls.Add(lvApps)
      Icon = CType(resources.GetObject("$this.Icon"), Icon)
      Name = "frmApps"
      Text = "SysPurge"
      tsApps.ResumeLayout(False)
      tsApps.PerformLayout()
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents lvApps As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents ColumnHeader3 As ColumnHeader
   Friend WithEvents tsApps As ToolStrip
   Friend WithEvents btnAppsRun As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
