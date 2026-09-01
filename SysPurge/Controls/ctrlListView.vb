'--------------------------------------------------------------------------------------------------
' ListView Utility
'    © 2026 Remus Rigo
'       v1.0.20260724
'--------------------------------------------------------------------------------------------------

Module ctrlListView

   '-----------------------------------------------------------------------------------------------
   ' ResizeColumns: Adjusts the width of the ListView columns
   Public Sub ResizeListViewColumns(lv As ListView)
      Dim w As Integer

      ' Setting column width to -1 in WinForms ListView auto-sizes the column
      lv.Columns(0).Width = -1
      lv.Columns(1).Width = -1

      ' Retrieve column widths via P/Invoke
      w = SendMessage(lv.Handle, LVM_GETCOLUMNWIDTH, New IntPtr(0), IntPtr.Zero).ToInt32()
      w += SendMessage(lv.Handle, LVM_GETCOLUMNWIDTH, New IntPtr(1), IntPtr.Zero).ToInt32()

      ' Calculate and set the width of the third column (index 2)
      lv.Columns(2).Width = lv.ClientSize.Width - w - GetSystemMetrics(SM_CXVSCROLL)
   End Sub

End Module
