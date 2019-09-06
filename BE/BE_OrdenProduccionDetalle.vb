Public Class BE_OrdenProduccionDetalle
    Public Sub New()
        _producto = New BE_Producto
    End Sub
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
End Class
