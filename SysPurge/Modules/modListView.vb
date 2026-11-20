'--------------------------------------------------------------------------------------------------
' ListView Utility
'    © 2026 Remus Rigo
'       v1.0.20261120
'--------------------------------------------------------------------------------------------------

Imports SysPurge.API

Module modListView


   '-----------------------------------------------------------------------------------------------
   ' ResizeColumns: Adjusts the width of the ListView columns
   Public Sub ResizeListViewColumns(lv As ListView, Optional autoSizeAllCollumns As Boolean = False)
      Dim w As Integer

      ' Setting column width to -1 in WinForms ListView auto-sizes the column
      lv.Columns(0).Width = -1
      If autoSizeAllCollumns Then
         lv.Columns(1).Width = -1
         lv.Columns(2).Width = -1
      End If

      ' Retrieve column widths via P/Invoke
      w = SendMessage(lv.Handle, LVM_GETCOLUMNWIDTH, New IntPtr(0), IntPtr.Zero).ToInt32()
      w += SendMessage(lv.Handle, LVM_GETCOLUMNWIDTH, New IntPtr(1), IntPtr.Zero).ToInt32()
      w += SendMessage(lv.Handle, LVM_GETCOLUMNWIDTH, New IntPtr(2), IntPtr.Zero).ToInt32()

      ' Calculate and set the width of the last column for the progress bar
      lv.Columns(3).Width = lv.ClientSize.Width - w - GetSystemMetrics(SM_CXVSCROLL)
   End Sub

End Module
