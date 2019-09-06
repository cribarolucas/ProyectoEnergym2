Public Class BE_Bitacora

    Private _id As Integer
    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Private _fecHora As DateTime
    Public Property FechaHora() As DateTime
        Get
            Return _fecHora
        End Get
        Set(ByVal value As DateTime)
            _fecHora = value
        End Set
    End Property

    Private _usuario As BE_Usuario
    Public Property Usuario() As BE_Usuario
        Get
            Return _usuario
        End Get
        Set(ByVal value As BE_Usuario)
            _usuario = value
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

    Private _esError As Boolean
    Public Property EsError() As Boolean
        Get
            Return _esError
        End Get
        Set(ByVal value As Boolean)
            _esError = value
        End Set
    End Property

End Class
