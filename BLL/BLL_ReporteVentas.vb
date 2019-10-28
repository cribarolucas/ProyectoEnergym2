Public Class BLL_ReporteVentas

    Public Function VentasProducto(ByVal fDesde As DateTime, ByVal fHasta As DateTime) As List(Of BE.BE_ReporteVentas)
        Dim BLL_Producto As BLL.BLL_Producto = New BLL.BLL_Producto
        Dim BE_ReporteVentas As BE.BE_ReporteVentas
        Dim reportes As List(Of BE.BE_ReporteVentas) = New List(Of BE.BE_ReporteVentas)
        Dim total As Integer

        'Obtener el total de las cantidades vendidas por producto
        Dim productos As List(Of BE.BE_Producto) = BLL_Producto.ListarMasVendidos(fDesde, fHasta)

        For i = 0 To productos.Count - 1
            BE_ReporteVentas = New BE.BE_ReporteVentas
            BE_ReporteVentas.Nombre = productos.Item(i).Nombre
            BE_ReporteVentas.Total = productos.Item(i).Stock.Cantidad 'Guardo la cantidad vendida por producto
            total += productos.Item(i).Stock.Cantidad
            reportes.Add(BE_ReporteVentas)
        Next

        'Calculo el porcentaje
        For i = 0 To reportes.Count - 1
            reportes.Item(i).Porcentaje = Decimal.Round(((reportes.Item(i).Total * 100) / total), 2)
        Next

        Return reportes

    End Function

    Public Function VentasCliente(ByVal fDesde As DateTime, ByVal fHasta As DateTime) As List(Of BE.BE_ReporteVentas)
        Dim BLL_Factura As BLL.BLL_Factura = New BLL.BLL_Factura
        Dim BE_ReporteVentas As BE.BE_ReporteVentas
        Dim reportes As List(Of BE.BE_ReporteVentas) = New List(Of BE.BE_ReporteVentas)
        Dim total As Decimal

        'Obtener el monto total comprado por cada cliente
        Dim facturas As List(Of BE.BE_Factura) = BLL_Factura.ObtenerClienteMayorComprador(fDesde, fHasta)

        For i = 0 To facturas.Count - 1
            BE_ReporteVentas = New BE.BE_ReporteVentas
            BE_ReporteVentas.Nombre = facturas.Item(i).Cliente.Nombre + " " + facturas.Item(i).Cliente.Apellido
            BE_ReporteVentas.Total = facturas.Item(i).PrecioTotal 'Guardo el monto total comprado por cliente
            total += facturas.Item(i).PrecioTotal
            reportes.Add(BE_ReporteVentas)
        Next

        'Calculo el porcentaje
        For i = 0 To reportes.Count - 1
            reportes.Item(i).Porcentaje = Decimal.Round(((reportes.Item(i).Total * 100) / total), 2)
        Next

        Return reportes

    End Function
End Class
