Public Class BE_OrdenProduccion
    Public Sub New()
        _estado = New BE_Estado
        _detalles = New List(Of BE.BE_OrdenProduccionDetalle)
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
    Private _estado As BE.BE_Estado
    Public Property Estado() As BE.BE_Estado
        Get
            Return _estado
        End Get
        Set(ByVal value As BE.BE_Estado)
            _estado = value
        End Set
    End Property
    Private _fechaIni As DateTime
    Public Property FechaInicio() As String
        Get
            Return _fechaIni
        End Get
        Set(ByVal value As String)
            _fechaIni = value
        End Set
    End Property
    Private _fechaFin As DateTime
    Public Property FechaFin() As DateTime
        Get
            Return _fechaFin
        End Get
        Set(ByVal value As DateTime)
            _fechaFin = value
        End Set
    End Property


    Private _detalles As List(Of BE.BE_OrdenProduccionDetalle)
    Public Property Detalles() As List(Of BE.BE_OrdenProduccionDetalle)
        Get
            Return _detalles
        End Get
        Set(ByVal value As List(Of BE.BE_OrdenProduccionDetalle))
            _detalles = value
        End Set
    End Property

End Class
