Imports System.Object
Imports System.Threading
Imports System.Globalization
Imports System.Web.Script.Serialization
Public Class Bitacora
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
            BE_Permiso.ID = 3 'Le paso el código del permiso que corresponde a la lectura de la bitácora
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                Me.BindData()
                Me.TraducirControles()
                mp.Traducir(mp)
                mp.Traducir(Me)
            End If
        End If
        Thread.CurrentThread.CurrentCulture = New CultureInfo(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo)
    End Sub
    Private Sub BindData()
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
        ddlUsuario.DataSource = Nothing
        ddlUsuario.DataSource = SEG_Usuario.ListarTodos()
        ddlUsuario.DataValueField = "ID"
        ddlUsuario.DataTextField = "NombreDeUsuario"
        ddlUsuario.DataBind()
    End Sub
    Protected Sub Bitacora_Listar(sender As Object, e As EventArgs) Handles B_ACEPTAR.Click
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim SEG_Bitacora As Seguridad.SEG_Bitacora = New Seguridad.SEG_Bitacora
        Dim fHasta As DateTime
        Dim listaBitacora As List(Of BE.BE_Bitacora) = New List(Of BE.BE_Bitacora)
        BE_Bitacora.Usuario = New BE.BE_Usuario

        BE_Bitacora.Usuario.ID = ddlUsuario.SelectedItem.Value
        BE_Bitacora.FechaHora = Convert.ToDateTime(hfDateFrom.Value)
        fHasta = Convert.ToDateTime(hfDateTo.Value)
        BE_Bitacora.EsError = cbError.Checked

        gvBitacora.DataSource = Nothing
        listaBitacora = SEG_Bitacora.LeerBitacora(BE_Bitacora, fHasta)
        If listaBitacora.Count > 0 Then
            gvBitacora.DataSource = listaBitacora
            lblError.Text = ""
        Else
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_063", _usuarioConectado.Idioma)
        End If
        gvBitacora.DataBind()
        Me.TraducirControles()
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvBitacora.PageIndex = e.NewPageIndex
        Me.Bitacora_Listar(sender, e)
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

    Private Sub TraducirControles()
        If gvBitacora.Rows.Count > 0 Then
            gvBitacora.HeaderRow.Cells(0).Text = _segIdioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
            gvBitacora.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_NOM_USU", _usuarioConectado.Idioma)
            gvBitacora.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_FEC_HORA", _usuarioConectado.Idioma)
            gvBitacora.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_DESC", _usuarioConectado.Idioma)
            gvBitacora.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_ES_ERROR", _usuarioConectado.Idioma)
            gvBitacora.Caption = _segIdioma.TraducirControl("T_BITACORA", _usuarioConectado.Idioma)
        End If
        Thread.CurrentThread.CurrentCulture = New CultureInfo(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo)
    End Sub

End Class