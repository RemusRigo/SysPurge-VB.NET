'--------------------------------------------------------------------------------------------------
' libRights - User rights functions
'    © 2026 Remus Rigo
'       v1.1 20260828
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.AccessControl
Imports System.Security.Principal
Imports Microsoft.Win32.SafeHandles

Imports SysPurge.API

Public Module libRights

   '-----------------------------------------------------------------------------------------------
   ' Check if app is running with administrative privileges
   Public Function IsAppElevated() As Boolean
      Dim identity As WindowsIdentity = WindowsIdentity.GetCurrent()
      Dim principal As New WindowsPrincipal(identity)

      Return principal.IsInRole(WindowsBuiltInRole.Administrator)
   End Function

   '-----------------------------------------------------------------------------------------------
   Public Sub EnablePrivilege(privName As String)
      Dim hToken As IntPtr = IntPtr.Zero

      If Not OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES Or TOKEN_QUERY, hToken) Then
         Throw New System.ComponentModel.Win32Exception()
      End If

      Try
         Dim luid As New LUID()
         If Not LookupPrivilegeValue(Nothing, privName, luid) Then
            Throw New System.ComponentModel.Win32Exception()
         End If

         Dim tp As New TOKEN_PRIVILEGES()
         tp.PrivilegeCount = 1
         tp.Privileges = New LUID_AND_ATTRIBUTES With {
             .Luid = luid,
             .Attributes = SE_PRIVILEGE_ENABLED
         }

         Dim retLen As Integer = 0
         Dim prev As New TOKEN_PRIVILEGES()

         If Not AdjustTokenPrivileges(hToken, False, tp, Marshal.SizeOf(tp), prev, retLen) Then
            Throw New System.ComponentModel.Win32Exception()
         End If

         ' AdjustTokenPrivileges succeeds even when the token doesn't hold the privilege
         Const ERROR_NOT_ALL_ASSIGNED As Integer = 1300
         If Marshal.GetLastWin32Error() = ERROR_NOT_ALL_ASSIGNED Then
            Throw New System.ComponentModel.Win32Exception(ERROR_NOT_ALL_ASSIGNED, $"Privilege not held: {privName}")
         End If
      Finally
         CloseHandle(hToken)
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Ownership
   Public Sub TakeOwnership(path As String)
      If String.IsNullOrWhiteSpace(path) Then
         Throw New ArgumentNullException(NameOf(path))
      End If

      If Not Directory.Exists(path) AndAlso Not File.Exists(path) Then
         Throw New IO.FileNotFoundException($"Path not found: {path}")
      End If

      ' Enable required privileges
      EnablePrivilege("SeTakeOwnershipPrivilege")
      EnablePrivilege("SeRestorePrivilege")

      Dim sid As SecurityIdentifier = WindowsIdentity.GetCurrent().User
      Dim sidObj As SecurityIdentifier = DirectCast(sid.Translate(GetType(SecurityIdentifier)), SecurityIdentifier)
      Dim sidPtr As IntPtr = sidObj.BinaryFormPtr()

      ' Set owner
      Dim result As UInteger
      Try
         result = SetNamedSecurityInfo(path, SE_FILE_OBJECT, OWNER_SECURITY_INFORMATION, sidPtr, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
      Finally
         Marshal.FreeHGlobal(sidPtr)
      End Try

      If result <> 0UI Then
         Throw New Win32Exception(CInt(result), $"SetNamedSecurityInfo failed for '{path}'") 'Throw New Exception("SetNamedSecurityInfo failed with error " & result)
      End If
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' ACL
   Public Sub GrantFullControl(path As String)
      Dim user As String = WindowsIdentity.GetCurrent().Name

      Dim di As New DirectoryInfo(path)
      Dim ds = di.GetAccessControl()

      ds.AddAccessRule(New FileSystemAccessRule(
        user,
        FileSystemRights.FullControl,
        InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit,
        PropagationFlags.None,
        AccessControlType.Allow))

      di.SetAccessControl(ds)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' PrepareTreeForDeletion: Makes a protected folder tree (e.g. Windows.old) deletable.
   '   - every folder and file: owner = current user, DACL = Full Control for current user + Administrators
   '   - clears ReadOnly/System/Hidden attributes (they block deletion)
   '   - junctions/symlinks: removes the link itself, never touches the target
   ' Requires elevation. Returns the number of items that could not be processed.
   Public Function PrepareTreeForDeletion(root As String, Optional onError As Action(Of String, Exception) = Nothing) As Integer
      If Not Directory.Exists(root) Then
         Throw New DirectoryNotFoundException($"Path not found: {root}")
      End If

      EnablePrivilege("SeTakeOwnershipPrivilege")
      EnablePrivilege("SeRestorePrivilege")

      ' Build the security descriptor once and reuse it for every item
      Dim sid As String = WindowsIdentity.GetCurrent().User.Value
      Dim sd As New RawSecurityDescriptor($"O:{sid}D:(A;;FA;;;{sid})(A;;FA;;;BA)")
      Dim sdBytes(sd.BinaryLength - 1) As Byte
      sd.GetBinaryForm(sdBytes, 0)

      Dim failures As Integer = 0
      Dim pending As New Stack(Of String)
      pending.Push(root)

      While pending.Count > 0
         Dim dir As String = pending.Pop()

         ' Fix the folder first, so it can be listed
         If Not ResetSecurity(dir, sdBytes, onError) Then
            failures += 1
            Continue While
         End If

         Dim entries As FileSystemInfo()
         Try
            entries = New DirectoryInfo(dir).GetFileSystemInfos()
         Catch ex As Exception
            failures += 1
            onError?.Invoke(dir, ex)
            Continue While
         End Try

         For Each entry As FileSystemInfo In entries
            If (entry.Attributes And FileAttributes.ReparsePoint) <> 0 Then
               ' Junction/symlink: delete the link only (the parent now grants us delete-child)
               Try
                  If (entry.Attributes And FileAttributes.Directory) <> 0 Then
                     Directory.Delete(entry.FullName, False)
                  Else
                     File.Delete(entry.FullName)
                  End If
               Catch ex As Exception
                  failures += 1
                  onError?.Invoke(entry.FullName, ex)
               End Try

            ElseIf (entry.Attributes And FileAttributes.Directory) <> 0 Then
               pending.Push(entry.FullName)

            ElseIf Not ResetSecurity(entry.FullName, sdBytes, onError) Then
               failures += 1
            End If
         Next
      End While

      Return failures
   End Function

   '-----------------------------------------------------------------------------------------------
   ' ResetSecurity: Sets owner + DACL on one item through a handle opened with backup semantics.
   ' SeRestorePrivilege grants WRITE_OWNER/WRITE_DAC regardless of the existing ACL.
   ' FILE_FLAG_OPEN_REPARSE_POINT guarantees a link's target is never modified.
   Private Function ResetSecurity(itemPath As String, sd As Byte(), onError As Action(Of String, Exception)) As Boolean
      Try
         Dim nativePath As String = If(itemPath.StartsWith("\\"), itemPath, "\\?\" & itemPath) ' long path support

         Using h As SafeFileHandle = CreateFile(nativePath, WRITE_OWNER Or WRITE_DAC,
                                                FILE_SHARE_READ Or FILE_SHARE_WRITE Or FILE_SHARE_DELETE, IntPtr.Zero, OPEN_EXISTING,
                                                FILE_FLAG_BACKUP_SEMANTICS Or FILE_FLAG_OPEN_REPARSE_POINT, IntPtr.Zero)
            If h.IsInvalid Then Throw New Win32Exception()

            If Not SetKernelObjectSecurity(h, OWNER_SECURITY_INFORMATION Or DACL_SECURITY_INFORMATION, sd) Then
               Throw New Win32Exception()
            End If
         End Using
      Catch ex As Exception
         onError?.Invoke(itemPath, ex)
         Return False
      End Try

      ' Owner + DACL are set; clearing attributes is best effort and must not mark the item as failed
      ' (a folder reported as failed would have its contents skipped).
      Try
         File.SetAttributes(itemPath, FileAttributes.Normal)
      Catch ex As Exception
         onError?.Invoke(itemPath, ex)
      End Try

      Return True
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Convert SecurityIdentifier to IntPtr
   <System.Runtime.CompilerServices.Extension>
   Public Function BinaryFormPtr(sid As SecurityIdentifier) As IntPtr
      Dim bytes(sid.BinaryLength - 1) As Byte
      sid.GetBinaryForm(bytes, 0)
      Dim ptr = Marshal.AllocHGlobal(bytes.Length)
      Marshal.Copy(bytes, 0, ptr, bytes.Length)
      Return ptr
   End Function

End Module

