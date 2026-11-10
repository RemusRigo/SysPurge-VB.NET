'--------------------------------------------------------------------------------------------------
' Logger class
'    © 2026 Remus Rigo
'       v1.0.20260625
'--------------------------------------------------------------------------------------------------

Imports System.IO
Imports System.Text

Public Enum LogLevel
   Info
   Warning
   [Error]
End Enum

Public Class Logger

   Public Property File As String = "application"
   Public ReadOnly Msg As LogMessage

   ' Shared by all Logger instances: several instances write to the same file
   Private Shared ReadOnly _lock As New Object()

   Public Sub New(fileName As String)
      Me.File = fileName
      Msg = New LogMessage(Me)
   End Sub

   Friend Sub Write(level As LogLevel, message As String)
      Dim now As DateTime = DateTime.Now
      ' Next to the exe (a relative path would follow the working dir, e.g. System32 when elevated)
      Dim folder As String = Path.Combine(AppContext.BaseDirectory, "Logs", now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"))
      Dim cleanMessage As String = message.Replace(vbCr, " ").Replace(vbLf, " ") ' Remove newlines from message

      Dim filePath As String = Path.Combine(folder, $"{Me.File}.log")
      Dim logLine As String = $"{now:yyyy-MM-dd HH:mm:ss}: {level}: {cleanMessage}"

      ' Logging must never break the caller (it is often called from inside a Catch block)
      Try
         SyncLock _lock
            Directory.CreateDirectory(folder)
            IO.File.AppendAllText(filePath, logLine & Environment.NewLine, Encoding.UTF8)
         End SyncLock
      Catch
      End Try
   End Sub

End Class

Public Class LogMessage

   Private ReadOnly _parent As Logger

   Public Sub New(parent As Logger)
      _parent = parent
   End Sub

   Public Sub Info(message As String)
      _parent.Write(LogLevel.Info, message)
   End Sub

   Public Sub Warning(message As String)
      _parent.Write(LogLevel.Warning, message)
   End Sub

   Public Sub [Error](message As String)
      _parent.Write(LogLevel.Error, message)
   End Sub

End Class
