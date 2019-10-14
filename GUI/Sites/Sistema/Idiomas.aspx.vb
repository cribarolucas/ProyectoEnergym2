Imports System.Object
Imports System.Globalization
Imports System.Web.Script.Serialization
Public Class Idiomas
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 6 'Le paso el código del permiso que corresponde a la gestión de idiomas
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                Me.BindGridIdiomas()
                Me.TraducirControles()
                mp.Traducir(mp)
                mp.Traducir(Me)
            End If
        End If
    End Sub
    Private Sub BindGridIdiomas()
        Dim SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
        Dim idiomas As List(Of BE.BE_Idioma) = SEG_Idioma.Listar()

        Dim ci() As CultureInfo
        ci = CultureInfo.GetCultures(CultureTypes.SpecificCultures)

        'ddlIdioma.DataSource = Nothing
        'ddlIdioma.DataSource = ci
        'ddlIdioma.SelectedIndex = -1
        'ddlIdioma.DataBind()

        gvIdiomas.DataSource = Nothing
        gvIdiomas.DataSource = idiomas
        gvIdiomas.DataBind()

    End Sub
    Private Sub BindGridLeyendas(ByVal idioma As BE.BE_Idioma)
        Dim SEG_Leyenda As Seguridad.SEG_Leyenda = New Seguridad.SEG_Leyenda
        Dim leyendas As List(Of BE.BE_Leyenda) = SEG_Leyenda.ListarPorIdioma(idioma)

        gvLeyendas.DataSource = Nothing
        gvLeyendas.DataSource = leyendas
        gvLeyendas.DataBind()

    End Sub
    Protected Sub OnPaging_I(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvIdiomas.PageIndex = e.NewPageIndex
        Me.BindGridIdiomas()
    End Sub
    Protected Sub OnPaging_L(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvLeyendas.PageIndex = e.NewPageIndex
        Dim BE_Idioma As BE.BE_Idioma = New BE.BE_Idioma
        BE_Idioma.Codigo = txtIdiomaCodigo.Text
        'BE_Idioma.Codigo = ddlIdioma.SelectedValue
        Me.BindGridLeyendas(BE_Idioma)
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
    Protected Sub B_LIMPIARL_Click(sender As Object, e As EventArgs) Handles B_LIMPIARL.Click
        Me.LimpiarCamposLeyenda()
    End Sub
    Private Sub LimpiarCamposLeyenda()
        txtLeyID.Text = ""
        txtLeyDesc.Text = ""
        lblErrorL.Text = ""
        Me.RestablecerColorFilasGridLeyendas()
    End Sub
    Private Sub RestablecerColorFilasGridLeyendas()
        For Each row As GridViewRow In gvLeyendas.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub
    Protected Sub B_LIMPIARI_Click(sender As Object, e As EventArgs) Handles B_LIMPIARI.Click
        Me.LimpiarCamposIdioma()
        Me.LimpiarCamposLeyenda()
        Me.OcultarDivLeyenda(True)
    End Sub
    Private Sub LimpiarCamposIdioma()
        txtIdiomaCodigo.Text = ""
        txtIdiomaNombre.Text = ""
        lblErrorI.Text = ""
        Me.RestablecerColorFilasGridIdiomas()
    End Sub
    Private Sub RestablecerColorFilasGridIdiomas()
        For Each row As GridViewRow In gvIdiomas.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub
    Protected Sub OnSelectedIndexChanged_I(ByVal sender As Object, ByVal e As EventArgs)

        txtIdiomaCodigo.Text = (DirectCast(gvIdiomas.SelectedRow.FindControl("lblID"), Label).Text)

        txtIdiomaNombre.Text = (DirectCast(gvIdiomas.SelectedRow.FindControl("lblNombre"), Label).Text)

        'Busco las leyendas del idioma seleccionado dentro de la variable de aplicación "Idiomas"
        Dim leyendas As List(Of BE.BE_Leyenda) = DirectCast(Application("Idiomas"), List(Of BE.BE_Idioma)).
                                                 Find(Function(x) x.Codigo = txtIdiomaCodigo.Text).Leyendas

        gvLeyendas.DataSource = Nothing
        gvLeyendas.DataSource = leyendas
        gvLeyendas.DataBind()

        Me.TraducirControlesLeyenda()

        Me.OcultarDivLeyenda(False)

        Me.RestablecerColorFilasGridIdiomas()
        Me.RestablecerColorFilasGridLeyendas()
        gvIdiomas.SelectedRow.BackColor = Drawing.Color.DeepSkyBlue
    End Sub
    Protected Sub OnSelectedIndexChanged_L(ByVal sender As Object, ByVal e As EventArgs)

        Me.RestablecerColorFilasGridLeyendas()
        gvLeyendas.SelectedRow.BackColor = Drawing.Color.DeepSkyBlue

        txtLeyID.Text = (DirectCast(gvLeyendas.SelectedRow.FindControl("lblCodLey"), Label).Text)
        txtLeyDesc.Text = (DirectCast(gvLeyendas.SelectedRow.FindControl("lblDescLey"), Label).Text)

    End Sub

    Protected Sub B_MODIF_L_Click(sender As Object, e As EventArgs) Handles B_MODIF_L.Click

        Dim BE_Leyenda As BE.BE_Leyenda = New BE.BE_Leyenda
        Dim BE_Idioma As BE.BE_Idioma = New BE.BE_Idioma
        Dim SEG_Leyenda As Seguridad.SEG_Leyenda = New Seguridad.SEG_Leyenda

        _usuarioConectado = Session("Usuario_Conectado")
        BE_Idioma.Codigo = txtIdiomaCodigo.Text

        BE_Leyenda.Codigo = txtLeyID.Text
        BE_Leyenda.Descripcion = txtLeyDesc.Text

        If SEG_Leyenda.ModificarIdiomaLeyenda(BE_Leyenda, BE_Idioma) Then

            Dim leyendas As List(Of BE.BE_Leyenda) = SEG_Leyenda.ListarPorIdioma(BE_Idioma)

            Me.BindGridLeyendas(BE_Idioma)

            'Busco dentro de la variable de aplicación "Idiomas" el idioma seleccionado
            'y actualizo las leyendas del mismo
            DirectCast(Application("Idiomas"), List(Of BE.BE_Idioma)). _
            Find(Function(x) x.Codigo = BE_Idioma.Codigo).Leyendas = leyendas
            lblErrorL.ForeColor = Drawing.Color.Green
            lblErrorL.Text = _segIdioma.TraducirControl("MS_008", _usuarioConectado.Idioma)
        Else
            lblErrorL.ForeColor = Drawing.Color.Red
            lblErrorL.Text = _segIdioma.TraducirControl("ME_014", _usuarioConectado.Idioma)
        End If
    End Sub

    Protected Sub B_AGREGAR_Click(sender As Object, e As EventArgs) Handles B_AGREGAR.Click
        Dim BE_Idioma As BE.BE_Idioma = New BE.BE_Idioma
        Dim SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
        Dim SEG_Leyenda As Seguridad.SEG_Leyenda = New Seguridad.SEG_Leyenda

        _usuarioConectado = Session("Usuario_Conectado")
        BE_Idioma.Codigo = txtIdiomaCodigo.Text
        BE_Idioma.Nombre = txtIdiomaNombre.Text

        If SEG_Idioma.AgregarIdioma(BE_Idioma) Then
            Dim leyendas As List(Of BE.BE_Leyenda) = _usuarioConectado.Idioma.Leyendas
            BE_Idioma.Leyendas = leyendas
            For Each leyenda As BE.BE_Leyenda In BE_Idioma.Leyendas
                SEG_Leyenda.ModificarIdiomaLeyenda(leyenda, BE_Idioma)
            Next
            Me.BindGridIdiomas()
            'Obtengo la lista de idiomas almacenada en la variable
            'de aplicación "Idiomas" y le agrego el nuevo idioma
            Dim idiomas As List(Of BE.BE_Idioma) = DirectCast(Application("Idiomas"), List(Of BE.BE_Idioma))
            idiomas.Add(BE_Idioma)
            Application.Remove("Idiomas")
            Application.Add("Idiomas", idiomas)
            Response.Redirect("~/Sites/Sistema/Idiomas.aspx")

            lblErrorI.ForeColor = Drawing.Color.Green
            lblErrorI.Text = _segIdioma.TraducirControl("MS_009", _usuarioConectado.Idioma)

        Else
            lblErrorI.ForeColor = Drawing.Color.Red
            lblErrorI.Text = _segIdioma.TraducirControl("ME_015", _usuarioConectado.Idioma)
        End If
    End Sub

    Protected Sub B_MODIF_I_Click(sender As Object, e As EventArgs) Handles B_MODIF_I.Click
        Dim SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
        Dim BE_Idioma As BE.BE_Idioma = New BE.BE_Idioma

        _usuarioConectado = Session("Usuario_Conectado")
        BE_Idioma.Codigo = txtIdiomaCodigo.Text
        BE_Idioma.Nombre = txtIdiomaNombre.Text

        If SEG_Idioma.ModificarIdioma(BE_Idioma) Then
            Me.BindGridIdiomas()

            'Busco dentro de la variable de aplicación "Idiomas" el idioma seleccionado
            'y actualizo el mismo
            DirectCast(Application("Idiomas"), List(Of BE.BE_Idioma)). _
            Find(Function(x) x.Codigo = BE_Idioma.Codigo).Nombre = BE_Idioma.Nombre

            lblErrorI.ForeColor = Drawing.Color.Green
            lblErrorI.Text = _segIdioma.TraducirControl("MS_010", _usuarioConectado.Idioma)
            Me.LimpiarCamposIdioma()
            Me.LimpiarCamposLeyenda()
            Me.OcultarDivLeyenda(True)
            Response.Redirect("~/Sites/Sistema/Idiomas.aspx")
        Else
            lblErrorI.ForeColor = Drawing.Color.Red
            lblErrorI.Text = _segIdioma.TraducirControl("ME_016", _usuarioConectado.Idioma)
        End If
    End Sub

    Protected Sub B_ELIMINAR_Click(sender As Object, e As EventArgs) Handles B_ELIMINAR.Click
        Dim SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
        Dim BE_Idioma As BE.BE_Idioma = New BE.BE_Idioma

        _usuarioConectado = Session("Usuario_Conectado")
        BE_Idioma.Codigo = txtIdiomaCodigo.Text

        If SEG_Idioma.EliminarIdioma(BE_Idioma) Then
            Me.BindGridIdiomas()

            'Busco dentro de la variable de aplicación "Idiomas" el idioma seleccionado
            'y elimino el mismo
            Dim i As BE.BE_Idioma = DirectCast(Application("Idiomas"), List(Of BE.BE_Idioma)). _
                                    Find(Function(x) x.Codigo = BE_Idioma.Codigo)

            DirectCast(Application("Idiomas"), List(Of BE.BE_Idioma)).Remove(i)
            Me.LimpiarCamposIdioma()
            Me.LimpiarCamposLeyenda()
            Me.OcultarDivLeyenda(True)

            lblErrorI.ForeColor = Drawing.Color.Green
            lblErrorI.Text = _segIdioma.TraducirControl("MS_011", _usuarioConectado.Idioma)
            Response.Redirect("~/Sites/Sistema/Idiomas.aspx")
        Else
            lblErrorI.ForeColor = Drawing.Color.Red
            lblErrorI.Text = _segIdioma.TraducirControl("ME_017", _usuarioConectado.Idioma)
        End If
    End Sub
    Private Sub TraducirControlesIdioma()
        If gvIdiomas.Rows.Count > 0 Then
            gvIdiomas.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
            gvIdiomas.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_NOMBRE", _usuarioConectado.Idioma)
            gvIdiomas.Caption = _segIdioma.TraducirControl("T_IDIOMAS", _usuarioConectado.Idioma)
            gvIdiomas.EmptyDataText = _segIdioma.TraducirControl("L_EDT_LANG", _usuarioConectado.Idioma)

            For Each row As GridViewRow In gvIdiomas.Rows
                DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_SELECT", _usuarioConectado.Idioma)
            Next
        End If
    End Sub
    Private Sub TraducirControlesLeyenda()
        If gvLeyendas.Rows.Count > 0 Then
            gvLeyendas.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_COD_LEY", _usuarioConectado.Idioma)
            gvLeyendas.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_DESC", _usuarioConectado.Idioma)
            gvLeyendas.Caption = _segIdioma.TraducirControl("T_LEYENDAS", _usuarioConectado.Idioma)
            gvLeyendas.EmptyDataText = _segIdioma.TraducirControl("L_EDT_LEY", _usuarioConectado.Idioma)

            For Each row As GridViewRow In gvLeyendas.Rows
                DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_SELECT", _usuarioConectado.Idioma)
            Next
        End If
    End Sub
    Private Sub OcultarDivLeyenda(ByVal verdadero As Boolean)
        If verdadero Then
            gvLeyendas.Visible = False
            L_COD_LEY.Visible = False
            txtLeyID.Visible = False
            L_DESC_LEY.Visible = False
            txtLeyDesc.Visible = False
            B_MODIF_L.Visible = False
            B_LIMPIARL.Visible = False
        Else
            gvLeyendas.Visible = True
            L_COD_LEY.Visible = True
            txtLeyID.Visible = True
            L_DESC_LEY.Visible = True
            txtLeyDesc.Visible = True
            B_MODIF_L.Visible = True
            B_LIMPIARL.Visible = True
        End If
    End Sub
    Private Sub TraducirControles()
        Me.TraducirControlesIdioma()
        Me.TraducirControlesLeyenda()
    End Sub
End Class