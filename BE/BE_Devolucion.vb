Public Class BE_Devolucion

    Public Sub New()
        _cliente = New BE_Cliente
        _factura = New BE_Factura
        _productos = New List(Of BE.BE_Producto)
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
    Private _descripcion As String
    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property
    Private _fecha As DateTime
    Public Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
        Set(ByVal value As DateTime)
            _fecha = value
        End Set
    End Property
    Private _cliente As BE_Cliente
    Public Property Cliente() As BE_Cliente
        Get
            Return _cliente
        End Get
        Set(ByVal value As BE_Cliente)
            _cliente = value
        End Set
    End Property
    Private _factura As BE_Factura
    Public Property Factura() As BE_Factura
        Get
            Return _factura
        End Get
        Set(ByVal value As BE_Factura)
            _factura = value
        End Set
    End Property
    Private _productos As List(Of BE.BE_Producto)
    Public Property Productos() As List(Of BE.BE_Producto)
        Get
            Return _productos
        End Get
        Set(ByVal value As List(Of BE.BE_Producto))
            _productos = value
        End Set
    End Property


End Class
