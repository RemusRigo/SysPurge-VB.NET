'--------------------------------------------------------------------------------------------------
' kernel32_dll
'
'   © 2026 Remus Rigo
'      v1.0 2026-06-05
'--------------------------------------------------------------------------------------------------

Imports Microsoft.Win32.SafeHandles
Imports System.Runtime.InteropServices

Public Module kernel32_dll

   Public Const WRITE_DAC As UInteger = &H40000UI
   Public Const WRITE_OWNER As UInteger = &H80000UI

   Public Const FILE_SHARE_READ As UInteger = &H1UI
   Public Const FILE_SHARE_WRITE As UInteger = &H2UI
   Public Const FILE_SHARE_DELETE As UInteger = &H4UI

   Public Const FILE_FLAG_OPEN_REPARSE_POINT As UInteger = &H200000UI
   Public Const FILE_FLAG_BACKUP_SEMANTICS As UInteger = &H2000000UI

   Public Const GENERIC_READ As UInteger = &H80000000UI
   Public Const GENERIC_WRITE As UInteger = &H40000000UI

   Public Const CREATE_NEW As UInteger = 1
   Public Const CREATE_ALWAYS As UInteger = 2
   Public Const OPEN_EXISTING As UInteger = 3
   Public Const OPEN_ALWAYS As UInteger = 4
   Public Const TRUNCATE_EXISTING As UInteger = 5

   Public Const PURGE_TXABORT As Integer = &H1
   Public Const PURGE_RXABORT As Integer = &H2
   Public Const PURGE_TXCLEAR As Integer = &H4
   Public Const PURGE_RXCLEAR As Integer = &H8

   '-----------------------------------------------------------------------------------------------
   ' Structures

   <StructLayout(LayoutKind.Sequential)>
   Public Structure DCB
      Public DCBlength As Integer
      Public BaudRate As Integer
      Public Flags As Integer
      Public wReserved As Short
      Public XonLim As Short
      Public XoffLim As Short
      Public ByteSize As Byte
      Public Parity As Byte
      Public StopBits As Byte
      Public XonChar As Byte
      Public XoffChar As Byte
      Public ErrorChar As Byte
      Public EofChar As Byte
      Public EvtChar As Byte
      Public wReserved1 As Short
   End Structure

   <StructLayout(LayoutKind.Sequential)>
   Public Structure COMMTIMEOUTS
      Public ReadIntervalTimeout As Integer
      Public ReadTotalTimeoutMultiplier As Integer
      Public ReadTotalTimeoutConstant As Integer
      Public WriteTotalTimeoutMultiplier As Integer
      Public WriteTotalTimeoutConstant As Integer
   End Structure

   '-----------------------------------------------------------------------------------------------
   ' Functions

   <DllImport("kernel32.dll", SetLastError:=True)>
   Public Function CancelIo(ByVal hFile As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
   End Function

   '<DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
   'Public Function CreateFile(lpFileName As String, dwDesiredAccess As UInteger, dwShareMode As UInteger, lpSecurityAttributes As IntPtr,
   '     dwCreationDisposition As UInteger, dwFlagsAndAttributes As UInteger, hTemplateFile As IntPtr) As IntPtr
   'End Function

   <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Unicode, EntryPoint:="CreateFileW")>
   Public Function CreateFile(lpFileName As String, dwDesiredAccess As UInteger, dwShareMode As UInteger, lpSecurityAttributes As IntPtr,
                              dwCreationDisposition As UInteger, dwFlagsAndAttributes As UInteger, hTemplateFile As IntPtr) As SafeFileHandle
   End Function

   <DllImport("kernel32.dll")>
   Public Function CloseHandle(hObject As IntPtr) As Boolean
   End Function

   <DllImport("kernel32.dll", SetLastError:=True)>
   Friend Function GetCommState(hFile As IntPtr, ByRef dcb As DCB) As Boolean
   End Function

   <DllImport("kernel32.dll")>
   Friend Function PurgeComm(hFile As IntPtr, dwFlags As UInteger) As Boolean
   End Function

   <DllImport("kernel32.dll", SetLastError:=True)>
   Friend Function ReadFile(
    hFile As IntPtr,
    lpBuffer As Byte(),
    nNumberOfBytesToRead As Integer,
    ByRef lpNumberOfBytesRead As Integer,
    lpOverlapped As IntPtr) As Boolean
   End Function

   <DllImport("kernel32.dll", SetLastError:=True)>
   Friend Function SetCommState(hFile As IntPtr, ByRef dcb As DCB) As Boolean
   End Function

   <DllImport("kernel32.dll", SetLastError:=True)>
   Friend Function SetCommTimeouts(hFile As IntPtr, ByRef timeouts As COMMTIMEOUTS) As Boolean
   End Function

End Module
