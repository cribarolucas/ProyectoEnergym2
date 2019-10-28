Public Class BLL_FacturaDetalle
    Private _dalFacturaDetalle As DAL.DAL_FacturaDetalle = New DAL.DAL_FacturaDetalle

    Public Function GenerarFacturaDetalle(ByVal factura As BE.BE_Factura) As Boolean
        Dim resultado As Boolean = True
        Dim BLL_Stock As BLL.BLL_Stock = New BLL_Stock
        If _dalFacturaDetalle.GenerarFacturaDetalle(factura) Then
            For Each df As BE.BE_FacturaDetalle In factura.DetallesFactura
                Dim BE_Stock As BE.BE_Stock = New BE.BE_Stock
                BE_Stock.Cantidad = df.Producto.Stock.Cantidad - df.Cantidad
                BE_Stock.ID = df.Producto.Stock.ID
                If Not BLL_Stock.ActualizarStock(BE_Stock) Then
                    resultado = False
                End If
            Next
        Else
            resultado = False
        End If
        Return resultado
    End Function
    Public Function ListarPorFactura(ByVal factura As BE.BE_Factura) As List(Of BE.BE_FacturaDetalle)
        Return _dalFacturaDetalle.ListarPorFactura(factura)
    End Function
End Class
