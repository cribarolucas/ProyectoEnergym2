Public Class BE_Producto

    Public Sub New()
        _stock = New BE_Stock
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

    Private _detalle As String
    Public Property Detalle() As String
        Get
            Return _detalle
        End Get
        Set(ByVal value As String)
            _detalle = value
        End Set
    End Property

    Private _precio As Decimal
    Public Property Precio() As Decimal
        Get
            Return _precio
        End Get
        Set(ByVal value As Decimal)
            _precio = value
        End Set
    End Property

    Private _fileName As String
    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property

    Private _filePath As String
    Public Property FilePath() As String
        Get
            Return _filePath
        End Get
        Set(ByVal value As String)
            _filePath = value
        End Set
    End Property

    Private _filePathThumbnail As String
    Public Property FilePathThumbnail() As String
        Get
            Return _filePathThumbnail
        End Get
        Set(ByVal value As String)
            _filePathThumbnail = value
        End Set
    End Property

    Private _alto As Decimal
    Public Property Alto() As Decimal
        Get
            Return _alto
        End Get
        Set(ByVal value As Decimal)
            _alto = value
        End Set
    End Property
    Private _ancho As Decimal

    Public Property Ancho() As Decimal
        Get
            Return _ancho
        End Get
        Set(ByVal value As Decimal)
            _ancho = value
        End Set
    End Property

    Private _largo As Decimal
    Public Property Largo() As Decimal
        Get
            Return _largo
        End Get
        Set(ByVal value As Decimal)
            _largo = value
        End Set
    End Property

    Private _stock As BE_Stock
    Public Property Stock() As BE_Stock
        Get
            Return _stock
        End Get
        Set(ByVal value As BE_Stock)
            _stock = value
        End Set
    End Property

End Class
