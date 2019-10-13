Imports System.IO
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Configuration
Public Class Cambiar_Contraseña
    Inherits System.Web.UI.Page
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Private _usuarioConectado As BE.BE_Usuario

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 15 'Le paso el código del permiso que corresponde al cambio de contraseña
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                mp.Traducir(mp)
                mp.Traducir(Me)
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
        End If

    End Sub

    Protected Sub B_ACEPTAR_Click(sender As Object, e As EventArgs) Handles B_ACEPTAR.Click
        Dim BE_Usuario As BE.BE_Usuario = New BE.BE_Usuario
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New Seguridad.SEG_GestorCifrado

        BE_Usuario.Clave = txtClave.Text
        If SEG_Usuario.VerificarContraseña(BE_Usuario, _usuarioConectado) Then
            BE_Usuario = _usuarioConectado
            BE_Usuario.Clave = SEG_GestorCifrado.GetHashMD5(txtClaveNew.Text)
            If SEG_Usuario.ActualizarContraseña(BE_Usuario) Then
                lblError.ForeColor = Drawing.Color.Green
                lblError.Text = _segIdioma.TraducirControl("MS_018", _usuarioConectado.Idioma)
            Else
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = _segIdioma.TraducirControl("ME_057", _usuarioConectado.Idioma)
            End If
        Else
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_058", _usuarioConectado.Idioma)
        End If
    End Sub

End Class