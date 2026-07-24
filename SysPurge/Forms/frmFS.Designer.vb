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
      lvFS = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      ColumnHeader3 = New ColumnHeader()
      tsFS = New ToolStrip()
      btnFSRun = New ToolStripButton()
      StatusStrip1 = New StatusStrip()
      tsFS.SuspendLayout()
      SuspendLayout()
      ' 
      ' lvFS
      ' 
      lvFS.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      lvFS.CheckBoxes = True
      lvFS.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2, ColumnHeader3})
      lvFS.FullRowSelect = True
      lvFS.Location = New Point(0, 28)
      lvFS.Name = "lvFS"
      lvFS.OwnerDraw = True
      lvFS.Size = New Size(807, 317)
      lvFS.TabIndex = 0
      lvFS.UseCompatibleStateImageBehavior = False
      lvFS.View = View.Details
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
      ' tsFS
      ' 
      tsFS.Items.AddRange(New ToolStripItem() {btnFSRun})
      tsFS.Location = New Point(0, 0)
      tsFS.Name = "tsFS"
      tsFS.Size = New Size(809, 25)
      tsFS.TabIndex = 1
      tsFS.Text = "ToolStrip1"
      ' 
      ' btnFSRun
      ' 
      btnFSRun.DisplayStyle = ToolStripItemDisplayStyle.Image
      btnFSRun.Image = CType(resources.GetObject("btnFSRun.Image"), Image)
      btnFSRun.ImageTransparentColor = Color.Magenta
      btnFSRun.Name = "btnFSRun"
      btnFSRun.Size = New Size(23, 22)
      btnFSRun.Text = "ToolStripButton1"
      ' 
      ' StatusStrip1
      ' 
      StatusStrip1.Location = New Point(0, 348)
      StatusStrip1.Name = "StatusStrip1"
      StatusStrip1.Size = New Size(809, 22)
      StatusStrip1.TabIndex = 2
      StatusStrip1.Text = "StatusStrip1"
      ' 
      ' frmFS
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(809, 370)
      Controls.Add(StatusStrip1)
      Controls.Add(tsFS)
      Controls.Add(lvFS)
      Icon = CType(resources.GetObject("$this.Icon"), Icon)
      Name = "frmFS"
      Text = "SysPurge"
      tsFS.ResumeLayout(False)
      tsFS.PerformLayout()
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents lvFS As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents ColumnHeader3 As ColumnHeader
   Friend WithEvents tsFS As ToolStrip
   Friend WithEvents btnFSRun As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
