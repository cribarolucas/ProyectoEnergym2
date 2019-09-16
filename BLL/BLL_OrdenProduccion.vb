Public Class BLL_OrdenProduccion
    Private _dalOrdenProduccion As DAL.DAL_OrdenProduccion = New DAL.DAL_OrdenProduccion
    Public Function ListarPorFechaEstado(ByVal estado As BE.BE_Estado, ByVal fIni As DateTime, ByVal fFin As DateTime) As List(Of BE.BE_OrdenProduccion)
        Return _dalOrdenProduccion.ListarPorFechaEstado(estado, fIni, fFin)
    End Function
    Public Function GenerarOrdenProduccion(ByVal op As BE.BE_OrdenProduccion) As Boolean
        Return _dalOrdenProduccion.GenerarOrdenProduccion(op)
    End Function
    Public Function ActualizarEstado(ByVal op As BE.BE_OrdenProduccion) As Boolean
        Dim resultado As Boolean = True
        Dim BLL_Stock As BLL_Stock = New BLL_Stock
        If _dalOrdenProduccion.ActualizarEstado(op) Then
            'Si la orden de producción fue aprobada, tengo que actualizar el stock
            'por cada uno de los productos
            If op.Estado.ID = 4 Then
                For Each detalle In op.Detalles
                    detalle.Producto = BLL_Stock.VerificarPorProducto(detalle.Producto)
                    detalle.Producto.Stock.Cantidad += detalle.Cantidad
                    If Not BLL_Stock.ActualizarStock(detalle.Producto.Stock) Then
                        resultado = False
                    End If
                Next
            End If
        End If
        Return resultado
    End Function
End Class
