Imports System.Object
Imports System.Threading
Imports System.Globalization
Imports System.Web.Script.Serialization

Public Class HistorialVentas
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack() Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 17 'Le paso el código del permiso que corresponde a la visualización del historial de ventas
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                mp.Traducir(mp)
                mp.Traducir(Me)
            End If
        End If
        Thread.CurrentThread.CurrentCulture = New CultureInfo(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo)
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
            Me.TraducirControlesGridFacturas()
            Me.TraducirControlesGridDetalles()
            Thread.CurrentThread.CurrentCulture = New CultureInfo(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo)
        End If
    End Sub
    Private Sub TraducirControlesGridFacturas()
        If gvFacturas.Rows.Count > 0 Then
            gvFacturas.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_FECHA", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_CLIENTE", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_TIPO_FAC", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(5).Text = _segIdioma.TraducirControl("C_IVA", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(6).Text = _segIdioma.TraducirControl("C_PRECIO_T", _usuarioConectado.Idioma)
            gvFacturas.Caption = _segIdioma.TraducirControl("T_FACT", _usuarioConectado.Idioma)

            For Each row As GridViewRow In gvFacturas.Rows
                DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_VER_DET", _usuarioConectado.Idioma)
            Next
        End If
    End Sub

    Private Sub TraducirControlesGridDetalles()
        If gvDetalles.Rows.Count > 0 Then
            gvDetalles.HeaderRow.Cells(0).Text = _segIdioma.TraducirControl("C_PROD", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_CANT", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_PRECIO_U", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_PRECIO_S", _usuarioConectado.Idioma)
            gvDetalles.Caption = _segIdioma.TraducirControl("T_DETALLES", _usuarioConectado.Idioma)
        End If
    End Sub

    Protected Sub OnPaging_gvFacturas(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvFacturas.PageIndex = e.NewPageIndex
        Me.BindGridFacturas()
        gvDetalles.DataSource = Nothing
        gvDetalles.DataBind()
    End Sub

    Protected Sub OnPaging_gvDetalles(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvDetalles.PageIndex = e.NewPageIndex
        Me.BindGridDetalles()
    End Sub
    Private Sub BindGridDetalles()
        Dim BE_Factura As BE.BE_Factura = New BE.BE_Factura
        Dim BLL_FacturaDetalle As BLL.BLL_FacturaDetalle = New BLL.BLL_FacturaDetalle
        BE_Factura.ID = DirectCast(gvFacturas.SelectedRow.FindControl("lblID"), Label).Text

        gvDetalles.DataSource = Nothing
        gvDetalles.DataSource = BLL_FacturaDetalle.ListarPorFactura(BE_Factura)
        gvDetalles.DataBind()
        Me.TraducirControlesGridDetalles()
    End Sub
    Protected Sub B_ACEPTAR_Click(sender As Object, e As EventArgs) Handles B_ACEPTAR.Click
        Me.BindGridFacturas()
    End Sub
    Private Sub BindGridFacturas()
        Dim fDesde As DateTime = hfDateFrom.Value
        Dim fHasta As DateTime = hfDateTo.Value
        Dim BLL_Factura As BLL.BLL_Factura = New BLL.BLL_Factura
        Dim listadoFacturas As List(Of BE.BE_Factura) = New List(Of BE.BE_Factura)
        gvFacturas.DataSource = Nothing
        listadoFacturas = BLL_Factura.ListarPorFechas(fDesde, fHasta)
        If listadoFacturas.Count > 0 Then
            gvFacturas.DataSource = listadoFacturas
            gvFacturas.DataBind()
            Me.TraducirControlesGridFacturas()
            lblError.Text = ""
        Else
            lblError.Text = _segIdioma.TraducirControl("ME_063", _usuarioConectado.Idioma)
            lblError.ForeColor = Drawing.Color.Red
        End If
        Me.RestablecerColorFilasGrid()
    End Sub
    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        gvDetalles.DataSource = Nothing
        gvDetalles.DataBind()
        gvFacturas.DataSource = Nothing
        gvFacturas.DataBind()
        Me.RestablecerColorFilasGrid()
    End Sub

    Private Sub RestablecerColorFilasGrid()
        For Each row As GridViewRow In gvFacturas.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub

    Protected Sub OnSelectedIndexChanged_gvFacturas(ByVal sender As Object, ByVal e As EventArgs) Handles gvFacturas.SelectedIndexChanged
        Me.BindGridDetalles()
        Me.RestablecerColorFilasGrid()
        gvFacturas.SelectedRow.BackColor = Drawing.Color.DeepSkyBlue
    End Sub

    Protected Sub gvFacturas_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim lblPrecioTotal As Label = DirectCast(e.Row.FindControl("lblPrecioTotal"), Label)
            Dim precioTotal As Decimal = Convert.ToDecimal(lblPrecioTotal.Text)
            Dim culture As CultureInfo = New CultureInfo("es-AR")
            lblPrecioTotal.Text = precioTotal.ToString("C", culture)
        End If
    End Sub

    Protected Sub gvDetalles_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim culture As CultureInfo = New CultureInfo("es-AR")
            Dim lblPrecioUnitario As Label = DirectCast(e.Row.FindControl("lblPrecioUnitario"), Label)
            Dim precioUnitario As Decimal = Convert.ToDecimal(lblPrecioUnitario.Text)
            lblPrecioUnitario.Text = precioUnitario.ToString("C", culture)

            Dim lblPrecioSubtotal As Label = DirectCast(e.Row.FindControl("lblPrecioSubtotal"), Label)
            Dim precioSubtotal As Decimal = Convert.ToDecimal(lblPrecioSubtotal.Text)
            lblPrecioSubtotal.Text = precioSubtotal.ToString("C", culture)
        End If
    End Sub
End Class