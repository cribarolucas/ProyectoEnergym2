Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_OrdenProduccion
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
    Public Function ListarPorFechaEstado(ByVal estado As BE.BE_Estado, ByVal fIni As DateTime, fFin As DateTime) As List(Of BE.BE_OrdenProduccion)
        Dim ordenes As List(Of BE.BE_OrdenProduccion) = New List(Of BE.BE_OrdenProduccion)
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim ordenProduccion As BE.BE_OrdenProduccion
        Dim detalle As BE.BE_OrdenProduccionDetalle

        Try
            Dim id_estado As SqlParameter = New SqlParameter("@id_estado", estado.ID)
            Dim fechaIni As SqlParameter = New SqlParameter("@fechaIni", fIni)
            Dim fechaFin As SqlParameter = New SqlParameter("@fechaFin", fFin)

            parametros.Add(id_estado)
            parametros.Add(fechaIni)
            parametros.Add(fechaFin)

            Dim tabla As DataTable = DAL_Acceso.Leer("OrdenProduccion_ListarPorFechaEstado", parametros)

            For Each registro As DataRow In tabla.Rows

                ordenProduccion = New BE.BE_OrdenProduccion
                ordenProduccion.ID = Convert.ToInt32(registro("ID"))
                ordenProduccion.Estado.ID = Convert.ToInt32(registro("ID_Estado"))
                ordenProduccion.Estado.Nombre = registro("EstadoNombre")
                ordenProduccion.FechaInicio = Convert.ToDateTime(registro("Fecha_Inicio"))
                If IsDBNull(registro("Fecha_Fin")) Then
                    ordenProduccion.FechaFin = Nothing
                Else
                    ordenProduccion.FechaFin = Convert.ToDateTime(registro("Fecha_Fin"))
                End If

                ordenes.Add(ordenProduccion)

            Next

            parametros.RemoveRange(0, parametros.Count)

            For i = 0 To ordenes.Count - 1

                Dim id_op As SqlParameter = New SqlParameter("@id_op", ordenes.Item(i).ID)

                parametros.Add(id_op)

                Dim tabla2 As DataTable = DAL_Acceso.Leer("OrdenProduccion_Producto_ListarPorOP", parametros)

                For Each registro As DataRow In tabla2.Rows
                    detalle = New BE.BE_OrdenProduccionDetalle
                    detalle.Cantidad = Convert.ToInt32(registro("Cantidad"))
                    detalle.Producto.ID = Convert.ToInt32(registro("ID_Producto"))
                    detalle.Producto.Nombre = registro("ProductoNombre")
                    ordenes.Item(i).Detalles.Add(detalle)
                Next

                parametros.RemoveRange(0, parametros.Count)

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
            BE_Bitacora.Descripcion = "Error al listar todas las órdenes de producción: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar todas las órdenes de producción: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return ordenes

    End Function

    Public Function GenerarOrdenProduccion(ByRef op As BE.BE_OrdenProduccion) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim id As SqlParameter = New SqlParameter("@id", SqlDbType.Int)
            id.Direction = System.Data.ParameterDirection.Output
            Dim id_estado As SqlParameter = New SqlParameter("@id_estado", op.Estado.ID)
            Dim fechaIni As SqlParameter = New SqlParameter("@fechaIni", op.FechaInicio)
            fechaIni.DbType = DbType.DateTime

            parametros.Add(id)
            parametros.Add(id_estado)
            parametros.Add(fechaIni)

            resultado = (_dalAcceso.Escribir("OrdenProduccion_Generar", parametros) > 0)

            If resultado Then

                op.ID = parametros(0).Value

                For i = 0 To op.Detalles.Count - 1

                    parametros.RemoveRange(0, parametros.Count)

                    Dim id_op As SqlParameter = New SqlParameter("@id_op", op.ID)
                    Dim id_p As SqlParameter = New SqlParameter("@id_p", op.Detalles.Item(i).Producto.ID)
                    Dim cant As SqlParameter = New SqlParameter("@cant", op.Detalles.Item(i).Cantidad)

                    parametros.Add(id_op)
                    parametros.Add(id_p)
                    parametros.Add(cant)

                    resultado = (_dalAcceso.Escribir("OrdenProduccion_Producto_Agregar", parametros) > 0)

                Next

                If resultado Then
                    BE_Bitacora.FechaHora = DateTime.Now
                    BE_Bitacora.Usuario = _usuarioActual
                    BE_Bitacora.Descripcion = "Orden de producción generada exitosamente."
                    BE_Bitacora.EsError = False
                    DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
                End If

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
            BE_Bitacora.Descripcion = "Error al generar orden de producción: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al generar orden de producción: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado

    End Function

    Public Function ActualizarEstado(ByVal op As BE.BE_OrdenProduccion) As Boolean

        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim id_op As SqlParameter = New SqlParameter("@id_op", op.ID)
            Dim id_estado As SqlParameter = New SqlParameter("@id_estado", op.Estado.ID)
            Dim id_fechaFin As SqlParameter = New SqlParameter("@fechaFin", op.FechaFin)

            parametros.Add(id_op)
            parametros.Add(id_estado)
            parametros.Add(id_fechaFin)

            resultado = (_dalAcceso.Escribir("OrdenProduccion_ActualizarEstado", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Estado de orden de producción actualizado exitosamente."
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
            BE_Bitacora.Descripcion = "Error al actualizar estado de orden de producción: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actaulizar estado de orden de producción: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado

    End Function

End Class
