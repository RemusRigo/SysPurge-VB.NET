'--------------------------------------------------------------------------------------------------
' Shell32
'   © 2026 Remus Rigo
'      v1.0 20261120
'--------------------------------------------------------------------------------------------------

Imports System.Runtime.InteropServices

Namespace API
   Module Shell32

      Public Const SHERB_NOCONFIRMATION As UInteger = &H1
      Public Const SHERB_NOPROGRESSUI As UInteger = &H2
      Public Const SHERB_NOSOUND As UInteger = &H4

      ' SHQUERYRBINFO has a different layout on x86 and x64 (shellapi.h packs it to 1 byte on 32-bit):
      '   x86: cbSize = 20 (no padding)
      '   x64: cbSize = 24 (4 bytes padding after cbSize)
      ' You build both platforms, so pick the right one at runtime via IntPtr.Size.
      <StructLayout(LayoutKind.Sequential, Pack:=4)>
      Public Structure SHQUERYRBINFO32
         Public cbSize As Integer
         Public i64Size As Long
         Public i64NumItems As Long
      End Structure

      <StructLayout(LayoutKind.Sequential, Pack:=8)>
      Public Structure SHQUERYRBINFO64
         Public cbSize As Integer
         Public i64Size As Long
         Public i64NumItems As Long
      End Structure

      <DllImport("shell32.dll", CharSet:=CharSet.Unicode, EntryPoint:="SHQueryRecycleBinW")>
      Public Function SHQueryRecycleBin32(pszRootPath As String, ByRef pSHQueryRBInfo As SHQUERYRBINFO32) As Integer
      End Function

      <DllImport("shell32.dll", CharSet:=CharSet.Unicode, EntryPoint:="SHQueryRecycleBinW")>
      Public Function SHQueryRecycleBin64(pszRootPath As String, ByRef pSHQueryRBInfo As SHQUERYRBINFO64) As Integer
      End Function

      ' pszRootPath = Nothing -> all drives. Returns HRESULT (0 = S_OK)
      <DllImport("shell32.dll", CharSet:=CharSet.Unicode, EntryPoint:="SHEmptyRecycleBinW")>
      Public Function SHEmptyRecycleBin(hwnd As IntPtr, pszRootPath As String, dwFlags As UInteger) As Integer
      End Function

   End Module
End Namespace