Public Class SEG_Permiso

    Dim DAL_Permiso As DAL.DAL_Permiso = New DAL.DAL_Permiso

    Public Overridable Function Validar(ByVal permiso As BE.BE_Permiso, _
                                        ByVal permisos As List(Of BE.BE_Permiso)) As Boolean
        Dim resultado As Boolean

        For Each p As BE.BE_Permiso In permisos
            If p.ID = permiso.ID Then
                resultado = True
                Exit For
            End If
        Next

        Return resultado

    End Function
    Public Function listarPermisos() As List(Of BE.BE_Permiso)
        Return DAL_Permiso.ListarPermisos()
    End Function
    Public Function ObtenerMaxID() As Integer
        Return DAL_Permiso.ObtenerMaxID()
    End Function
    Public Function AgregarPermisoUsuario(ByVal usuario As BE.BE_Usuario) As Boolean
        Return DAL_Permiso.AgregarPermisoUsuario(usuario)
    End Function
    Public Function EliminarPermisoUsuario(ByVal usuario As BE.BE_Usuario) As Boolean
        Return DAL_Permiso.EliminarPermisoUsuario(usuario)
    End Function
    Public Function ListarPermisosPorUsuario(ByVal usuario As BE.BE_Usuario) As List(Of BE.BE_Permiso)
        Return DAL_Permiso.ListarPermisosPorUsuario(usuario)
    End Function
    Public Function ListarTodoString() As List(Of String)
        Dim permisos As List(Of BE.BE_Permiso) = Me.listarPermisos()
        Dim listaDeStrings As List(Of String) = New List(Of String)
        Dim cadena As String
        For i = 0 To permisos.Count - 1
            cadena = permisos(i).ID.ToString & permisos(i).Nombre
            listaDeStrings.Add(cadena)
        Next
        Return listaDeStrings
    End Function
End Class
