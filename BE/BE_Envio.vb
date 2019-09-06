Public Class BE_Envio

    Public Sub New()
        _estado = New BE_Estado
        _pedido = New BE_Pedido
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


    Private _estado As BE_Estado
    Public Property Estado() As BE_Estado
        Get
            Return _estado
        End Get
        Set(ByVal value As BE_Estado)
            _estado = value
        End Set
    End Property

    Private _pedido As BE_Pedido
    Public Property Pedido() As BE_Pedido
        Get
            Return _pedido
        End Get
        Set(ByVal value As BE_Pedido)
            _pedido = value
        End Set
    End Property


End Class
