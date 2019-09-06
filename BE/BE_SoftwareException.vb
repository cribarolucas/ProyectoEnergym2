Public Class BE_SoftwareException
    Inherits Exception

    Public Sub New(codigo As String)
        MyBase.New()
        Me.Codigo = codigo
    End Sub

    Public Sub New(message As String, codigo As String)
        MyBase.New(message)
        Me.Codigo = codigo
    End Sub

    Public Sub New(message As String, innerException As Exception, codigo As String)
        MyBase.New(Message, innerException)
        Me.Codigo = codigo
    End Sub

    Private _cod As String
    Public Property Codigo() As String
        Get
            Return _cod
        End Get
        Set(ByVal value As String)
            _cod = value
        End Set
    End Property

    Private _desc As String
    Public Property Descripcion() As String
        Get
            Return _desc
        End Get
        Set(ByVal value As String)
            _desc = value
        End Set
    End Property

End Class
