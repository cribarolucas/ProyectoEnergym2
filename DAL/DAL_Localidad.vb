Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_Localidad
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL_Acceso

    Public Function ListarPorProvincia(ByVal provincia As BE.BE_Provincia) As List(Of BE.BE_Localidad)
        Dim localidades As List(Of BE.BE_Localidad) = New List(Of BE.BE_Localidad)
        Dim l As BE.BE_Localidad
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try
            Dim p As SqlParameter = New SqlParameter("id_provincia", provincia.ID)
            parametros.Add(p)

            Dim dt As DataTable = _dalAcceso.Leer("Localidad_ListarPorProvincia", parametros)
            For Each registro As DataRow In dt.Rows
                l = New BE.BE_Localidad
                l.ID = Convert.ToInt32(registro("ID"))
                l.Nombre = registro("Nombre")
                localidades.Add(l)
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
            BE_Bitacora.Descripcion = "Error al listar localidades: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar localidades: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return localidades
    End Function

End Class
