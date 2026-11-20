'--------------------------------------------------------------------------------------------------
' SysPurge: CleanerList.vb - shared logic for the cleaner ListViews (frmFS, frmReg, frmMSApps, frmApps)
'    © 2026 Remus Rigo
'       v1.0.20261120
'
'    One ListView row = one CleanAction. The same action data is used to calculate the Size
'    column (in background) and to do the clean, so both always work on the same paths/keys.
'--------------------------------------------------------------------------------------------------

Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Threading
Imports Microsoft.Win32

'--------------------------------------------------------------------------------------------------
' RegTarget: a registry key whose values are deleted (one TaskCleanRegValues call)
Friend Class RegTarget
   Public ReadOnly Root As RegistryKey
   Public ReadOnly KeyPath As String
   Public ReadOnly IncludeDefault As Boolean

   Public Sub New(root As RegistryKey, keyPath As String, Optional includeDefault As Boolean = False)
      Me.Root = root
      Me.KeyPath = keyPath
      Me.IncludeDefault = includeDefault
   End Sub
End Class

'--------------------------------------------------------------------------------------------------
' CleanAction: what one ListView row cleans
Friend Class CleanAction
   Public Name As String
   Public Group As String
   Public Targets As New List(Of CleanTarget)               ' folders/files
   Public Reg As RegTarget                                  ' registry values
   Public CustomClean As Action(Of ListView, ListViewItem)  ' anything else (e.g. TaskClearEventLogs)
   Public CustomSize As Func(Of CancellationToken, String)  ' Size column text for CustomClean
   Public BeforeClean As Action                             ' optional: stop services, take ownership, ...
   Public AfterClean As Action                              ' optional: restart services (always runs)
End Class

