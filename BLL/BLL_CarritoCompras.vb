Public Class BLL_CarritoCompras
    Public Sub CalcularTotal(ByRef carritoCompras As BE.BE_CarritoCompras)
        Dim total As Decimal
        For Each item As BE.BE_CarritoItem In carritoCompras.CarritoItems
            total += item.PrecioSubtotal
        Next
        carritoCompras.Total = total
    End Sub

    Public Sub CalcularSubotal(ByRef carritoComrpas As BE.BE_CarritoCompras)
        For i = 0 To carritoComrpas.CarritoItems.Count - 1
            carritoComrpas.CarritoItems.Item(i).PrecioSubtotal =
            carritoComrpas.CarritoItems.Item(i).Cantidad * carritoComrpas.CarritoItems(i).Producto.Precio
        Next
    End Sub

    Public Sub ActualizarCantidadItem(ByRef carritoCompras As BE.BE_CarritoCompras, ByVal carritoItem As BE.BE_CarritoItem)
        If carritoCompras.CarritoItems.Count = 0 Then
            carritoItem.Cantidad = 1
            carritoCompras.CarritoItems.Add(carritoItem)
            Me.CalcularSubotal(carritoCompras)
        Else
            Dim BE_CarritoItem As BE.BE_CarritoItem =
                carritoCompras.CarritoItems.Find(Function(x) x.Producto.ID = carritoItem.Producto.ID)
            If IsNothing(BE_CarritoItem) Then
                carritoItem.Cantidad = 1
                carritoCompras.CarritoItems.Add(carritoItem)
            Else
                BE_CarritoItem.Cantidad += 1
            End If
            Me.CalcularSubotal(carritoCompras)
        End If
        Me.CalcularTotal(carritoCompras)
    End Sub

    Public Sub EliminarItem(ByRef carritoCompras As BE.BE_CarritoCompras, ByVal carritoItem As BE.BE_CarritoItem)
        carritoCompras.CarritoItems.Remove(carritoCompras.CarritoItems.Find(Function(x) x.Producto.ID = carritoItem.Producto.ID))
    End Sub

    Public Function ObtenerTotalItems(ByVal carritoItems As List(Of BE.BE_CarritoItem)) As Integer
        Dim totalItems As Integer = 0
        For Each item As BE.BE_CarritoItem In carritoItems
            totalItems += item.Cantidad
        Next
        Return totalItems
    End Function

End Class
