<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSysPurge
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSysPurge))
      lvSysPurge = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      ColumnHeader3 = New ColumnHeader()
      ToolStrip1 = New ToolStrip()
      btnTSPurge = New ToolStripButton()
      StatusStrip1 = New StatusStrip()
      ToolStrip1.SuspendLayout()
      SuspendLayout()
      ' 
      ' lvSysPurge
      ' 
      lvSysPurge.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      lvSysPurge.CheckBoxes = True
      lvSysPurge.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2, ColumnHeader3})
      lvSysPurge.FullRowSelect = True
      lvSysPurge.Location = New Point(0, 28)
      lvSysPurge.Name = "lvSysPurge"
      lvSysPurge.OwnerDraw = True
      lvSysPurge.Size = New Size(809, 317)
      lvSysPurge.TabIndex = 0
      lvSysPurge.UseCompatibleStateImageBehavior = False
      lvSysPurge.View = View.Details
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
      ' ToolStrip1
      ' 
      ToolStrip1.Items.AddRange(New ToolStripItem() {btnTSPurge})
      ToolStrip1.Location = New Point(0, 0)
      ToolStrip1.Name = "ToolStrip1"
      ToolStrip1.Size = New Size(809, 25)
      ToolStrip1.TabIndex = 1
      ToolStrip1.Text = "ToolStrip1"
      ' 
      ' btnTSPurge
      ' 
      btnTSPurge.DisplayStyle = ToolStripItemDisplayStyle.Image
      btnTSPurge.Image = CType(resources.GetObject("btnTSPurge.Image"), Image)
      btnTSPurge.ImageTransparentColor = Color.Magenta
      btnTSPurge.Name = "btnTSPurge"
      btnTSPurge.Size = New Size(23, 22)
      btnTSPurge.Text = "ToolStripButton1"
      ' 
      ' StatusStrip1
      ' 
      StatusStrip1.Location = New Point(0, 348)
      StatusStrip1.Name = "StatusStrip1"
      StatusStrip1.Size = New Size(809, 22)
      StatusStrip1.TabIndex = 2
      StatusStrip1.Text = "StatusStrip1"
      ' 
      ' frmSysPurge
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(809, 370)
      Controls.Add(StatusStrip1)
      Controls.Add(ToolStrip1)
      Controls.Add(lvSysPurge)
      Icon = CType(resources.GetObject("$this.Icon"), Icon)
      Name = "frmSysPurge"
      Text = "SysPurge"
      ToolStrip1.ResumeLayout(False)
      ToolStrip1.PerformLayout()
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents lvSysPurge As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents ColumnHeader3 As ColumnHeader
   Friend WithEvents ToolStrip1 As ToolStrip
   Friend WithEvents btnTSPurge As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
