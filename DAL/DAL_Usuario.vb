Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_Usuario
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
    Public Function VerificarUsuario(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try

            Dim nombre As SqlParameter = New SqlParameter("@nomUsu", usuario.NombreDeUsuario)

            parametros.Add(nombre)

            Dim i As Object = _dalAcceso.LeerEscalar("Usuario_VerificarUsuario", parametros)

            If i = 1 Then
                resultado = True
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function
    Public Function ObtenerDatos(ByVal usuario As BE.BE_Usuario) As BE.BE_Usuario
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim usuario_conectado As BE.BE_Usuario

        Try

            Dim nombre As SqlParameter = New SqlParameter("@nomUsu", usuario.NombreDeUsuario)

            parametros.Add(nombre)

            Dim tabla As DataTable = _dalAcceso.Leer("Usuario_ObtenerDatos", parametros)

            If tabla.Rows.Count > 0 Then

                usuario_conectado = New BE.BE_Usuario
                usuario_conectado.Idioma = New BE.BE_Idioma
                usuario_conectado.Perfil = New List(Of BE.BE_Permiso)

                For Each registro As DataRow In tabla.Rows

                    Dim permiso As BE.BE_Permiso = New BE.BE_Permiso
                    usuario_conectado.ID = Convert.ToInt32(registro("ID"))
                    usuario_conectado.NombreDeUsuario = registro("NombreUsuario")
                    usuario_conectado.Clave = registro("Clave")
                    usuario_conectado.Idioma.Codigo = registro("idiomaID")
                    usuario_conectado.CantidadDeIntentosFallidos = Convert.ToInt32(registro("intFall"))
                    usuario_conectado.Bloqueado = Convert.ToBoolean(registro("bloqueado"))
                    usuario_conectado.Activo = Convert.ToBoolean(registro("activo"))

                    Dim p As BE.BE_Permiso
                    If registro.IsNull("PGIDG") Then
                        p = New BE.BE_Permiso
                    Else
                        p = New BE.BE_GrupoPermiso
                    End If
                    p.ID = Convert.ToInt32(registro("PUIDP"))
                    p.Nombre = registro("PN")
                    usuario_conectado.Perfil.Add(p)

                Next

                parametros.RemoveRange(0, parametros.Count)

                If usuario_conectado.Perfil.Count > 0 Then

                    For i = 0 To usuario_conectado.Perfil.Count - 1
                        If TypeOf (usuario_conectado.Perfil(i)) Is BE.BE_GrupoPermiso Then
                            Me.ListarPorGrupo(usuario_conectado.Perfil(i))
                        End If
                    Next

                End If

                Dim DAL_Leyenda As DAL_Leyenda = New DAL_Leyenda
                usuario_conectado.Idioma.Leyendas = DAL_Leyenda.ListarPorIdioma(usuario_conectado.Idioma)

            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return usuario_conectado

    End Function
    Public Sub ActualizarIntFallidos(ByVal usuario As BE.BE_Usuario)

        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try

            Dim nomUsu As SqlParameter = New SqlParameter("@nomUsu", usuario.NombreDeUsuario)
            Dim intFall As SqlParameter = New SqlParameter("@intFall", usuario.CantidadDeIntentosFallidos)
            Dim dvh As SqlParameter = New SqlParameter("@dvh", usuario.DVH)

            parametros.Add(nomUsu)
            parametros.Add(intFall)
            parametros.Add(dvh)

            Dim resultado As Boolean = (_dalAcceso.Escribir("Usuario_ActIntFall", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "El campo 'intentos fallidos' fue actualizado exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actualizar el campo 'intentos fallidos': " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actualizar el campo 'intentos fallidos': " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

    End Sub

    Public Function ListarTodos() As List(Of BE.BE_Usuario)
        Dim usuarios As List(Of BE.BE_Usuario) = New List(Of BE.BE_Usuario)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim usuario As BE.BE_Usuario

        Try

            Dim tabla As DataTable = _dalAcceso.Leer("Usuario_ListarTodos")

            For Each registro As DataRow In tabla.Rows

                usuario = New BE.BE_Usuario
                usuario.Idioma = New BE.BE_Idioma
                usuario.ID = Convert.ToInt32(registro("ID"))
                usuario.NombreDeUsuario = registro("NombreUsuario").ToString
                usuario.Clave = registro("Clave").ToString
                usuario.Idioma.Codigo = registro("Codigo").ToString
                usuario.Idioma.Nombre = registro("Nombre").ToString
                usuario.CantidadDeIntentosFallidos = Convert.ToInt32(registro("intFall"))
                usuario.Bloqueado = Convert.ToBoolean(registro("bloqueado"))
                usuario.Activo = Convert.ToBoolean(registro("activo"))

                usuarios.Add(usuario)

            Next

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar todos los usuarios: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar todos los usuarios: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return usuarios

    End Function
    Public Function CambiarIdioma(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            Dim idUsuario As SqlParameter = New SqlParameter("@idUsuario", usuario.ID)
            Dim idIdioma As SqlParameter = New SqlParameter("@idIdioma", usuario.Idioma.Codigo)
            Dim dvh As SqlParameter = New SqlParameter("@dvh", usuario.DVH)

            parametros.Add(idUsuario)
            parametros.Add(idIdioma)
            parametros.Add(dvh)

            resultado = (_dalAcceso.Escribir("Usuario_CambiarIdioma", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Idioma de usuario cambiado exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al cambiar el idioma del usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al modificar el idioma del usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function

    Public Function RegistrarUsuario(ByVal usuario As BE.BE_Usuario) As Boolean

        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            Dim id As SqlParameter = New SqlParameter("@id", usuario.ID)
            Dim nomUsu As SqlParameter = New SqlParameter("@nomUsu", usuario.NombreDeUsuario)
            Dim clave As SqlParameter = New SqlParameter("@clave", usuario.Clave)
            Dim idiomaID As SqlParameter = New SqlParameter("@idiomaID", usuario.Idioma.Codigo)
            Dim intFall As SqlParameter = New SqlParameter("@intFall", usuario.CantidadDeIntentosFallidos)
            Dim bloqueado As SqlParameter = New SqlParameter("@bloqueado", usuario.Bloqueado)
            Dim activo As SqlParameter = New SqlParameter("@activo", usuario.Activo)
            Dim dvh As SqlParameter = New SqlParameter("@dvh", usuario.DVH)

            parametros.Add(id)
            parametros.Add(nomUsu)
            parametros.Add(clave)
            parametros.Add(idiomaID)
            parametros.Add(intFall)
            parametros.Add(bloqueado)
            parametros.Add(activo)
            parametros.Add(dvh)

            resultado = (_dalAcceso.Escribir("Usuario_Agregar", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Usuario registrado exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al registrar usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al registrar usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado

    End Function

    Public Function EliminarUsuario(ByVal usuario As BE.BE_Usuario) As Boolean

        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            Dim id As SqlParameter = New SqlParameter("@id", usuario.ID)

            parametros.Add(id)

            resultado = (_dalAcceso.Escribir("Usuario_Eliminar", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Usuario eliminado exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al eliminar usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al eliminar usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function

    Public Function ObtenerMaxID() As Integer

        Dim tabla As DataTable
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim id As Integer

        Try
            tabla = _dalAcceso.Leer("Usuario_ObtenerMaxID")

            For Each registro As DataRow In tabla.Rows
                id = registro("ID")
            Next

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al obtener el ID máximo: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al obtener el ID máximo: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return id

    End Function
    Public Function Bloquear(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            Dim nomUsu As SqlParameter = New SqlParameter("@nomUsu", usuario.NombreDeUsuario)
            Dim bloqueado As SqlParameter = New SqlParameter("@bloqueado", usuario.Bloqueado)
            Dim intFall As SqlParameter = New SqlParameter("@intFall", usuario.CantidadDeIntentosFallidos)
            Dim dvh As SqlParameter = New SqlParameter("@dvh", usuario.DVH)

            parametros.Add(nomUsu)
            parametros.Add(bloqueado)
            parametros.Add(intFall)
            parametros.Add(dvh)

            resultado = (_dalAcceso.Escribir("Usuario_Bloquear", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Usuario bloqueado exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al bloquear usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al bloquear usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function
    Public Function ModificarUsuario(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            Dim id As SqlParameter = New SqlParameter("@id", usuario.ID)
            Dim nomUsu As SqlParameter = New SqlParameter("@nomUsu", usuario.NombreDeUsuario)
            Dim clave As SqlParameter = New SqlParameter("@clave", usuario.Clave)
            Dim idioma As SqlParameter = New SqlParameter("@idiomaID", usuario.Idioma.Codigo)
            Dim intFall As SqlParameter = New SqlParameter("@intFall", usuario.CantidadDeIntentosFallidos)
            Dim bloqueado As SqlParameter = New SqlParameter("@bloq", usuario.Bloqueado)
            Dim activo As SqlParameter = New SqlParameter("@activo", usuario.Activo)
            Dim dvh As SqlParameter = New SqlParameter("@dvh", usuario.DVH)

            parametros.Add(id)
            parametros.Add(nomUsu)
            parametros.Add(clave)
            parametros.Add(idioma)
            parametros.Add(intFall)
            parametros.Add(bloqueado)
            parametros.Add(activo)
            parametros.Add(dvh)

            resultado = (_dalAcceso.Escribir("Usuario_Modificar", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Usuario actualizado exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actualizar usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actualizar usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function
    Public Function VerificarCambioNombre(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try
            Dim id As SqlParameter = New SqlParameter("@id", usuario.ID)
            Dim nombre As SqlParameter = New SqlParameter("@nomUsu", usuario.NombreDeUsuario)

            parametros.Add(id)
            parametros.Add(nombre)

            Dim i As Object = _dalAcceso.LeerEscalar("Usuario_VerificarCambioNombre", parametros)

            If i > 0 Then
                resultado = True
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException

            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function
    Public Function ActualizarContraseña(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            Dim id As SqlParameter = New SqlParameter("@id", usuario.ID)
            Dim clave As SqlParameter = New SqlParameter("@clave", usuario.Clave)
            Dim dvh As SqlParameter = New SqlParameter("@dvh", usuario.DVH)

            parametros.Add(id)
            parametros.Add(clave)
            parametros.Add(dvh)

            resultado = (_dalAcceso.Escribir("Usuario_CambiarContraseña", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Cotraseña actualizada exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actualizar contraseña: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actualizar contraseña: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function

    Public Function BajaLogica(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            Dim id As SqlParameter = New SqlParameter("@id", usuario.ID)
            Dim activo As SqlParameter = New SqlParameter("@activo", usuario.Activo)
            Dim dvh As SqlParameter = New SqlParameter("@dvh", usuario.DVH)

            parametros.Add(id)
            parametros.Add(activo)
            parametros.Add(dvh)

            resultado = (_dalAcceso.Escribir("Usuario_BajaLogica", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Usuario dado de baja exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al realizar la baja lógica de un usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error realizar la baja lógica de un usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function

    Private Sub ListarPorGrupo(ByRef perfil As BE.BE_GrupoPermiso)
        Dim param As SqlParameter = New SqlParameter("@ID_Grupo", perfil.ID)
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        parametros.Add(param)
        Dim tabla As DataTable = _dalAcceso.Leer("PermisoGrupo_ListarPorGrupo", parametros)
        For Each registro In tabla.Rows
            Dim grupo As BE.BE_GrupoPermiso = New BE.BE_GrupoPermiso
            grupo.ID = Convert.ToInt32(registro("ID_Permiso"))
            grupo.Nombre = registro("Nombre")
            Me.ListarPorGrupo(grupo)
            If grupo.Permisos.Count > 0 Then
                DirectCast(perfil, BE.BE_GrupoPermiso).Permisos.Add(grupo)
            Else
                Dim p As BE.BE_Permiso = New BE.BE_Permiso
                p.ID = Convert.ToInt32(registro("ID_Permiso"))
                p.Nombre = registro("Nombre")
                DirectCast(perfil, BE.BE_GrupoPermiso).Permisos.Add(p)
            End If
        Next
    End Sub

End Class
