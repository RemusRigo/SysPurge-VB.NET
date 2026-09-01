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
      lvReg = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      ColumnHeader3 = New ColumnHeader()
      btnTSReg = New ToolStrip()
      btnRegRun = New ToolStripButton()
      StatusStrip1 = New StatusStrip()
      btnTSReg.SuspendLayout()
      SuspendLayout()
      ' 
      ' lvReg
      ' 
      lvReg.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      lvReg.CheckBoxes = True
      lvReg.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2, ColumnHeader3})
      lvReg.FullRowSelect = True
      lvReg.Location = New Point(0, 28)
      lvReg.Name = "lvReg"
      lvReg.OwnerDraw = True
      lvReg.Size = New Size(807, 317)
      lvReg.TabIndex = 0
      lvReg.UseCompatibleStateImageBehavior = False
      lvReg.View = View.Details
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
      ' btnTSReg
      ' 
      btnTSReg.Items.AddRange(New ToolStripItem() {btnRegRun})
      btnTSReg.Location = New Point(0, 0)
      btnTSReg.Name = "btnTSReg"
      btnTSReg.Size = New Size(809, 25)
      btnTSReg.TabIndex = 1
      btnTSReg.Text = "ToolStrip1"
      ' 
      ' btnRegRun
      ' 
      btnRegRun.DisplayStyle = ToolStripItemDisplayStyle.Image
      btnRegRun.Image = CType(resources.GetObject("btnRegRun.Image"), Image)
      btnRegRun.ImageTransparentColor = Color.Magenta
      btnRegRun.Name = "btnRegRun"
      btnRegRun.Size = New Size(23, 22)
      btnRegRun.Text = "ToolStripButton1"
      ' 
      ' StatusStrip1
      ' 
      StatusStrip1.Location = New Point(0, 348)
      StatusStrip1.Name = "StatusStrip1"
      StatusStrip1.Size = New Size(809, 22)
      StatusStrip1.TabIndex = 2
      StatusStrip1.Text = "StatusStrip1"
      ' 
      ' frmReg
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(809, 370)
      Controls.Add(StatusStrip1)
      Controls.Add(btnTSReg)
      Controls.Add(lvReg)
      Icon = CType(resources.GetObject("$this.Icon"), Icon)
      Name = "frmReg"
      Text = "SysPurge"
      btnTSReg.ResumeLayout(False)
      btnTSReg.PerformLayout()
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents lvReg As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents ColumnHeader3 As ColumnHeader
   Friend WithEvents btnTSReg As ToolStrip
   Friend WithEvents btnRegRun As ToolStripButton
   Friend WithEvents StatusStrip1 As StatusStrip

End Class
