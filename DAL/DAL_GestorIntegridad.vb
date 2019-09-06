Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_GestorIntegridad
    Private DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia.ObtenerUsuarioActual()

    Public Function listarDVV(nombreTabla As String) As String
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim dvv As String

        Try

            Dim p As SqlParameter = New SqlParameter("@tabla", nombreTabla)

            parametros.Add(p)

            Dim tabla As DataTable = DAL_Acceso.Leer("DVV_Listar", parametros)

            For Each registro As DataRow In tabla.Rows

                dvv = registro("DigitoVerificador").ToString

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
            BE_Bitacora.Descripcion = "Error al listar DVV: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar DVV: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return dvv

    End Function

    Public Function ListarDVH(ByVal nombreTabla As String) As List(Of String)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim dvh As String
        Dim listaDeDVH As List(Of String) = New List(Of String)
        Dim sp As String

        Try

            Select Case nombreTabla
                Case "Usuario"
                    sp = "Usuario_ListarDVH"
                Case "Permiso"
                    sp = "Permiso_ListarDVH"
                Case "Factura"
                    sp = "Factura_ListarDVH"
                Case Else
                    'No hacer nada
            End Select

            Dim tabla As DataTable = DAL_Acceso.Leer(sp)

            For Each registro As DataRow In tabla.Rows

                dvh = registro("DVH")

                listaDeDVH.Add(dvh)

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
            BE_Bitacora.Descripcion = "Error al listar DVH: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar DVH: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return listaDeDVH

    End Function
    Public Sub ActualizarDVV(ByVal tabla As String, ByVal dvv As String)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Try

            Dim sp As String
            If Me.DVV_ExisteTabla(tabla) Then
                sp = "DVV_Actualizar"
            Else
                sp = "DVV_Insertar"
            End If

            Dim t As SqlParameter = New SqlParameter("@tabla", tabla)
            Dim digVV As SqlParameter = New SqlParameter("@dvv", dvv)

            parametros.Add(t)
            parametros.Add(digVV)

            resultado = (DAL_Acceso.Escribir(sp, parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Tabla DVV actualizada exitosamente."
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
            BE_Bitacora.Descripcion = "Error al actualizar DVV: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actualizar DVV: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

    End Sub
    Private Function DVV_ExisteTabla(tabla As String) As Boolean
        Dim idioma As BE.BE_Idioma = New BE.BE_Idioma
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean

        Try

            Dim p As SqlParameter = New SqlParameter("@tabla", tabla)

            parametros.Add(p)

            Dim i As Integer = DAL_Acceso.LeerEscalar("DVV_ExisteTabla", parametros)

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
            BE_Bitacora.Descripcion = "Error al verificar el valor de un campo en la tabla DVV: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al verificar el valor de un campo en la tabla DVV: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function

End Class
