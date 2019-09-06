Public Class BE_ReporteVentas
    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property
    Private _porcentaje As Decimal
    Public Property Porcentaje() As Decimal
        Get
            Return _porcentaje
        End Get
        Set(ByVal value As Decimal)
            _porcentaje = value
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
