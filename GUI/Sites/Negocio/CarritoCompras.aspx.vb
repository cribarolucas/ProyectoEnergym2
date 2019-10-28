Imports System.IO
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports System.Configuration
Public Class CarritoCompras
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _bllProducto As BLL.BLL_Producto = New BLL.BLL_Producto
    Private _bllStock As BLL.BLL_Stock = New BLL.BLL_Stock
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim BLL_CarritoCompras As BLL.BLL_CarritoCompras = New BLL.BLL_CarritoCompras
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            hfUsuarioConectado.Value = jss.Serialize(_usuarioConectado)
            Dim mp As MasterPage = Me.Master
            Me.BindData()
            Me.TraducirControles()
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
                BE_Permiso.ID = 13 'Le paso el código del permiso que corresponde al carrito de compras
                mp.VerificarAutorizacion(BE_Permiso)
            End If
            mp.Traducir(mp)
            mp.Traducir(Me)
        End If
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim mp As MasterPage = Me.Master
        Dim drp As DropDownList = DirectCast(Page.Master.FindControl("ddlIdiomas"), DropDownList)
        AddHandler mp.ddlIdiomas_SelIdxChanged, AddressOf CambiarIdioma
    End Sub

    Protected Sub CambiarIdioma(sender As Object, e As System.EventArgs)
        Dim mp As MasterPage = Me.Master
        Dim ddl As DropDownList = DirectCast(Page.Master.FindControl("ddlIdiomas"), DropDownList)

        If ddl.SelectedIndex >= 0 Then
            Dim idioma As BE.BE_Idioma = New BE.BE_Idioma
            idioma.Codigo = DirectCast(ddl.SelectedValue, String)
            idioma.Nombre = DirectCast(ddl.Items(ddl.SelectedIndex).Text, String)
            idioma.Leyendas = DirectCast(Application("Idiomas"),  _
                                List(Of BE.BE_Idioma)).Find(Function(x) x.Codigo = idioma.Codigo).Leyendas
            Session.Remove("Idioma_Actual")
            Session.Add("Idioma_Actual", idioma)
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)

            If Not IsNothing(Session("Usuario_Conectado")) Then
                Dim usuario As BE.BE_Usuario = Session("Usuario_Conectado")
                usuario.Idioma = idioma
                Session.Remove("Usuario_Conectado")
                Session.Add("Usuario_Conectado", usuario)
                'Actualizar idioma usuario
                Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
                SEG_Usuario.CambiarIdioma(usuario, idioma)
            End If
            mp.Traducir(Me)
            Me.TraducirControles()
        End If
    End Sub
    Private Sub BindData()
        Dim BLL_CarritoCompras As BLL.BLL_CarritoCompras = New BLL.BLL_CarritoCompras
        gvCarrito.DataSource = Nothing
        If Not IsNothing(Session("CarritoCompras")) Then
            B_UPD_CART.Enabled = True
            B_PEDIDO.Enabled = True
            gvCarrito.DataSource = DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras).CarritoItems
        Else
            B_UPD_CART.Enabled = False
            B_PEDIDO.Enabled = False
        End If
        gvCarrito.DataBind()
    End Sub
    Protected Sub gvCarrito_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim culture As CultureInfo = New CultureInfo("es-AR")
            Dim lblSubtotal As Label = DirectCast(e.Row.FindControl("lblSubtotal"), Label)
            Dim subtotal As Decimal = Convert.ToDecimal(lblSubtotal.Text)
            lblSubtotal.Text = subtotal.ToString("C", culture)

            Dim lblPrecio As Label = DirectCast(e.Row.FindControl("lblPrecio"), Label)
            Dim precio As Decimal = Convert.ToDecimal(lblPrecio.Text)
            lblPrecio.Text = precio.ToString("C", culture)

        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim culture As CultureInfo = New CultureInfo("es-AR")
            Dim lblTotal As Label = DirectCast(e.Row.FindControl("lbltotal"), Label)
            Dim BLL_CarritoCompras As BLL.BLL_CarritoCompras = New BLL.BLL_CarritoCompras
            Dim BE_CarritoCompras As BE.BE_CarritoCompras = DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras)
            lblTotal.Text = BE_CarritoCompras.Total.ToString("C", culture)
        End If
    End Sub
    Protected Sub B_UPD_CART_Click(sender As Object, e As EventArgs) Handles B_UPD_CART.Click
        Me.ActualizarCarrito()
        Me.BindData()
        Me.TraducirControles()
    End Sub

    Private Sub ActualizarCarrito()
        Dim BLL_CarritoCompras As BLL.BLL_CarritoCompras = New BLL.BLL_CarritoCompras
        Dim BE_CarritoCompras As BE.BE_CarritoCompras

        For Each row As GridViewRow In gvCarrito.Rows
            If (row.RowType = DataControlRowType.DataRow) Then
                Dim id As Integer = Convert.ToInt32(DirectCast(row.FindControl("lblID"), Label).Text)
                BE_CarritoCompras = DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras)
                BE_CarritoCompras.CarritoItems.Find(Function(x) x.Producto.ID = id).Cantidad = _
                        Convert.ToInt32(DirectCast(row.FindControl("txtCantidad"), TextBox).Text)
                BLL_CarritoCompras.CalcularSubotal(BE_CarritoCompras)
            End If
        Next
        lblError.Text = ""

        BLL_CarritoCompras.CalcularTotal(BE_CarritoCompras)
    End Sub
    Private Sub AgregarDetallePedido(ByVal producto As BE.BE_Producto, ByVal cantidad As Integer, ByRef pedido As BE.BE_Pedido)
        Dim BLL_Stock As BLL.BLL_Stock = New BLL.BLL_Stock
        'Le paso el ID del stock y la cantidad actual en BD
        Dim p As BE.BE_Producto = BLL_Stock.VerificarPorProducto(producto)
        producto.Stock = p.Stock
        producto.Precio = DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras).CarritoItems. _
                                     Find(Function(x) x.Producto.ID = producto.ID).Producto.Precio()
        Dim detalle As BE.BE_PedidoDetalle = New BE.BE_PedidoDetalle
        detalle.Producto = producto
        detalle.Cantidad = cantidad
        detalle.PrecioUnitario = producto.Precio
        detalle.PrecioSubtotal = DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras).CarritoItems. _
                                            Find(Function(x) x.Producto.ID = producto.ID).PrecioSubtotal
        pedido.DetallesPedido.Add(detalle)
    End Sub
    Protected Sub B_PEDIDO_Click(sender As Object, e As EventArgs) Handles B_PEDIDO.Click
        Me.ActualizarCarrito()
        Dim BLL_Stock As BLL.BLL_Stock = New BLL.BLL_Stock
        Dim BLL_Factura As BLL.BLL_Factura = New BLL.BLL_Factura
        Dim BLL_FacturaDetalle As BLL.BLL_FacturaDetalle = New BLL.BLL_FacturaDetalle
        Dim pedido As BE.BE_Pedido = New BE.BE_Pedido

        lblError.Text = ""
        For Each item As BE.BE_CarritoItem In (DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras).CarritoItems)
            Dim producto As BE.BE_Producto = New BE.BE_Producto
            Dim cantidad As Integer
            producto = item.Producto
            cantidad = item.Cantidad
            Me.AgregarDetallePedido(producto, cantidad, pedido)
        Next
        Me.RealizarPedido(pedido)
    End Sub
    Private Sub RealizarPedido(ByRef pedido As BE.BE_Pedido)
        Dim BE_Cliente As BE.BE_Cliente = New BE.BE_Cliente
        Dim BLL_Cliente As BLL.BLL_Cliente = New BLL.BLL_Cliente
        Dim BLL_Pedido As BLL.BLL_Pedido = New BLL.BLL_Pedido
        Dim BLL_PedidoDetalle As BLL.BLL_PedidoDetalle = New BLL.BLL_PedidoDetalle
        pedido.Fecha = DateTime.Now
        BE_Cliente.ID = _usuarioConectado.ID
        pedido.Cliente = BLL_Cliente.ObtenerDatosCliente(BE_Cliente)

        If BLL_Pedido.GenerarPedido(pedido) AndAlso
            BLL_PedidoDetalle.GenerarPedidoDetalle(pedido) Then
            lblError.Text = _segIdioma.TraducirControl("MS_023", Session("Idioma_Actual"))
            lblError.ForeColor = Drawing.Color.Green
            Session("CarritoCompras") = Nothing
            Me.BindData()
        Else
            lblError.Text = _segIdioma.TraducirControl("ME_068", Session("Idioma_Actual"))
            lblError.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Protected Sub gvCarrito_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim BE_CarritoItem As BE.BE_CarritoItem = New BE.BE_CarritoItem
        Dim BLL_CarritoCompras As BLL.BLL_CarritoCompras = New BLL.BLL_CarritoCompras
        Dim BE_CarritoCompras As BE.BE_CarritoCompras = DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras)
        BE_CarritoItem.Producto.ID = Convert.ToInt32(DirectCast(gvCarrito.Rows.Item(e.RowIndex).FindControl("lblID"), Label).Text)
        BLL_CarritoCompras.EliminarItem(BE_CarritoCompras, BE_CarritoItem)
        BLL_CarritoCompras.CalcularTotal(BE_CarritoCompras)
        Me.BindData()
        Me.TraducirControles()
    End Sub

    Private Sub TraducirControles()
        If gvCarrito.Rows.Count > 0 Then
            gvCarrito.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_NOMBRE", Session("Idioma_Actual"))
            gvCarrito.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_CANT", Session("Idioma_Actual"))
            gvCarrito.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_PRECIO", Session("Idioma_Actual"))
            gvCarrito.FooterRow.Cells(4).Text = _segIdioma.TraducirControl("C_TOTAL", Session("Idioma_Actual"))
            gvCarrito.HeaderRow.Cells(5).Text = _segIdioma.TraducirControl("C_SUBTOTAL", Session("Idioma_Actual"))
            gvCarrito.Caption = _segIdioma.TraducirControl("T_SHOPCART", Session("Idioma_Actual"))

            For Each row As GridViewRow In gvCarrito.Rows
                DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_DELETE", Session("Idioma_Actual"))
            Next
            B_PEDIDO.Text = _segIdioma.TraducirControl(B_PEDIDO.ID, Session("Idioma_Actual"))
            B_UPD_CART.Text = _segIdioma.TraducirControl(B_UPD_CART.ID, Session("Idioma_Actual"))
        Else
            gvCarrito.EmptyDataText = _segIdioma.TraducirControl("L_EDT_CART", Session("Idioma_Actual"))
        End If
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Me.ActualizarCarrito()
        gvCarrito.PageIndex = e.NewPageIndex
        Me.BindData()
    End Sub

End Class