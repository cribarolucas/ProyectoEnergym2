Public Class BE_GrupoPermiso
    Inherits BE_Permiso

    Public Sub New()
        _permisos = New List(Of BE_Permiso)
    End Sub

    Private _permisos As List(Of BE_Permiso)
    Public ReadOnly Property Permisos() As List(Of BE_Permiso)
        Get
            Return _permisos
        End Get
    End Property

End Class
