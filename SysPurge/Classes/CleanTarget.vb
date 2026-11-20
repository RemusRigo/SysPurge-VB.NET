'-----------------------------------------------------------------------------------------------
' CleanTarget: one folder plus its clean options (one TaskCleanFolders call)
Public Class CleanTarget
   Public ReadOnly Path As String
   Public ReadOnly Mask As String
   Public ReadOnly Recursive As Boolean
   Public ReadOnly DeleteFolders As Boolean
   Public ReadOnly IncludeParent As Boolean

   Public Sub New(path As String, Optional mask As String = "*.*", Optional recursive As Boolean = True,
                  Optional deleteFolders As Boolean = True, Optional includeParent As Boolean = False)
      Me.Path = path
      Me.Mask = mask
      Me.Recursive = recursive
      Me.DeleteFolders = deleteFolders
      Me.IncludeParent = includeParent
   End Sub

End Class