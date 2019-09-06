Public Class SEG_Usuario
    Dim DAL_Usuario As DAL.DAL_Usuario = New DAL.DAL_Usuario
    Dim SEG_Permiso As SEG_Permiso = New SEG_Permiso
    Dim SEG_GestorIntegridad As SEG_GestorIntegridad = New SEG_GestorIntegridad

    Public Function VerificarDatos(ByVal usuario As BE.BE_Usuario) As Boolean
        Return DAL_Usuario.VerificarUsuario(usuario)
    End Function

    Public Function ObtenerDatos(ByRef usuario As BE.BE_Usuario) As BE.BE_Usuario
        Dim SEG_GestorCifrado As SEG_GestorCifrado = New SEG_GestorCifrado

        'Obtengo todos los datos del usuario que está intentando conectarse
        Dim u As BE.BE_Usuario = DAL_Usuario.ObtenerDatos(usuario)

        'Comparo el HashMD5 de la clave ingresada por pantalla y la clave almacenada en BD
        If SEG_GestorCifrado.CompareHashMD5(usuario.Clave, u.Clave) Then
            Return u
        Else
            usuario = u
            Return Nothing
        End If
    End Function
    Public Sub ActualizarIntFallidos(ByRef usuario As BE.BE_Usuario, ByVal logueado As Boolean)
        Dim SEG_GestorCifrado As SEG_GestorCifrado = New SEG_GestorCifrado

        'Si existe el usuario, pero ingresó una contraseña incorrecta
        'sumo 1 a la cantidad de intentos fallidos, caso contrario,
        'blanqueo ese campo
        If Not logueado Then
            usuario.CantidadDeIntentosFallidos += 1
        End If

        If usuario.CantidadDeIntentosFallidos >= 3 Then
            usuario.Bloqueado = True
            usuario.CantidadDeIntentosFallidos = 3
            usuario.DVH = SEG_GestorCifrado.GetHashMD5(usuario.ID.ToString & usuario.NombreDeUsuario & usuario.Clave & _
               usuario.Idioma.Codigo & usuario.CantidadDeIntentosFallidos.ToString & _
               usuario.Bloqueado.ToString & usuario.Activo.ToString)

            DAL_Usuario.Bloquear(usuario)
        Else
            usuario.DVH = SEG_GestorCifrado.GetHashMD5(usuario.ID.ToString & usuario.NombreDeUsuario & usuario.Clave & _
               usuario.Idioma.Codigo & usuario.CantidadDeIntentosFallidos.ToString & _
               usuario.Bloqueado.ToString & usuario.Activo.ToString)
            DAL_Usuario.ActualizarIntFallidos(usuario)
        End If

        SEG_GestorIntegridad.ActualizarDVV("Usuario")

    End Sub
    Public Function ListarTodoString() As List(Of String)
        Dim usuarios As List(Of BE.BE_Usuario) = DAL_Usuario.ListarTodos()
        Dim listaDeStrings As List(Of String) = New List(Of String)
        Dim cadena As String
        For i = 0 To usuarios.Count - 1
            cadena = usuarios(i).ID.ToString & usuarios(i).NombreDeUsuario & usuarios(i).Clave & _
                     usuarios(i).Idioma.Codigo & usuarios(i).CantidadDeIntentosFallidos.ToString & _
                     usuarios(i).Bloqueado.ToString & usuarios(i).Activo.ToString
            listaDeStrings.Add(cadena)
        Next
        Return listaDeStrings
    End Function
    Public Function ListarTodos() As List(Of BE.BE_Usuario)
        Return DAL_Usuario.ListarTodos()
    End Function
    Public Function CambiarIdioma(ByVal usuario As BE.BE_Usuario, ByVal idioma As BE.BE_Idioma) As Boolean
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New SEG_GestorCifrado
        Dim resultado As Boolean
        usuario.DVH = SEG_GestorCifrado.GetHashMD5(usuario.ID.ToString & usuario.NombreDeUsuario & usuario.Clave & _
                                           usuario.Idioma.Codigo & usuario.CantidadDeIntentosFallidos.ToString & _
                                           usuario.Bloqueado.ToString & usuario.Activo.ToString)
        If DAL_Usuario.CambiarIdioma(usuario) Then
            SEG_GestorIntegridad.ActualizarDVV("Usuario")
            resultado = True
        End If
        Return resultado
    End Function
    Public Function RegistrarUsuario(ByRef usuario As BE.BE_Usuario) As Boolean
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New SEG_GestorCifrado
        Dim resultado As Boolean
        usuario.ID = DAL_Usuario.ObtenerMaxID() + 1
        usuario.DVH = SEG_GestorCifrado.GetHashMD5(usuario.ID.ToString & usuario.NombreDeUsuario & usuario.Clave & _
                                                   usuario.Idioma.Codigo & usuario.CantidadDeIntentosFallidos.ToString & _
                                                   usuario.Bloqueado.ToString & usuario.Activo.ToString)
        If DAL_Usuario.RegistrarUsuario(usuario) Then
            SEG_GestorIntegridad.ActualizarDVV("Usuario")
            resultado = SEG_Permiso.AgregarPermisoUsuario(usuario)
        End If
        Return resultado
    End Function
    Public Function EliminarUsuario(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim resultado As Boolean
        If DAL_Usuario.EliminarUsuario(usuario) Then
            usuario.Perfil = SEG_Permiso.ListarPermisosPorUsuario(usuario)
            If SEG_Permiso.EliminarPermisoUsuario(usuario) Then
                SEG_GestorIntegridad.ActualizarDVV("Usuario")
                resultado = True
            Else
                SEG_Permiso.AgregarPermisoUsuario(usuario)
            End If
        End If
        Return resultado
    End Function
    Public Function ModificarUsuario(ByRef usuario As BE.BE_Usuario) As Boolean
        Dim resultado As Boolean
        If SEG_Permiso.EliminarPermisoUsuario(usuario) Then
            If SEG_Permiso.AgregarPermisoUsuario(usuario) Then
                Dim SEG_GestorCifrado As SEG_GestorCifrado = New SEG_GestorCifrado
                usuario.DVH = SEG_GestorCifrado.GetHashMD5(usuario.ID.ToString & usuario.NombreDeUsuario & usuario.Clave & _
                                           usuario.Idioma.Codigo & usuario.CantidadDeIntentosFallidos.ToString & _
                                           usuario.Bloqueado.ToString & usuario.Activo.ToString)
                If DAL_Usuario.ModificarUsuario(usuario) Then
                    SEG_GestorIntegridad.ActualizarDVV("Usuario")
                    resultado = True
                End If
            End If
        End If
        Return resultado
    End Function
    Public Function VerificarCambioNombre(ByVal usuario As BE.BE_Usuario) As Boolean
        Return DAL_Usuario.VerificarCambioNombre(usuario)
    End Function
    Public Function ActualizarContraseña(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim resultado As Boolean
        Dim SEG_GestorCifrado As SEG_GestorCifrado = New SEG_GestorCifrado
        usuario.DVH = SEG_GestorCifrado.GetHashMD5(usuario.ID.ToString & usuario.NombreDeUsuario & usuario.Clave & _
                                   usuario.Idioma.Codigo & usuario.CantidadDeIntentosFallidos.ToString & _
                                   usuario.Bloqueado.ToString & usuario.Activo.ToString)
        If DAL_Usuario.ActualizarContraseña(usuario) Then
            SEG_GestorIntegridad.ActualizarDVV("Usuario")
            resultado = True
        End If
        Return resultado
    End Function
    Public Function VerificarContraseña(ByVal usuario As BE.BE_Usuario, ByVal usuarioConectado As BE.BE_Usuario) As Boolean
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New SEG_GestorCifrado
        Return SEG_GestorCifrado.CompareHashMD5(usuario.Clave, usuarioConectado.Clave)
    End Function
    Public Function BajaLogica(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim resultado As Boolean
        Dim SEG_GestorCifrado As SEG_GestorCifrado = New SEG_GestorCifrado
        usuario.DVH = SEG_GestorCifrado.GetHashMD5(usuario.ID.ToString & usuario.NombreDeUsuario & usuario.Clave & _
                                   usuario.Idioma.Codigo & usuario.CantidadDeIntentosFallidos.ToString & _
                                   usuario.Bloqueado.ToString & usuario.Activo.ToString)
        If DAL_Usuario.BajaLogica(usuario) Then
            SEG_GestorIntegridad.ActualizarDVV("Usuario")
            resultado = True
        End If
        Return resultado
    End Function
End Class
