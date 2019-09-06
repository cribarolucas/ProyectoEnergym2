Public Class BE_Provincia

    Public Sub New()
        _localidades = New List(Of BE_Localidad)
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
    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property
    Private _localidades As List(Of BE_Localidad)
    Public Property Localidad() As List(Of BE_Localidad)
        Get
            Return _localidades
        End Get
        Set(ByVal value As List(Of BE_Localidad))
            _localidades = value
        End Set
    End Property

End Class
