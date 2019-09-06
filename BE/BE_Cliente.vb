Public Class BE_Cliente
    Inherits BE_Usuario

    Public Sub New()
        _iva = New BE_IVA
        _provincia = New BE_Provincia
        _localidad = New BE_Localidad
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
    Private _apellido As String
    Public Property Apellido() As String
        Get
            Return _apellido
        End Get
        Set(ByVal value As String)
            _apellido = value
        End Set
    End Property
    Private _dni As Integer
    Public Property DNI() As Integer
        Get
            Return _dni
        End Get
        Set(ByVal value As Integer)
            _dni = value
        End Set
    End Property
    Private _cuit As Long
    Public Property CUIT() As Long
        Get
            Return _cuit
        End Get
        Set(ByVal value As Long)
            _cuit = value
        End Set
    End Property

    Private _calle As String
    Public Property Calle() As String
        Get
            Return _calle
        End Get
        Set(ByVal value As String)
            _calle = value
        End Set
    End Property
    Private _altura As Short
    Public Property Altura() As Short
        Get
            Return _altura
        End Get
        Set(ByVal value As Short)
            _altura = value
        End Set
    End Property
    Private _piso As Short
    Public Property Piso() As Short
        Get
            Return _piso
        End Get
        Set(ByVal value As Short)
            _piso = value
        End Set
    End Property
    Private _departamento As String
    Public Property Departamento() As String
        Get
            Return _departamento
        End Get
        Set(ByVal value As String)
            _departamento = value
        End Set
    End Property
    Private _codigoPostal As Short
    Public Property CodigoPostal() As Short
        Get
            Return _codigoPostal
        End Get
        Set(ByVal value As Short)
            _codigoPostal = value
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
    Private _iva As BE_IVA
    Public Property IVA() As BE_IVA
        Get
            Return _iva
        End Get
        Set(ByVal value As BE_IVA)
            _iva = value
        End Set
    End Property

    Private _provincia As BE_Provincia
    Public Property Provincia() As BE_Provincia
        Get
            Return _provincia
        End Get
        Set(ByVal value As BE_Provincia)
            _provincia = value
        End Set
    End Property
    Private _localidad As BE_Localidad
    Public Property Localidad() As BE_Localidad
        Get
            Return _localidad
        End Get
        Set(ByVal value As BE_Localidad)
            _localidad = value
        End Set
    End Property


End Class
