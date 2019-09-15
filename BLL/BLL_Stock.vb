Public Class BLL_Stock
    Private _dalStock As DAL.DAL_Stock = New DAL.DAL_Stock
    Public Function EliminarStock(ByVal producto As BE.BE_Producto) As Boolean
        Return _dalStock.EliminarStock(producto)
    End Function
    Public Function VerificarPorProducto(ByVal producto As BE.BE_Producto) As BE.BE_Producto
        Return _dalStock.VerificarPorProducto(producto)
    End Function
    Public Function ActualizarStock(ByVal stock As BE.BE_Stock) As Boolean
        Return _dalStock.ActualizarStock(stock)
    End Function
End Class
