Public Class BE_Consulta

    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Private _apellido As String
    Public Property Apellido() As String
        Get
            Return _apellido
        End Get
        Set(ByVal value As String)
            _apellido = value
        End Set
    End Property

    Private _email As String
    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    Private _telefono As Long
    Public Property Telefono() As Long
        Get
            Return _telefono
        End Get
        Set(ByVal value As Long)
            _telefono = value
        End Set
    End Property

    Private _consulta As String
    Public Property Consulta() As String
        Get
            Return _consulta
        End Get
        Set(ByVal value As String)
            _consulta = value
        End Set
    End Property

End Class
