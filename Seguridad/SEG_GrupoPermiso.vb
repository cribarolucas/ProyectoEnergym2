Public Class SEG_GrupoPermiso
    Inherits SEG_Permiso

    Private DAL_Permiso As DAL.DAL_Permiso = New DAL.DAL_Permiso
    Private SEG_GestorIntegridad As SEG_GestorIntegridad = New SEG_GestorIntegridad

    Public Overrides Function Validar(ByVal permisoDeLaPagina As BE.BE_Permiso, _
                                      ByVal permisosDelUsuario As List(Of BE.BE_Permiso)) As Boolean

        Dim encontrado As Boolean

        For i = 0 To permisosDelUsuario.Count - 1
            If TypeOf (permisosDelUsuario.Item(i)) Is BE.BE_GrupoPermiso Then
                'Como es un Perfil, le paso la lista de permisos del mismo
                If Me.Validar(permisoDeLaPagina, DirectCast(permisosDelUsuario.Item(i), BE.BE_GrupoPermiso).Permisos) Then
                    encontrado = True
                    Exit For
                End If
            Else
                Dim permisos As List(Of BE.BE_Permiso) = New List(Of BE.BE_Permiso)
                permisos.Add(permisosDelUsuario.Item(i))
                If MyBase.Validar(permisoDeLaPagina, permisos) Then
                    encontrado = True
                    Exit For
                End If
            End If
        Next

        Return encontrado

    End Function

    Public Function agregarPerfil(ByRef perfil As BE.BE_GrupoPermiso) As Boolean
        Dim resultado As Boolean = True
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New SEG_GestorCifrado
        'Primero agrego el nuevo Perfil a la tabla Permiso,
        'luego agrego el perfil a la tabla GrupoPermiso
        perfil.ID = DAL_Permiso.ObtenerMaxID() + 1
        perfil.DVH = SEG_GestorCifrado.GetHashMD5(perfil.ID & perfil.Nombre)

        If DAL_Permiso.AgregarPermiso(perfil) Then
            For i = 0 To perfil.Permisos.Count - 1
                If DAL_Permiso.AgregarPerfil(perfil, perfil.Permisos(i)) Then
                    SEG_GestorIntegridad.ActualizarDVV("Permiso")
                Else
                    resultado = False
                End If
            Next
        Else
            resultado = False
        End If
        Return resultado
    End Function

    Public Function eliminarPerfil(ByVal perfil As BE.BE_GrupoPermiso) As Boolean
        Dim resultado As Boolean
        Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
        BE_Permiso.ID = perfil.ID
        If DAL_Permiso.EliminarPerfil(perfil) Then
            If DAL_Permiso.EliminarPermiso(BE_Permiso) Then
                SEG_GestorIntegridad.ActualizarDVV("Permiso")
                resultado = True
            End If
        End If
        Return resultado
    End Function
    Public Function VerificarCambioNombre(ByVal perfil As BE.BE_GrupoPermiso) As Boolean
        'Verifical que el nombre elegido no pertenezca a otro perfil
        Return DAL_Permiso.VerificarCambioNombre(perfil)
    End Function

    Public Function ModificarPerfil(ByVal perfil As BE.BE_GrupoPermiso) As Boolean
        Dim resultado As Boolean = True
            If DAL_Permiso.EliminarPerfil(perfil) Then
                For Each permiso As BE.BE_Permiso In perfil.Permisos
                    If Not DAL_Permiso.AgregarPerfil(perfil, permiso) Then
                        resultado = False
                    End If
                Next
            End If
        Return resultado
    End Function

    Public Function VerificarRelacionUsuarioPerfil(ByVal perfil As BE.BE_GrupoPermiso) As Boolean
        Return DAL_Permiso.PermisoUsuarioExiste(perfil)
    End Function

End Class
