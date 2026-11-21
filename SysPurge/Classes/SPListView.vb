'--------------------------------------------------------------------------------------------------
' SysPurge: SPListView.vb - flicker-free ListView (double buffered, no background erase)
'    © 2026 Remus Rigo
'       v1.0.20261120
'--------------------------------------------------------------------------------------------------

Public Class SPListView
   Inherits ListView

   Private Const WM_ERASEBKGND As Integer = &H14

   Private ReadOnly lv As ListView

   Public Sub New()
      ' DoubleBuffered also sets LVS_EX_DOUBLEBUFFER and keeps it when the handle is recreated
      Me.DoubleBuffered = True
      Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
   End Sub

   ' Don't let Windows erase the background before painting (classic ListView flicker source)
   Protected Overrides Sub WndProc(ByRef m As Message)
      If m.Msg = WM_ERASEBKGND Then
         m.Result = New IntPtr(1)
         Return
      End If
      MyBase.WndProc(m)
   End Sub

End Class
