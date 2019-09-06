Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_Provincia
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL_Acceso
    Public Function ListarProvincias() As List(Of BE.BE_Provincia)
        Dim provincias As List(Of BE.BE_Provincia) = New List(Of BE.BE_Provincia)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim p As BE.BE_Provincia
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try
            Dim dt As DataTable = _dalAcceso.Leer("Provincia_ListarTodo")
            For Each registro As DataRow In dt.Rows
                p = New BE.BE_Provincia
                p.ID = Convert.ToInt32(registro("ID"))
                p.Nombre = registro("Nombre")
                provincias.Add(p)
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
            BE_Bitacora.Descripcion = "Error al listar provincias: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar provincias: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return provincias
    End Function
End Class
