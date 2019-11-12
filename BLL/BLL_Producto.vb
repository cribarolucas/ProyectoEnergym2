Imports System.Drawing
Public Class BLL_Producto
    Private _dalProducto As DAL.DAL_Producto = New DAL.DAL_Producto
    Public Function ListarProductos() As List(Of BE.BE_Producto)
        Return _dalProducto.ListarProductos()
    End Function

    Public Function MuscuMin() As Decimal
        Dim productos As New List(Of BE.BE_Producto)
        productos = _dalProducto.ListarMuscuMin
        Dim prod As New BE.BE_Producto
        For Each p As BE.BE_Producto In productos
            prod.Precio = p.Precio
        Next
        Return prod.Precio
    End Function

    Public Function CardioMin() As Decimal
        Dim productos As New List(Of BE.BE_Producto)
        productos = _dalProducto.ListarCardioMin
        Dim prod As New BE.BE_Producto
        For Each p As BE.BE_Producto In productos
            prod.Precio = p.Precio
        Next
        Return prod.Precio
    End Function


    Public Function ListarMusculacion() As List(Of BE.BE_Producto)
        Return _dalProducto.ListarMusculacion()
    End Function
    Public Function ListarCardio() As List(Of BE.BE_Producto)
        Return _dalProducto.ListarCardio()
    End Function

    Public Function AgregarProducto(ByRef producto As BE.BE_Producto) As Boolean
        Return _dalProducto.AgregarProducto(producto)
    End Function
    Public Function ModificarProducto(ByVal producto As BE.BE_Producto) As Boolean
        Return _dalProducto.ModificarProducto(producto)
    End Function
    Public Function EliminarProducto(ByVal producto As BE.BE_Producto) As Boolean
        Return _dalProducto.EliminarProducto(producto)
    End Function
    Public Function ListarMasVendidos(ByVal fDesde As DateTime, ByVal fHasta As DateTime) As List(Of BE.BE_Producto)
        Return _dalProducto.ListarMasVendidos(fDesde, fHasta)
    End Function

    Public Function ListarStock() As List(Of BE.BE_Producto)
        Return _dalProducto.ListarStock()
    End Function

End Class
