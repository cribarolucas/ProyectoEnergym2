Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_TipoFactura
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL.DAL_Acceso

    Public Function ListarPorID(ByVal tf As BE.BE_TipoFactura) As BE.BE_TipoFactura
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim tipoFactura As BE.BE_TipoFactura = New BE.BE_TipoFactura
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try
            Dim id As SqlParameter = New SqlParameter("@id", tf.ID)
            parametros.Add(id)

            Dim tabla As DataTable = DAL_Acceso.Leer("Tipo_Factura_ListarPorID", parametros)

            For Each registro As DataRow In tabla.Rows

                tipoFactura.ID = registro("ID").ToString
                tipoFactura.Tipo = registro("Tipo").ToString

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
            BE_Bitacora.Descripcion = "Error al leer Tipo Factura: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer Tipo Factura: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return tipoFactura

    End Function


End Class
