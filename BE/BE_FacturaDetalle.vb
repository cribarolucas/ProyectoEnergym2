Public Class BE_FacturaDetalle
    Public Sub New()
        _producto = New BE_Producto
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
    Private _producto As BE_Producto
    Public Property Producto() As BE_Producto
        Get
            Return _producto
        End Get
        Set(ByVal value As BE_Producto)
            _producto = value
        End Set
    End Property
    Private _cantidad As Integer
    Public Property Cantidad() As Integer
        Get
            Return _cantidad
        End Get
        Set(ByVal value As Integer)
            _cantidad = value
        End Set
    End Property
    Private _precioUnitario As Decimal
    Public Property PrecioUnitario() As Decimal
        Get
            Return _precioUnitario
        End Get
        Set(ByVal value As Decimal)
            _precioUnitario = value
        End Set
    End Property
    Private _precioSubtotal As Decimal
    Public Property PrecioSubtotal() As Decimal
        Get
            Return _precioSubtotal
        End Get
        Set(ByVal value As Decimal)
            _precioSubtotal = value
        End Set
    End Property


End Class
