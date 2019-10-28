Public Class BLL_Factura
    Private _dalFactura As DAL.DAL_Factura = New DAL.DAL_Factura
    Public Function GenerarFactura(ByRef factura As BE.BE_Factura) As Boolean
        Dim resultado As Boolean
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New Seguridad.SEG_GestorCifrado
        Dim SEG_GestorIntegridad As Seguridad.SEG_GestorIntegridad = New Seguridad.SEG_GestorIntegridad
        Dim BLL_TipoFactura As BLL.BLL_TipoFactura = New BLL_TipoFactura
        Dim BE_TipoFactura As BE.BE_TipoFactura = New BE.BE_TipoFactura

        For Each detalle As BE.BE_FacturaDetalle In factura.DetallesFactura
            factura.PrecioTotal += detalle.Cantidad * detalle.PrecioUnitario
        Next

        If factura.Cliente.IVA.ID = 1 Then
            BE_TipoFactura.ID = 1
        Else
            BE_TipoFactura.ID = 2
        End If

        factura.IVA.ID = factura.Cliente.IVA.ID
        factura.TipoFactura = BLL_TipoFactura.ListarPorID(BE_TipoFactura)

        Dim precioTotal As String = factura.PrecioTotal.ToString
        precioTotal = precioTotal.Replace(".", ",")

        Dim strArray() As String = factura.Fecha.ToString.Split("/")
        Dim strFecha As String
        For i = 0 To strArray.Length - 1
            If strArray(i).Length = 1 Then
                strArray(i) = "0" + strArray(i).ToString
            End If
            If i = 0 OrElse
               i = 1 Then
                strFecha += strArray(i).ToString + "/"
            Else
                strFecha += strArray(i).ToString
            End If
        Next


        'strFecha = Right("00" & factura.Fecha.Day, 2) & "/" & Right("00" & factura.Fecha.Month, 2) & "/" & factura.Fecha.Year & " " & factura.Fecha.TimeOfDay.ToString

        Dim str As String = strFecha & factura.Cliente.ID.ToString &
                            factura.TipoFactura.ID.ToString &
                            factura.IVA.ID.ToString & factura.Pedido.ID.ToString &
                            precioTotal

        factura.DVH = SEG_GestorCifrado.GetHashMD5(str)

        If _dalFactura.GenerarFactura(factura) Then
            SEG_GestorIntegridad.ActualizarDVV("Factura")
            resultado = True
        End If
        Return resultado

    End Function

    Public Function ListarPorFechas(ByVal fdesde As DateTime, ByVal fhasta As DateTime) As List(Of BE.BE_Factura)
        Return _dalFactura.ListarPorFechas(fdesde, fhasta)
    End Function

    Public Function ListarTodos() As List(Of BE.BE_Factura)
        Return _dalFactura.ListarTodos()
    End Function

    Public Function ObtenerClienteMayorComprador(ByVal fDesde As DateTime, ByVal fHasta As DateTime) As List(Of BE.BE_Factura)
        Return _dalFactura.ObtenerClienteMayorComprador(fDesde, fHasta)
    End Function

    Public Function ListarPorID(ByVal factura As BE.BE_Factura) As List(Of BE.BE_Factura)
        Return _dalFactura.ListarPorID(factura)
    End Function

End Class
