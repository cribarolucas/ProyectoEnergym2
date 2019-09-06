Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_Leyenda
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia.ObtenerUsuarioActual()
    Public Function ListarPorIdioma(ByVal idioma As BE.BE_Idioma) As List(Of BE.BE_Leyenda)
        Dim leyendas As List(Of BE.BE_Leyenda) = New List(Of BE.BE_Leyenda)
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try

            Dim codID As SqlParameter = New SqlParameter("@codID", idioma.Codigo)
            parametros.Add(codID)

            Dim tabla As DataTable = DAL_Acceso.Leer("Idioma_Leyenda_ListarPorIdioma", parametros)

            For Each registro As DataRow In tabla.Rows

                Dim l As BE.BE_Leyenda = New BE.BE_Leyenda
                l.Codigo = registro("CodLey").ToString
                l.Descripcion = registro("Descripcion").ToString

                leyendas.Add(l)

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
            BE_Bitacora.Descripcion = "Error al listar leyendas por idioma: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar leyendas por idioma: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return leyendas

    End Function
    Public Function Listar() As List(Of BE.BE_Leyenda)
        Dim leyendas As List(Of BE.BE_Leyenda) = New List(Of BE.BE_Leyenda)
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try

            Dim tabla As DataTable = DAL_Acceso.Leer("Leyenda_Listar")

            For Each registro As DataRow In tabla.Rows

                Dim l As BE.BE_Leyenda = New BE.BE_Leyenda
                l.Codigo = registro("CodLey").ToString

                leyendas.Add(l)

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
            BE_Bitacora.Descripcion = "Error al leer las leyendas: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer las leyendas: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return leyendas

    End Function
    Public Function EliminarPorIdioma(ByVal idioma As BE.BE_Idioma) As Boolean

        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try

            Dim codID As SqlParameter = New SqlParameter("@codID", idioma.Codigo)

            parametros.Add(codID)

            resultado = (DAL_Acceso.Escribir("Leyenda_EliminarPorIdioma", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Leyendas eliminadas exitosamente."
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
            BE_Bitacora.Descripcion = "Error al eliminar leyendas: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al eliminar leyendas: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function
    Public Function AgregarIdiomaLeyenda(ByVal idioma As BE.BE_Idioma, ByVal leyendas As List(Of BE.BE_Leyenda)) As Boolean

        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim codLey As SqlParameter
        Dim codID As SqlParameter

        Try

            For i = 0 To leyendas.Count - 1
                codLey = New SqlParameter("@codLey", leyendas(i).Codigo)
                codID = New SqlParameter("@codID", idioma.Codigo)

                parametros.Add(codID)
                parametros.Add(codLey)

                resultado = (DAL_Acceso.Escribir("Idioma_Leyenda_Agregar", parametros) > 0)

                If resultado Then
                    parametros.Remove(codID)
                    parametros.Remove(codLey)
                Else
                    Exit For
                End If

            Next

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Leyenda agregada exitosamente."
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
            BE_Bitacora.Descripcion = "Error al agregar leyenda: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al agregar leyenda: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function

    Public Function ModificarIdiomaLeyenda(ByVal leyenda As BE.BE_Leyenda, ByVal idioma As BE.BE_Idioma) As Boolean

        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try

            Dim codLey As SqlParameter = New SqlParameter("@codLey", leyenda.Codigo)
            Dim desc As SqlParameter = New SqlParameter("@desc", leyenda.Descripcion)
            Dim codId As SqlParameter = New SqlParameter("@codID", idioma.Codigo)

            parametros.Add(codId)
            parametros.Add(codLey)
            parametros.Add(desc)

            resultado = (DAL_Acceso.Escribir("Idioma_Leyenda_Modificar", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Leyenda modificada exitosamente."
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
            BE_Bitacora.Descripcion = "Error al modificar leyenda: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al modificar leyenda: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return resultado

    End Function

End Class
