Imports System.IO
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Configuration
Imports System.Drawing
Imports System.Threading
Imports System.Globalization

Public Class OrdenesProduccion
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _bllOrdenProduccion As BLL.BLL_OrdenProduccion = New BLL.BLL_OrdenProduccion
    Private _bllEstado As BLL.BLL_Estado = New BLL.BLL_Estado
    Private _bllStock As BLL.BLL_Stock = New BLL.BLL_Stock
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Private _ordenes As List(Of BE.BE_OrdenProduccion)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 21 'Le paso el código del permiso que corresponde a la gestión de pedidos
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                Me.BindDataEstadoOrdenes()
                Me.TraducirControles()
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
            Me.TraducirControles()
        End If

    End Sub
    Protected Sub gvOrdenes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvOrdenes.SelectedIndexChanged
        hfOrdenProduccionID.Value = Convert.ToInt32(DirectCast(gvOrdenes.SelectedRow.FindControl("lblID"), Label).Text)
        hfEstadoOrdenID.Value = Convert.ToInt32(DirectCast(gvOrdenes.SelectedRow.FindControl("lblEstadoID"), Label).Text)
        Me.RestablecerColorFilasGrid()
        gvOrdenes.SelectedRow.BackColor = Color.DeepSkyBlue
        lblError.Text = ""
        Dim BE_OrdenProduccion As BE.BE_OrdenProduccion = New BE.BE_OrdenProduccion
        BE_OrdenProduccion.ID = Convert.ToInt32(DirectCast(gvOrdenes.SelectedRow.FindControl("lblID"), Label).Text)
        _ordenes = DirectCast(Session("OrdenesProduccion"), List(Of BE.BE_OrdenProduccion))
        gvDetalles.DataSource = Nothing
        gvDetalles.DataSource = _ordenes.Find(Function(x) x.ID = BE_OrdenProduccion.ID).Detalles
        gvDetalles.DataBind()
        Me.TraducirControles()
    End Sub
    Private Sub RestablecerColorFilasGrid()
        For Each row As GridViewRow In gvOrdenes.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub
    Private Sub BindDataOrdenes(Optional ByVal mensaje As String = Nothing)
        Dim fIni As DateTime = Convert.ToDateTime(hfDateFrom.Value)
        Dim fFin As DateTime = Convert.ToDateTime(hfDateTo.Value)
        Dim estado As BE.BE_Estado = New BE.BE_Estado
        estado.ID = ddlEstado.SelectedValue

        gvOrdenes.DataSource = Nothing
        _ordenes = _bllOrdenProduccion.ListarPorFechaEstado(estado, fIni, fFin)
        gvOrdenes.DataSource = _ordenes

        If IsNothing(mensaje) Then
            If _ordenes.Count = 0 Then
                lblError.Text = _segIdioma.TraducirControl("ME_063", _usuarioConectado.Idioma)
                lblError.ForeColor = Color.Red
            Else
                lblError.Text = ""
                hfEstadoOrdenID.Value = ""
                hfOrdenProduccionID.Value = ""
            End If
        End If

        If estado.ID = 1 Then
            gvOrdenes.Columns(4).Visible = False
        Else
            gvOrdenes.Columns(4).Visible = True
        End If

        gvOrdenes.DataBind()
        Session.Remove("OrdenesProduccion")
        Session.Add("OrdenesProduccion", _ordenes)
        gvDetalles.DataSource = Nothing
        gvDetalles.DataBind()
        Me.TraducirControles()
        hfOrdenProduccionID.Value = ""
    End Sub

    Private Sub BindDataEstadoOrdenes()
        ddlEstado.DataSource = Nothing
        ddlEstado.DataSource = _bllEstado.ListarTodosPorOrdenProduccion()
        ddlEstado.DataValueField = "ID"
        ddlEstado.DataTextField = "Nombre"
        ddlEstado.DataBind()
    End Sub
    Private Sub TraducirControles()
        If gvOrdenes.Rows.Count > 0 Then
            gvOrdenes.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
            gvOrdenes.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_ESTADO", _usuarioConectado.Idioma)
            gvOrdenes.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_FEC_INI", _usuarioConectado.Idioma)
            gvOrdenes.HeaderRow.Cells(5).Text = _segIdioma.TraducirControl("C_FEC_FIN", _usuarioConectado.Idioma)
            gvOrdenes.Caption = _segIdioma.TraducirControl("T_ORD_PROD", _usuarioConectado.Idioma)

            For Each row As GridViewRow In gvOrdenes.Rows
                DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_VER_DET", _usuarioConectado.Idioma)
            Next

        End If
        If gvDetalles.Rows.Count > 0 Then
            gvDetalles.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_PROD", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_CANT", _usuarioConectado.Idioma)
            gvDetalles.Caption = _segIdioma.TraducirControl("T_ODP_DET", _usuarioConectado.Idioma)
        End If
        Thread.CurrentThread.CurrentCulture = New CultureInfo(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo)
    End Sub

    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        hfOrdenProduccionID.Value = ""
        lblError.Text = ""
        Me.RestablecerColorFilasGrid()
        gvOrdenes.DataSource = Nothing
        gvOrdenes.DataBind()
        gvDetalles.DataSource = Nothing
        gvDetalles.DataBind()
    End Sub

    Protected Sub gvDetalles_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)

    End Sub

    Protected Sub B_ODP_LIS_Click(sender As Object, e As EventArgs) Handles B_ODP_LIS.Click
        Me.BindDataOrdenes()
    End Sub

    Protected Sub B_ODP_APR_Click(sender As Object, e As EventArgs) Handles B_ODP_APR.Click
        Dim BE_EstadoPedido As BE.BE_Estado = New BE.BE_Estado
        BE_EstadoPedido.ID = 4
        Me.ActualizarEstadoOrden(BE_EstadoPedido)
    End Sub
    Private Sub ActualizarEstadoOrden(ByVal estado As BE.BE_Estado)
        Dim BLL_Stock As BLL.BLL_Stock = New BLL.BLL_Stock
        Dim BE_OrdenProduccion As BE.BE_OrdenProduccion = New BE.BE_OrdenProduccion
        BE_OrdenProduccion.ID = Convert.ToInt32(DirectCast(gvOrdenes.SelectedRow.FindControl("lblID"), Label).Text)
        BE_OrdenProduccion = DirectCast(Session("OrdenesProduccion"), List(Of BE.BE_OrdenProduccion)).Find(Function(x) x.ID = BE_OrdenProduccion.ID)
        BE_OrdenProduccion.FechaInicio = Convert.ToDateTime(DirectCast(gvOrdenes.SelectedRow.FindControl("lblFechaInicio"), Label).Text)
        BE_OrdenProduccion.FechaFin = DateTime.Now
        BE_OrdenProduccion.Estado = estado

        If _bllOrdenProduccion.ActualizarEstado(BE_OrdenProduccion) Then
            'La orden de producción ha sido actualizada
            lblError.Text = _segIdioma.TraducirControl("MS_025", _usuarioConectado.Idioma)
            lblError.ForeColor = Color.Green
            Me.BindDataOrdenes(lblError.Text)
        Else
            'Error: la orden de producción no ha sido actualizado.
            lblError.Text = _segIdioma.TraducirControl("ME_075", _usuarioConectado.Idioma)
            lblError.ForeColor = Color.Red
        End If

    End Sub

    Protected Sub B_ODP_CAN_Click(sender As Object, e As EventArgs) Handles B_ODP_CAN.Click
        Dim BE_EstadoPedido As BE.BE_Estado = New BE.BE_Estado
        BE_EstadoPedido.ID = 5
        Me.ActualizarEstadoOrden(BE_EstadoPedido)
    End Sub

    Protected Sub gvOrdenes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvOrdenes.PageIndex = e.NewPageIndex
        Me.BindDataOrdenes()
    End Sub

End Class