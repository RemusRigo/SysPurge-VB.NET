'--------------------------------------------------------------------------------------------------
' libServices.vb
'    © 2026 Remus Rigo
'       v1.0.2026-06-13
'--------------------------------------------------------------------------------------------------

Imports System.ServiceProcess

Module libServices
   ''' <summary>
   ''' Retrieves the current status of the specified Windows service.
   ''' </summary>
   Friend Function GetServiceStatus(serviceName As String) As ServiceControllerStatus
      Try
         Using sc As New ServiceController(serviceName)
            Return sc.Status
         End Using
      Catch ex As Exception
         Throw New InvalidOperationException($"Failed to get status for service '{serviceName}'.", ex)
      End Try
   End Function

   ''' <summary>
   ''' Starts the service if it is not already running. (default timeout: 10 seconds)
   ''' </summary>
   Friend Function StartService(serviceName As String, Optional timeoutMilliseconds As Integer = 10000) As Boolean
      Try
         Using sc As New ServiceController(serviceName)
            ' Check if it's already running
            If sc.Status = ServiceControllerStatus.Running Then
               Return True
            End If

            ' Start if it's not already starting
            If sc.Status <> ServiceControllerStatus.StartPending Then
               sc.Start()
            End If

            ' Wait for it to start
            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeoutMilliseconds))
            Return True
         End Using
      Catch ex As Exception
         Return False
      End Try
   End Function

   ''' <summary>
   ''' Stops the service if it is not already stopped. (default timeout: 10 seconds)
   ''' </summary>
   Friend Function StopService(serviceName As String, Optional timeoutMilliseconds As Integer = 10000) As Boolean
      Try
         Using sc As New ServiceController(serviceName)
            ' Check if it's already stopped
            If sc.Status = ServiceControllerStatus.Stopped Then
               Return True
            End If

            ' Stop if it's running
            If sc.Status <> ServiceControllerStatus.StopPending Then
               sc.Stop()
            End If

            ' Wait for it to stop
            sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeoutMilliseconds))
            Return True
         End Using
      Catch ex As Exception
         Return False
      End Try
   End Function


   ''' <summary>
   ''' Restarts the service by stopping it and then starting it. (default timeout: 10 seconds for each operation)
   ''' </summary>
   Friend Function RestartService(serviceName As String, Optional timeoutMilliseconds As Integer = 10000) As Boolean
      Try
         Using sc As New ServiceController(serviceName)

            ' Stop if running
            If sc.Status <> ServiceControllerStatus.Stopped AndAlso sc.Status <> ServiceControllerStatus.StopPending Then
               sc.Stop()
               sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeoutMilliseconds))
            End If

            ' Start
            sc.Start()
            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeoutMilliseconds))
            Return True

         End Using
      Catch ex As Exception
         Return False
      End Try
   End Function

End Module
