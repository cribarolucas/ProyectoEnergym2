Public Class BE_Pedido

    Public Sub New()
        _cliente = New BE_Cliente
        _estado = New BE_Estado
        _detallesPedido = New List(Of BE_PedidoDetalle)
    End Sub

    Private _id As Integer
    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property
    Private _fecha As DateTime
    Public Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
        Set(ByVal value As DateTime)
            _fecha = value
        End Set
    End Property
    Private _cliente As BE_Cliente
    Public Property Cliente() As BE_Cliente
        Get
            Return _cliente
        End Get
        Set(ByVal value As BE_Cliente)
            _cliente = value
        End Set
    End Property

    Private _precioTotal As Decimal
    Public Property PrecioTotal() As Decimal
        Get
            Return _precioTotal
        End Get
        Set(ByVal value As Decimal)
            _precioTotal = value
        End Set
    End Property

    Private _estado As BE_Estado
    Public Property Estado() As BE_Estado
        Get
            Return _estado
        End Get
        Set(ByVal value As BE_Estado)
            _estado = value
        End Set
    End Property

    Private _detallesPedido As List(Of BE_PedidoDetalle)
    Public Property DetallesPedido() As List(Of BE_PedidoDetalle)
        Get
            Return _detallesPedido
        End Get
        Set(ByVal value As List(Of BE_PedidoDetalle))
            _detallesPedido = value
        End Set
    End Property


End Class
