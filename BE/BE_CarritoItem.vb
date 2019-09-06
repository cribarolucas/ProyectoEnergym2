Public Class BE_CarritoItem

    Public Sub New()
        _producto = New BE.BE_Producto
    End Sub

    Private _producto As BE.BE_Producto
    Public Property Producto() As BE.BE_Producto
        Get
            Return _producto
        End Get
        Set(value As BE.BE_Producto)
            _producto = value
        End Set
    End Property
    Private _cantidad As Integer
    Public Property Cantidad() As Integer
        Get
            Return _cantidad
        End Get
        Set(value As Integer)
            _cantidad = value
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
