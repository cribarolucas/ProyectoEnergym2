Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_PedidoDetalle

    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL.DAL_Acceso

    Public Function GenerarPedidoDetalle(ByVal pedido As BE.BE_Pedido) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            For Each detalle As BE.BE_PedidoDetalle In pedido.DetallesPedido

                Dim id_pedido As SqlParameter = New SqlParameter("@id_pedido", pedido.ID)
                Dim id_producto As SqlParameter = New SqlParameter("@id_producto", detalle.Producto.ID)
                Dim cant As SqlParameter = New SqlParameter("@cant", detalle.Cantidad)
                Dim precioUnitario As SqlParameter = New SqlParameter("@precioUnitario", detalle.PrecioUnitario)
                Dim precioSubtotal As SqlParameter = New SqlParameter("@precioSubtotal", detalle.PrecioSubtotal)

                parametros.Add(id_pedido)
                parametros.Add(id_producto)
                parametros.Add(cant)
                parametros.Add(precioUnitario)
                parametros.Add(precioSubtotal)

                resultado = (_dalAcceso.Escribir("PedidoDetalle_Generar", parametros) > 0)

                If resultado Then
                    BE_Bitacora.FechaHora = DateTime.Now
                    BE_Bitacora.Usuario = _usuarioActual
                    BE_Bitacora.Descripcion = "Detalle de pedido generado exitosamente."
                    BE_Bitacora.EsError = False
                    DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
                End If

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
            BE_Bitacora.Descripcion = "Error al generar detalle de pedido: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al generar detalle de pedido: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado

    End Function

    Public Function ListarPorPedido(ByVal pedido As BE.BE_Pedido) As List(Of BE.BE_PedidoDetalle)
        Dim detalles As List(Of BE.BE_PedidoDetalle) = New List(Of BE.BE_PedidoDetalle)
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim detalle As BE.BE_PedidoDetalle

        Try
            Dim id_pedido As SqlParameter = New SqlParameter("@id_pedido", pedido.ID)

            parametros.Add(id_pedido)

            Dim tabla As DataTable = _dalAcceso.Leer("PedidoDetalle_ListarPorPedido", parametros)

            For Each registro As DataRow In tabla.Rows

                detalle = New BE.BE_PedidoDetalle
                detalle.ID = Convert.ToInt32(registro("ID"))
                detalle.Producto.ID = Convert.ToInt32(registro("ID_Producto"))
                detalle.Producto.Nombre = registro("NombreProducto")
                detalle.Cantidad = Convert.ToInt32(registro("Cantidad"))
                detalle.PrecioUnitario = Convert.ToDecimal(registro("PrecioUnitario"))
                detalle.PrecioSubtotal = Convert.ToDecimal(registro("PrecioSubtotal"))

                detalles.Add(detalle)

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
            BE_Bitacora.Descripcion = "Error al listar detalles por pedido: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar detalles por pedido: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return detalles

    End Function

End Class
