Public Class BE_TipoFactura
    Private _id As Integer
    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property
    Private _tipo As Char
    Public Property Tipo() As Char
        Get
            Return _tipo
        End Get
        Set(ByVal value As Char)
            _tipo = value
        End Set
    End Property

End Class
