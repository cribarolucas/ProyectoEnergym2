Public Class BE_CarritoCompras

    Public Sub New()
        _carritoItems = New List(Of BE_CarritoItem)
    End Sub

    Private _carritoItems As List(Of BE_CarritoItem)
    Public Property CarritoItems() As List(Of BE_CarritoItem)
        Get
            Return _carritoItems
        End Get
        Set(ByVal value As List(Of BE_CarritoItem))
            _carritoItems = value
        End Set
    End Property

    Private _total As Decimal
    Public Property Total() As Decimal
        Get
            Return _total
        End Get
        Set(ByVal value As Decimal)
            _total = value
        End Set
    End Property


End Class
