
Public Class BLL_PedidoDetalle
    Private _dalPedidoDetalle As DAL.DAL_PedidoDetalle = New DAL.DAL_PedidoDetalle

    Public Function GenerarPedidoDetalle(ByVal pedido As BE.BE_Pedido) As Boolean
        Return _dalPedidoDetalle.GenerarPedidoDetalle(pedido)
    End Function
    Public Function ListarPorPedido(ByVal Pedido As BE.BE_Pedido) As List(Of BE.BE_PedidoDetalle)
        Return _dalPedidoDetalle.ListarPorPedido(Pedido)
    End Function
End Class
