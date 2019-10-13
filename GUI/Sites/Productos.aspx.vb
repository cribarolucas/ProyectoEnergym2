Imports System.IO
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports System.Configuration
Public Class Productos
    Inherits System.Web.UI.Page

    Private _usuarioConectado As BE.BE_Usuario
    Private _bllProducto As BLL.BLL_Producto = New BLL.BLL_Producto
    Private _bllStock As BLL.BLL_Stock = New BLL.BLL_Stock
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Me.BindData()
            Me.TraducirControles()
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
                BE_Permiso.ID = 23 'Le paso el código del permiso que corresponde a la visualización de productos
                mp.VerificarAutorizacion(BE_Permiso)
            End If
            mp.Traducir(mp)
            mp.Traducir(Me)
            Dim BLL_CarritoCompras As BLL.BLL_CarritoCompras = New BLL.BLL_CarritoCompras
            If IsNothing(Session("CarritoCompras")) Then
                L_ITEMS_V.Text = "0"
            Else
                'L_ITEMS_V.Text = " " + BLL_CarritoCompras.ObtenerTotalItems(DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras).CarritoItems).ToString
            End If
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
        Session.Add("Productos", _bllProducto.ListarProductos())
        dlProductos.DataSource = Nothing
        dlProductos.DataSource = DirectCast(Session("Productos"), List(Of BE.BE_Producto))
        dlProductos.DataBind()
    End Sub

    Protected Sub dlProductos_ItemCommand(source As Object, e As DataListCommandEventArgs) Handles dlProductos.ItemCommand

        If (e.CommandName = "AddShopCart") Then
            dlProductos.SelectedIndex = e.Item.ItemIndex

            Dim BE_CarritoItem As BE.BE_CarritoItem = New BE.BE_CarritoItem
            Dim BE_CarritoCompras As BE.BE_CarritoCompras = Session("CarritoCompras")
            Dim BLL_CarritoCompras As BLL.BLL_CarritoCompras = New BLL.BLL_CarritoCompras
            If IsNothing(BE_CarritoCompras) Then
                BE_CarritoCompras = New BE.BE_CarritoCompras
            End If

            BE_CarritoItem.Producto.ID = Convert.ToInt32(DirectCast(dlProductos.SelectedItem.FindControl("lblID"), Label).Text)
            BE_CarritoItem.Producto = DirectCast(Session("Productos"), List(Of BE.BE_Producto)).Find(Function(x) x.ID = BE_CarritoItem.Producto.ID)

            'BLL_CarritoCompras.ActualizarCantidadItem(BE_CarritoCompras, BE_CarritoItem)

            'L_ITEMS_V.Text = " " + BLL_CarritoCompras.ObtenerTotalItems(BE_CarritoCompras.CarritoItems).ToString()

            Session("CarritoCompras") = Nothing
            Session.Add("CarritoCompras", BE_CarritoCompras)
        ElseIf (e.CommandName = "SeeDetails") Then
            dlProductos.SelectedIndex = e.Item.ItemIndex
            Dim BE_Producto As BE.BE_Producto = New BE.BE_Producto
            BE_Producto.ID = Convert.ToInt32(DirectCast(dlProductos.SelectedItem.FindControl("lblID"), Label).Text)
            BE_Producto = DirectCast(Session("Productos"), List(Of BE.BE_Producto)).Find(Function(x) x.ID = BE_Producto.ID)
            Session.Add("ProductoDetalle", BE_Producto)
            Response.Redirect("~/Sites/Negocio/Producto_Detalle.aspx")
        End If

    End Sub
    Protected Sub dlProductos_ItemDataBound(sender As Object, e As DataListItemEventArgs)

        If e.Item.ItemType = ListItemType.Item OrElse
           e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim culture As CultureInfo = New CultureInfo("es-AR")
            Dim lblPrecio As Label = DirectCast(e.Item.FindControl("txtPrecio"), Label)
            Dim Precio As Decimal = Convert.ToDecimal(lblPrecio.Text)
            lblPrecio.Text = Precio.ToString("C", culture)

        End If

    End Sub
    Private Sub TraducirControles()
        Dim culture As CultureInfo = New CultureInfo("es-AR")
        For Each item As DataListItem In dlProductos.Items
            DirectCast(item.FindControl("B_ADD_CART"), Button).Text = _segIdioma.TraducirControl("B_ADD_CART", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
            DirectCast(item.FindControl("B_VER_DET"), Button).Text = _segIdioma.TraducirControl("B_VER_DET", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
            DirectCast(item.FindControl("L_PRECIO"), Label).Text = _segIdioma.TraducirControl("L_PRECIO", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
            DirectCast(item.FindControl("txtPrecio"), Label).Text = DirectCast(item.FindControl("txtPrecio"), Label).Text.ToString(culture)
        Next
    End Sub
    Protected Sub B_SEE_CART_Click(sender As Object, e As EventArgs) Handles B_SEE_CART.Click
        If Not IsNothing(Session("CarritoCompras")) Then
            If DirectCast(Session("CarritoCompras"), BE.BE_CarritoCompras).CarritoItems.Count > 0 Then
                Response.Redirect("~/Sites/Negocio/CarritoCompras.aspx")
            End If
        End If
    End Sub
End Class