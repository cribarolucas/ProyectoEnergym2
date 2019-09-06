Public Class BE_Usuario

    Public Sub New()
        _perfil = New List(Of BE_Permiso)
        _idioma = New BE_Idioma
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

    Private _nomUsu As String
    Public Property NombreDeUsuario() As String
        Get
            Return _nomUsu
        End Get
        Set(ByVal value As String)
            _nomUsu = value
        End Set
    End Property

    Private _perfil As List(Of BE.BE_Permiso)
    Public Property Perfil() As List(Of BE.BE_Permiso)
        Get
            Return _perfil
        End Get
        Set(ByVal value As List(Of BE.BE_Permiso))
            _perfil = value
        End Set
    End Property

    Private _bloqueado As Boolean
    Public Property Bloqueado() As Boolean
        Get
            Return _bloqueado
        End Get
        Set(ByVal value As Boolean)
            _bloqueado = value
        End Set
    End Property

    Private _clave As String
    Public Property Clave() As String
        Get
            Return _clave
        End Get
        Set(ByVal value As String)
            _clave = value
        End Set
    End Property

    Private _idioma As BE_Idioma
    Public Property Idioma() As BE_Idioma
        Get
            Return _idioma
        End Get
        Set(ByVal value As BE_Idioma)
            _idioma = value
        End Set
    End Property

    Private _intFall As Short
    Public Property CantidadDeIntentosFallidos() As Short
        Get
            Return _intFall
        End Get
        Set(ByVal value As Short)
            _intFall = value
        End Set
    End Property
    Private _activo As Boolean
    Public Property Activo() As Boolean
        Get
            Return _activo
        End Get
        Set(ByVal value As Boolean)
            _activo = value
        End Set
    End Property


    Private _dvh As String
    Public Property DVH() As String
        Get
            Return _dvh
        End Get
        Set(ByVal value As String)
            _dvh = value
        End Set
    End Property

End Class
