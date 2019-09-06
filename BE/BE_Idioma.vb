Public Class BE_Idioma

    Public Sub New()
        _leyendas = New List(Of BE_Leyenda)
    End Sub

    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Private _codigo As String
    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Private _leyendas As List(Of BE_Leyenda)
    Public Property Leyendas() As List(Of BE_Leyenda)
        Get
            Return _leyendas
        End Get
        Set(ByVal value As List(Of BE_Leyenda))
            _leyendas = value
        End Set
    End Property


End Class
