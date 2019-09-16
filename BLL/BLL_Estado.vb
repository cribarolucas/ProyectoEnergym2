Public Class BLL_Estado

    Private _dalEstado As DAL.DAL_Estado = New DAL.DAL_Estado

    Public Function ListarTodosPorPedido() As List(Of BE.BE_Estado)
        Return _dalEstado.ListarTodosPorPedido()
    End Function

    Public Function ListarTodosPorOrdenProduccion() As List(Of BE.BE_Estado)
        Return _dalEstado.ListarTodosPorOrdenProduccion()
    End Function
End Class
