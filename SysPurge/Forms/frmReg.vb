'--------------------------------------------------------------------------------------------------
' SysPurge: frmReg.vb - Registry cleaner
'    © 2026 Remus Rigo
'       v1.1.20260724
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Win32

Public Class frmReg

   Dim grp As ListViewGroup = Nothing
   Dim log As New Logger(appName)

   '-----------------------------------------------------------------------------------------------
   ' Add ListView Group
   Private Sub LV_AddGroup(name As String)
      grp = New ListViewGroup(name)
      lvReg.Groups.Add(grp)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Add ListView item
   Private Sub LV_AddItem(name As String, isChecked As Boolean)
      Dim item As New ListViewItem(name)
      item.SubItems.Add("")
      item.SubItems.Add("")
      item.Checked = isChecked
      item.Tag = 0
      item.Group = grp
      lvReg.Items.Add(item)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Build Options
   Public Sub BuildOptions()
      lvReg.BeginUpdate()
      lvReg.Items.Clear()
      lvReg.Groups.Clear()

      LV_AddGroup("MRU")
      LV_AddItem("Run MRU", True)

      LV_AddGroup("Misc")
      If IsAppElevated() Then LV_AddItem("Shared DLL's)", True)

      ResizeListViewColumns(lvReg)
      lvReg.EndUpdate()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Process Actions
   Private Sub ProcessActions(itemsToProcess As List(Of ListViewItem))
      For Each item As ListViewItem In itemsToProcess
         Dim grp = item.Group
         If grp Is Nothing Then Continue For

         Select Case grp.Header

            '--------------------------------------------------------------------------------------
            Case "MRU"
               Select Case item.Text

                  Case "Run MRU"
                     log.Msg.Info("Clean: Microsoft Windows » Registry » MRU: Run MRU")
                     TaskCleanRegValues(lvReg, item, Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU", False)

               End Select

            '--------------------------------------------------------------------------------------
            Case "Misc"
               Select Case item.Text

                  Case "Shared DLL's)"
                     'log.Msg.Info("Clean: Microsoft Windows » FileSystem: Jump List")
                     '
               End Select

         End Select
      Next
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmReg: onLoad
   Private Sub frmReg_Load(sender As Object, e As EventArgs) Handles Me.Load
      Me.Text = appName & " " & appVersion & " " & appAuthor
      SendMessage(lvReg.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, CType(LVS_EX_DOUBLEBUFFER, IntPtr), CType(LVS_EX_DOUBLEBUFFER, IntPtr))

      BuildOptions()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' btnTSPurge: onClick
   Private Async Sub btnTSPurge_Click(sender As Object, e As EventArgs) Handles btnRegRun.Click
      ' 1. Gather the items to process on the UI thread
      Dim itemsToProcess As New List(Of ListViewItem)()
      For Each item As ListViewItem In lvReg.Items
         If item.Checked AndAlso item.Group IsNot Nothing Then
            itemsToProcess.Add(item)
         End If
      Next

      Try
         'Pass the gathered items to the background worker
         Await Task.Run(Sub() ProcessActions(itemsToProcess))
      Finally
         'toolBtnPurge.Enabled = True
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawColumnHeader
   Private Sub lvSysPurge_DrawColumnHeader(sender As Object, e As DrawListViewColumnHeaderEventArgs) Handles lvReg.DrawColumnHeader
      ' draw column headers with default style
      e.DrawDefault = True
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawItem
   Private Sub lvSysPurge_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles lvReg.DrawItem
      ' draw items with default style (except subitem 2 which is handled in DrawSubItem)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' lvSysPurge: DrawSubItem
   Private Sub lvSysPurge_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles lvReg.DrawSubItem
      ' column 3 (index 2)
      If e.ColumnIndex <> 2 Then
         e.DrawDefault = True
         Return
      End If

      e.DrawBackground()

      Const COLOR_FILL_ACTIVE As Integer = &HD07800
      Const COLOR_FILL_DONE As Integer = &H50B000
      Const COLOR_BORDER As Integer = &HAAAAAA
      Const PADDING_H As Integer = 3
      Const PADDING_V As Integer = 4

      Dim r As RECT
      r.Top = e.ColumnIndex
      r.Left = LVIR_BOUNDS
      SendMessage(lvReg.Handle, LVM_GETSUBITEMRECT, CType(e.ItemIndex, IntPtr), r)

      Dim rect = Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom)
      rect.Inflate(-PADDING_H, -PADDING_V)

      Dim progress As Integer = 0
      If e.Item.Tag IsNot Nothing Then
         progress = CInt(e.Item.Tag)
      End If

      Dim g = e.Graphics

      Using bg As New SolidBrush(SystemColors.Window)
         g.FillRectangle(bg, rect)
      End Using

      If progress > 0 Then
         Dim fillWidth = CInt(rect.Width * progress / 100.0)
         Dim fillRect = New Rectangle(rect.Left, rect.Top, fillWidth, rect.Height)

         Dim fillColor = If(progress >= 100, ColorTranslator.FromWin32(COLOR_FILL_DONE),
                                            ColorTranslator.FromWin32(COLOR_FILL_ACTIVE))
         Using br As New SolidBrush(fillColor)
            g.FillRectangle(br, fillRect)
         End Using
      End If

      Using pen As New Pen(ColorTranslator.FromWin32(COLOR_BORDER))
         g.DrawRectangle(pen, rect)
      End Using

      If progress > 0 Then
         Dim text = progress.ToString() & "%"
         TextRenderer.DrawText(g, text, lvReg.Font, rect, Color.Black, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
      End If
   End Sub

End Class
