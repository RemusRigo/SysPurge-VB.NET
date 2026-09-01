Module Processes

   Public Function IsProcessRunning(procName As String) As Boolean
      Return Process.GetProcessesByName(procName).Length > 0
   End Function

   Public Sub KillProcessById(pid As Integer)
      Try
         Dim p As Process = Process.GetProcessById(pid)
         p.Kill()
         p.WaitForExit()
      Catch ex As Exception
         ' Optional: log or ignore
      End Try
   End Sub

   Public Sub KillProcessByName(procName As String)
      For Each p As Process In Process.GetProcessesByName(procName)
         Try
            p.Kill()
            p.WaitForExit()
         Catch
            ' ignore or log
         End Try
      Next
   End Sub


End Module
