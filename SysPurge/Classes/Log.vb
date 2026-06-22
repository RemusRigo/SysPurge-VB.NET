'--------------------------------------------------------------------------------------------------
' Log - A simple logger
'    © 2026 Remus Rigo
'       v1.0 2026-06-03
'--------------------------------------------------------------------------------------------------

Imports System.IO
Imports System.Text
Imports System.Windows.Forms ' Required for Application.ProductName

Public Class Log
   Private Shared _instance As Log
   Private Shared ReadOnly _padlock As New Object()
   Private ReadOnly _appName As String

   ' Private constructor prevents external instantiation
   Private Sub New(appName As String)
      _appName = appName
   End Sub

   ' Singleton property access
   Public Shared ReadOnly Property Instance As Log
      Get
         SyncLock _padlock
            If _instance Is Nothing Then
               ' Using Application.ProductName to pull the project name automatically
               _instance = New Log(Application.ProductName)
            End If
         End SyncLock
         Return _instance
      End Get
   End Property

   ' Helper methods
   Public Sub Info(message As String)
      WriteLog("INFO", message)
   End Sub

   Public Sub Warning(message As String)
      WriteLog("WARNING", message)
   End Sub

   Public Sub [Error](message As String)
      WriteLog("ERROR", message)
   End Sub

   Private Sub WriteLog(level As String, message As String)
      Try
         Dim now As DateTime = DateTime.Now
         Dim folder As String = Path.Combine("Logs", now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"))
         Dim cleanMessage As String = message.Replace(vbCr, " ").Replace(vbLf, " ") ' Remove newlines from message

         If Not Directory.Exists(folder) Then Directory.CreateDirectory(folder)

         Dim filePath As String = Path.Combine(folder, $"{_appName}.log")
         Dim logLine As String = $"{now:yyyy-MM-dd HH:mm:ss}: {level}: {cleanMessage}"

         SyncLock _padlock
            File.AppendAllText(filePath, logLine & Environment.NewLine, Encoding.UTF8)
         End SyncLock
      Catch
         ' Fail silently or write to Event Viewer
      End Try
   End Sub
End Class