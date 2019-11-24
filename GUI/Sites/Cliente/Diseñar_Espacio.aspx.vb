
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json
Imports IronPdf

Public Class Diseñar_Espacio
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Private _bllProducto As BLL.BLL_Producto = New BLL.BLL_Producto
    Dim productosCatalogo As New List(Of BE.BE_Producto)


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 25 'Le paso el código del permiso que corresponde al diseño del espacio
            mp.VerificarAutorizacion(BE_Permiso)
            Me.BindData()

            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                mp.Traducir(mp)
                mp.Traducir(Me)
            End If
        End If
    End Sub

    Private Sub BindData()

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
            idioma.Leyendas = DirectCast(Application("Idiomas"),
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
        End If

    End Sub

    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        Response.Redirect("~/Sites/Cliente/Diseñar_Espacio.aspx")
        lblError.Text = ""
        lblMensaje.Text = ""
    End Sub


    Protected Sub asdf_Click(sender As Object, e As EventArgs) Handles B_ACEPTAR.Click
        Dim items As List(Of BE.BE_Item) = JsonConvert.DeserializeObject(Of List(Of BE.BE_Item))(hf_producto.Value)

        Dim productos As New List(Of BE.BE_Producto)

        productosCatalogo = _bllProducto.ListarProductos
        If items.Count <> 0 Then


            For Each i As BE.BE_Item In items
                Dim p As New BE.BE_Producto
                p.ID = i.ID
                p.Cantidad = i.Cantidad
                p.Nombre = productosCatalogo.Find(Function(x) x.ID = p.ID).Nombre
                p.Precio = productosCatalogo.Find(Function(x) x.ID = p.ID).Precio
                p.Stock = productosCatalogo.Find(Function(x) x.ID = p.ID).Stock

                productos.Add(p)

            Next

            Dim BLL_Stock As BLL.BLL_Stock = New BLL.BLL_Stock
            Dim BLL_Factura As BLL.BLL_Factura = New BLL.BLL_Factura
            Dim BLL_FacturaDetalle As BLL.BLL_FacturaDetalle = New BLL.BLL_FacturaDetalle
            Dim pedido As BE.BE_Pedido = New BE.BE_Pedido

            lblError.Text = ""
            lblMensaje.Text = ""

            For Each prod As BE.BE_Producto In productos
                Me.AgregarDetallePedido(prod, prod.Cantidad, pedido)

            Next
            Me.RealizarPedido(pedido)
        Else
            lblError.Text = "Arrastre productos para realizar un pedido."
            lblError.ForeColor = Drawing.Color.Red
        End If
    End Sub


    Private Sub AgregarDetallePedido(ByVal producto As BE.BE_Producto, ByVal cantidad As Integer, ByRef pedido As BE.BE_Pedido)
        Dim BLL_Stock As BLL.BLL_Stock = New BLL.BLL_Stock
        productosCatalogo = _bllProducto.ListarProductos
        'Le paso el ID del stock y la cantidad actual en BD
        Dim p As BE.BE_Producto = BLL_Stock.VerificarPorProducto(producto)
        producto.Stock = p.Stock
        producto.Precio = productosCatalogo.Find(Function(x) x.ID = producto.ID).Precio

        Dim detalle As BE.BE_PedidoDetalle = New BE.BE_PedidoDetalle
        detalle.Producto = producto
        detalle.Cantidad = cantidad
        detalle.PrecioUnitario = producto.Precio
        detalle.PrecioSubtotal = cantidad * producto.Precio
        pedido.DetallesPedido.Add(detalle)
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
            lblMensaje.Text = "El monto total es de " & pedido.PrecioTotal
            lblMensaje.ForeColor = Drawing.Color.Green
        Else
            lblError.Text = _segIdioma.TraducirControl("ME_068", Session("Idioma_Actual"))
            lblError.ForeColor = Drawing.Color.Red
        End If
    End Sub

End Class