'--------------------------------------------------------------------------------------------------
' CleanerList: drives one ListView (columns, items, background size scan, run, progress drawing)
Friend Class CleanerList

   Private Const COL_SIZE As Integer = 1
   Private Const COL_PROGRESS As Integer = 3

   Private ReadOnly lv As ListView
   Private ReadOnly log As Logger
   Private ReadOnly actions As New Dictionary(Of ListViewItem, CleanAction)
   Private grp As ListViewGroup
   Private sizeCts As CancellationTokenSource

   '-----------------------------------------------------------------------------------------------
   ' Configures the ListView: call once from the form's Load
   Public Sub New(listView As ListView, logger As Logger)
      lv = listView
      log = logger

      lv.View = View.Details
      lv.FullRowSelect = False
      lv.CheckBoxes = True
      lv.OwnerDraw = True

      ' Fill the space between the ToolStrip and the StatusStrip.
      ' BringToFront = docked last, so the strips take their space first.
      lv.Dock = DockStyle.Fill
      lv.BringToFront()

      lv.Columns.Clear()
      lv.Columns.Add("Action", 60)
      lv.Columns.Add("Size", 60)
      lv.Columns.Add("Deleted", 60)
      lv.Columns.Add("Progress", 60)

      AddHandler lv.Resize, Sub() ResizeListViewColumns(lv)
      AddHandler lv.DrawColumnHeader, Sub(s, e) e.DrawDefault = True
      AddHandler lv.DrawItem, Sub(s, e) Return         ' rows are drawn per sub-item
      AddHandler lv.DrawSubItem, AddressOf DrawSubItem
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Build: clears the list, lets the form add groups/items, then starts the size scan
   Public Sub Build(addItems As Action)
      CancelSizeScan()
      actions.Clear()
      grp = Nothing

      lv.BeginUpdate()
      Try
         lv.Items.Clear()
         lv.Groups.Clear()
         addItems()
         ResizeListViewColumns(lv, True)
      Finally
         lv.EndUpdate()
      End Try

      StartSizeScan()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Add ListView Group
   Public Sub AddGroup(name As String)
      grp = New ListViewGroup(name)
      lv.Groups.Add(grp)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Add a row that cleans folders/files
   Public Function AddItem(name As String, isChecked As Boolean, ParamArray targets() As CleanTarget) As CleanAction
      Dim act = NewRow(name, isChecked)

      ' skip empty paths and duplicates (same folder + mask), so sizes aren't counted twice
      Dim seen As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
      For Each t As CleanTarget In targets
         If String.IsNullOrEmpty(t.Path) Then Continue For
         If seen.Add(t.Path.TrimEnd("\"c) & "|" & t.Mask) Then act.Targets.Add(t)
      Next

      Return act
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Add a row that deletes the values of a registry key
   Public Function AddRegItem(name As String, isChecked As Boolean, root As RegistryKey, keyPath As String,
                              Optional includeDefault As Boolean = False) As CleanAction
      Dim act = NewRow(name, isChecked)
      act.Reg = New RegTarget(root, keyPath, includeDefault)
      Return act
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Add a row with its own clean (and optional size) logic
   Public Function AddCustomItem(name As String, isChecked As Boolean, clean As Action(Of ListView, ListViewItem),
                                 Optional size As Func(Of CancellationToken, String) = Nothing) As CleanAction
      Dim act = NewRow(name, isChecked)
      act.CustomClean = clean
      act.CustomSize = size
      Return act
   End Function

   Private Function NewRow(name As String, isChecked As Boolean) As CleanAction
      Dim item As New ListViewItem(name)
      item.SubItems.Add("...")          ' Size (calculated in background)
      item.SubItems.Add("")             ' Deleted
      item.SubItems.Add("")             ' Progress
      item.Checked = isChecked
      item.Tag = 0
      item.Group = grp
      lv.Items.Add(item)

      Dim act As New CleanAction With {.Name = name, .Group = If(grp Is Nothing, "", grp.Header)}
      actions(item) = act
      Return act
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Run: cleans the checked rows on a background thread, then refreshes the Size column
   Public Async Function RunAsync() As Task
      ' Gather the items on the UI thread (in ListView order)
      Dim work As New List(Of KeyValuePair(Of ListViewItem, CleanAction))()
      For Each item As ListViewItem In lv.Items
         Dim act As CleanAction = Nothing
         If item.Checked AndAlso actions.TryGetValue(item, act) Then
            work.Add(New KeyValuePair(Of ListViewItem, CleanAction)(item, act))
         End If
      Next

      ' Stop the size scan so it doesn't compete with the delete for the disk
      CancelSizeScan()

      Try
         Await Task.Run(Sub() ProcessActions(work))
      Catch ex As Exception
         log.Msg.Error($"Cleanup failed: {ex.Message}")
      End Try

      ' Refresh the Size column with what is left
      StartSizeScan()
   End Function

   Private Sub ProcessActions(work As List(Of KeyValuePair(Of ListViewItem, CleanAction)))
      For Each kv In work
         Dim item As ListViewItem = kv.Key
         Dim act As CleanAction = kv.Value

         log.Msg.Info($"Clean: {act.Group}: {act.Name}")
         Try
            act.BeforeClean?.Invoke()

            If act.CustomClean IsNot Nothing Then
               act.CustomClean(lv, item)
            ElseIf act.Reg IsNot Nothing Then
               TaskCleanRegValues(lv, item, act.Reg.Root, act.Reg.KeyPath, act.Reg.IncludeDefault)
            Else
               TaskCleanTargets(lv, item, act.Targets)
            End If
         Catch ex As Exception
            log.Msg.Warning($"{act.Name}: {ex.Message}")
            SetTaskProgressBytes(lv, item, 0, 100)
         Finally
            ' runs even if the clean failed (e.g. restart Windows Update services)
            Try
               act.AfterClean?.Invoke()
            Catch ex As Exception
               log.Msg.Warning($"{act.Name}: {ex.Message}")
            End Try
         End Try
      Next
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Size scan: calculates the Size column of every row in background (sequential: parallel scans
   ' of the same disk are slower). Cancelled by Build, RunAsync and the form's FormClosing.
   Public Sub CancelSizeScan()
      sizeCts?.Cancel()
   End Sub

   Private Sub StartSizeScan()
      CancelSizeScan()
      sizeCts = New CancellationTokenSource()
      Dim token As CancellationToken = sizeCts.Token
      Dim work = actions.ToList()       ' snapshot taken on the UI thread

      Task.Run(Sub()
                  For Each kv In work
                     If token.IsCancellationRequested Then Return

                     Dim text As String
                     Try
                        text = GetSizeText(kv.Value, token)
                     Catch ex As Exception
                        text = "?"
                     End Try
                     If token.IsCancellationRequested Then Return

                     Try
                        If Not lv.IsHandleCreated Then Return
                        lv.BeginInvoke(Sub()
                                          If token.IsCancellationRequested OrElse kv.Key.ListView Is Nothing Then Return
                                          kv.Key.SubItems(COL_SIZE).Text = text
                                          ResizeListViewColumns(lv, True)
                                       End Sub)
                     Catch ex As ObjectDisposedException
                        Return
                     Catch ex As InvalidOperationException
                        Return
                     End Try
                  Next
               End Sub, token)
   End Sub

   Private Shared Function GetSizeText(act As CleanAction, token As CancellationToken) As String
      If act.CustomClean IsNot Nothing Then
         Return If(act.CustomSize Is Nothing, "", act.CustomSize(token))
      End If

      If act.Reg IsNot Nothing Then
         Return $"{CountRegValues(act.Reg)} entries"
      End If

      If act.Targets.Count = 0 Then Return ""

      Dim total As Long = 0
      For Each t As CleanTarget In act.Targets
         total += GetTargetSize(t, token)
      Next
      Return FormatBytes(total)
   End Function

   Private Shared Function CountRegValues(r As RegTarget) As Integer
      If r.Root Is Nothing Then Return 0
      Using key As RegistryKey = r.Root.OpenSubKey(r.KeyPath, False)
         If key Is Nothing Then Return 0
         Return key.GetValueNames().Count(Function(n) r.IncludeDefault OrElse n <> "")
      End Using
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Size helper for the "EventViewer: Logs" row: total size of all event log files
   Public Shared Function GetEventLogsSize(token As CancellationToken) As String
      Dim total As Long = 0
      Using session As New EventLogSession()
         For Each name As String In session.GetLogNames()
            If token.IsCancellationRequested Then Exit For
            Try
               total += If(session.GetLogInformation(name, PathType.LogName).FileSize, 0L)
            Catch
               ' log not readable: skip
            End Try
         Next
      End Using
      Return FormatBytes(total)
   End Function

   '-----------------------------------------------------------------------------------------------
   ' DrawSubItem: default drawing, except the Progress column (progress bar, value in item.Tag)
   Private Sub DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs)
      If e.ColumnIndex <> COL_PROGRESS Then
         e.DrawDefault = True
         Return
      End If

      e.DrawBackground()

      Const COLOR_FILL_ACTIVE As Integer = &HD07800
      Const COLOR_FILL_DONE As Integer = &H50B000
      Const COLOR_BORDER As Integer = &HAAAAAA
      Const PADDING_H As Integer = 3
      Const PADDING_V As Integer = 4

      Dim rect As Rectangle = e.Bounds
      rect.Inflate(-PADDING_H, -PADDING_V)

      Dim progress As Integer = If(TypeOf e.Item.Tag Is Integer, CInt(e.Item.Tag), 0)
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
         TextRenderer.DrawText(g, progress.ToString() & "%", lv.Font, rect, Color.Black,
                               TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
      End If
   End Sub

End Class
