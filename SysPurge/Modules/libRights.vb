'--------------------------------------------------------------------------------------------------
' libRights - User rights functions
'    © 2026 Remus Rigo
'       v1.0 2026-06-10
'--------------------------------------------------------------------------------------------------

Imports System.Security.Principal

Public Module libRights

   '-----------------------------------------------------------------------------------------------
   ' Check if app is running with administrative privileges
   Public Function IsAppElevated() As Boolean
      Dim identity As WindowsIdentity = WindowsIdentity.GetCurrent()
      Dim principal As New WindowsPrincipal(identity)

      Return principal.IsInRole(WindowsBuiltInRole.Administrator)
   End Function
End Module

