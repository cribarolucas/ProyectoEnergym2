Public Class BE_Factura

    Public Sub New()
        _cliente = New BE_Cliente
        _tipoFactura = New BE_TipoFactura
        _iva = New BE_IVA
        _pedido = New BE_Pedido
        _detallesFactura = New List(Of BE_FacturaDetalle)
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
    Private _tipoFactura As BE_TipoFactura
    Public Property TipoFactura() As BE_TipoFactura
        Get
            Return _tipoFactura
        End Get
        Set(ByVal value As BE_TipoFactura)
            _tipoFactura = value
        End Set
    End Property

    Private _iva As BE_IVA
    Public Property IVA() As BE_IVA
        Get
            Return _iva
        End Get
        Set(ByVal value As BE_IVA)
            _iva = value
        End Set
    End Property

    Private _detallesFactura As List(Of BE_FacturaDetalle)
    Public Property DetallesFactura() As List(Of BE_FacturaDetalle)
        Get
            Return _detallesFactura
        End Get
        Set(ByVal value As List(Of BE_FacturaDetalle))
            _detallesFactura = value
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

    Private _precioTotal As Decimal
    Public Property PrecioTotal() As Decimal
        Get
            Return _precioTotal
        End Get
        Set(ByVal value As Decimal)
            _precioTotal = value
        End Set
    End Property
    Private _dvh As String
    Public Property DVH() As String
        Get
            Return _dvh
        End Get
        Set(ByVal value As String)
            _dvh = value
        End Set
    End Property
End Class
