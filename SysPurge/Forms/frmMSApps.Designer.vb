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
      lvMSApps = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      ColumnHeader3 = New ColumnHeader()
      tsMSApps = New ToolStrip()
      btnMSAppsRun = New ToolStripButton()
      StatusStrip1 = New StatusStrip()
      tsMSApps.SuspendLayout()
      SuspendLayout()
      ' 
      ' lvMSApps
      ' 
      lvMSApps.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      lvMSApps.CheckBoxes = True
      lvMSApps.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2, ColumnHeader3})
      lvMSApps.FullRowSelect = True
      lvMSApps.Location = New Point(0, 28)
      lvMSApps.Name = "lvMSApps"
      lvMSApps.OwnerDraw = True
      lvMSApps.Size = New Size(807, 317)
      lvMSApps.TabIndex = 0
      lvMSApps.UseCompatibleStateImageBehavior = False
      lvMSApps.View = View.Details
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
      ' tsMSApps
      ' 
      tsMSApps.Items.AddRange(New ToolStripItem() {btnMSAppsRun})
      tsMSApps.Location = New Point(0, 0)
      tsMSApps.Name = "tsMSApps"
      tsMSApps.Size = New Size(809, 25)
      tsMSApps.TabIndex = 1
      tsMSApps.Text = "ToolStrip1"
      ' 
      ' btnMSAppsRun
      ' 
      btnMSAppsRun.DisplayStyle = ToolStripItemDisplayStyle.Image
      btnMSAppsRun.Image = CType(resources.GetObject("btnMSAppsRun.Image"), Image)
      btnMSAppsRun.ImageTransparentColor = Color.Magenta
      btnMSAppsRun.Name = "btnMSAppsRun"
      btnMSAppsRun.Size = New Size(23, 22)
      btnMSAppsRun.Text = "ToolStripButton1"
      ' 
      ' StatusStrip1
      ' 
      StatusStrip1.Location = New Point(0, 348)
      StatusStrip1.Name = "StatusStrip1"
      StatusStrip1.Size = New Size(809, 22)
      StatusStrip1.TabIndex = 2
      StatusStrip1.Text = "StatusStrip1"
      ' 
      ' frmMSApps
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(809, 370)
      Controls.Add(StatusStrip1)
      Controls.Add(tsMSApps)
      Controls.Add(lvMSApps)
      Icon = CType(resources.GetObject("$this.Icon"), Icon)
      Name = "frmMSApps"
      Text = "SysPurge"
      tsMSApps.ResumeLayout(False)
      tsMSApps.PerformLayout()
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents lvMSApps As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents ColumnHeader3 As ColumnHeader
   Friend WithEvents tsMSApps As ToolStrip
   Friend WithEvents btnMSAppsRun As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
