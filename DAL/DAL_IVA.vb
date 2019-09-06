Public Class DAL_IVA
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia.ObtenerUsuarioActual()

    Public Function ListarTodo() As List(Of BE.BE_IVA)
        Dim listadoIVA As List(Of BE.BE_IVA) = New List(Of BE.BE_IVA)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora

        Try

            Dim tabla As DataTable = DAL_Acceso.Leer("IVA_ListarTodo")

            For Each registro As DataRow In tabla.Rows

                Dim iva As BE.BE_IVA = New BE.BE_IVA
                iva.ID = registro("ID").ToString
                iva.Nombre = registro("Nombre").ToString

                listadoIVA.Add(iva)

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
            BE_Bitacora.Descripcion = "Error al listar los IVA: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar los IVA: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return listadoIVA

    End Function

End Class
