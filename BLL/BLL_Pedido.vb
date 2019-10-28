Public Class BLL_Pedido

    Private _dalPedido As DAL.DAL_Pedido = New DAL.DAL_Pedido
    Public Function GenerarPedido(ByRef pedido As BE.BE_Pedido) As Boolean
        For Each detalle As BE.BE_PedidoDetalle In pedido.DetallesPedido
            pedido.PrecioTotal += detalle.Cantidad * detalle.PrecioUnitario
        Next
        pedido.Estado.ID = 1 'Pendiente de aprobación
        Return _dalPedido.GenerarPedido(pedido)
    End Function

    Public Function ListarPorFechaEstado(ByVal fdesde As DateTime, ByVal fhasta As DateTime,
                                         ByVal estado As BE.BE_Estado) As List(Of BE.BE_Pedido)
        Return _dalPedido.ListarPorFechaEstado(fdesde, fhasta, estado)
    End Function

    Public Function ActualizarEstadoPedido(ByVal pedido As BE.BE_Pedido) As Boolean
        Dim resultado As Boolean = True
        Dim BE_OrdenProduccion As BE.BE_OrdenProduccion
        Dim BE_OrdenProduccionDetalle As BE.BE_OrdenProduccionDetalle
        Dim BLL_OrdenProduccion As BLL.BLL_OrdenProduccion = New BLL_OrdenProduccion

        If pedido.Estado.ID = 2 Then
            Dim BE_Factura As BE.BE_Factura = New BE.BE_Factura
            Dim BLL_Factura As BLL.BLL_Factura = New BLL_Factura
            Dim BLL_FacturaDetalle As BLL.BLL_FacturaDetalle = New BLL_FacturaDetalle
            Dim BLL_Stock As BLL.BLL_Stock = New BLL_Stock
            BE_Factura.Fecha = DateTime.Now
            BE_Factura.Cliente = pedido.Cliente
            BE_Factura.Pedido = pedido

            For Each detalle As BE.BE_PedidoDetalle In pedido.DetallesPedido
                Dim BE_FacturaDetalle As BE.BE_FacturaDetalle = New BE.BE_FacturaDetalle
                BE_FacturaDetalle.Producto = BLL_Stock.VerificarPorProducto(detalle.Producto)
                BE_FacturaDetalle.Cantidad = detalle.Cantidad
                'Si la cantidad disponible es menor a la cantidad solicitada, genero una orden de producción
                If BE_FacturaDetalle.Producto.Stock.Cantidad < BE_FacturaDetalle.Cantidad Then
                    BE_OrdenProduccion = New BE.BE_OrdenProduccion
                    BE_OrdenProduccionDetalle = New BE.BE_OrdenProduccionDetalle
                    BE_OrdenProduccion.Estado.ID = 1 'Pendiente de aprobación
                    BE_OrdenProduccionDetalle.Cantidad = BE_FacturaDetalle.Cantidad
                    BE_OrdenProduccionDetalle.Producto = BE_FacturaDetalle.Producto
                    BE_OrdenProduccion.Detalles.Add(BE_OrdenProduccionDetalle)
                    BE_OrdenProduccion.FechaInicio = DateTime.Now
                    BE_OrdenProduccion.FechaFin = Nothing
                    If Not BLL_OrdenProduccion.GenerarOrdenProduccion(BE_OrdenProduccion) Then
                        resultado = False
                        Exit For
                    End If
                End If
                BE_FacturaDetalle.PrecioUnitario = detalle.PrecioUnitario
                BE_FacturaDetalle.PrecioSubtotal = detalle.PrecioSubtotal
                BE_Factura.DetallesFactura.Add(BE_FacturaDetalle)
            Next

            If resultado Then
                If Not _dalPedido.ActualizarEstadoPedido(pedido) OrElse
                    Not BLL_Factura.GenerarFactura(BE_Factura) OrElse
                    Not BLL_FacturaDetalle.GenerarFacturaDetalle(BE_Factura) Then
                    resultado = False
                End If
            End If

        ElseIf pedido.Estado.ID = 3 Then
            If _dalPedido.ActualizarEstadoPedido(pedido) Then
                resultado = True
            End If
        End If

        Return resultado

    End Function

    Public Function ListarAprobados() As List(Of BE.BE_Pedido)
        Return _dalPedido.ListarAprobados()
    End Function
End Class
