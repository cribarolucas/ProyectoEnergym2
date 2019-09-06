Imports System.Web.Script.Serialization
Public Class Login
    Inherits System.Web.UI.Page
    Private _sesionActual As BE.BE_SesionActual = BE.BE_SesionActual.ObtenerInstancia()
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Form.DefaultButton = Me.B_ACEPTAR.UniqueID
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            mp.Traducir(mp)
            mp.Traducir(Me)
        End If
    End Sub

    Protected Sub iniciarSesion(sender As Object, e As EventArgs) Handles B_ACEPTAR.Click
        Dim BE_Usuario As BE.BE_Usuario = New BE.BE_Usuario
        Dim usuario_conectado As BE.BE_Usuario
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario

        BE_Usuario.NombreDeUsuario = txtUser.Text
        BE_Usuario.Clave = txtClave.Text

        'Existe el usuario con el que se está intentando acceder?
        If SEG_Usuario.VerificarDatos(BE_Usuario) Then

            usuario_conectado = SEG_Usuario.ObtenerDatos(BE_Usuario)

            If Not usuario_conectado Is Nothing Then
                If usuario_conectado.Activo Then
                    If Not usuario_conectado.Bloqueado Then
                        'Blanqueo la cantidad de intentos fallidos solo
                        'si la misma es mayor a 0
                        If usuario_conectado.CantidadDeIntentosFallidos > 0 Then
                            usuario_conectado.CantidadDeIntentosFallidos = 0
                            SEG_Usuario.ActualizarIntFallidos(usuario_conectado, True)
                        End If

                        Session.Add("Usuario_Conectado", usuario_conectado)
                        _sesionActual.EstablecerUsuarioActual(usuario_conectado)
                        Session.Remove("Idioma_Actual")
                        Session.Add("Idioma_Actual", usuario_conectado.Idioma)
                        Response.Redirect("~/Sites/Inicio.aspx")
                    Else
                        'Usuario bloqueado
                        lblError.Visible = True
                        lblError.ForeColor = Drawing.Color.Red
                        lblError.Text = _segIdioma.TraducirControl("ME_002", usuario_conectado.Idioma)
                    End If
                Else
                    'Usuario inactivo
                    Dim BE_Cliente As BE.BE_Cliente = New BE.BE_Cliente
                    Dim BLL_Cliente As BLL.BLL_Cliente = New BLL.BLL_Cliente
                    BE_Cliente.ID = usuario_conectado.ID
                    BE_Cliente = BLL_Cliente.ObtenerDatosCliente(BE_Cliente)
                    'Verificar si es un cliente
                    If BE_Cliente.ID = 0 Then
                        lblError.Visible = True
                        lblError.ForeColor = Drawing.Color.Red
                        lblError.Text = _segIdioma.TraducirControl("ME_064", usuario_conectado.Idioma)
                    Else
                        B_ACEPTAR.Visible = False
                        B_CONFIRM.Visible = True
                        B_CANCELAR.Visible = True
                        lblError.Text = _segIdioma.TraducirControl("MI_002", usuario_conectado.Idioma)
                        lblError.ForeColor = Drawing.Color.Red
                        Session.Add("UsuarioInactivo", usuario_conectado)
                    End If
                End If
            Else
                'Contraseña incorrecta
                lblError.Visible = True
                lblError.ForeColor = Drawing.Color.Red
                Dim idioma As BE.BE_Idioma = Session("Idioma_Actual")
                lblError.Text = _segIdioma.TraducirControl("ME_003", idioma)
                SEG_Usuario.ActualizarIntFallidos(BE_Usuario, False)
                If BE_Usuario.Bloqueado Then
                    'Usuario bloqueado
                    lblError.ForeColor = Drawing.Color.Red
                    lblError.Text = _segIdioma.TraducirControl("ME_002", idioma)
                End If
            End If

        Else
            'Usuario o contraseña inválido/a
            lblError.Visible = True
            lblError.ForeColor = Drawing.Color.Red
            Dim idioma As BE.BE_Idioma = Session("Idioma_Actual")
            lblError.Text = _segIdioma.TraducirControl("ME_004", idioma)
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

    Protected Sub B_CANCELAR_Click(sender As Object, e As EventArgs) Handles B_CANCELAR.Click
        Session.Remove("UsuarioInactivo")
        Response.Redirect("~/Sites/Inicio.aspx")
    End Sub

    Protected Sub B_CONFIRM_Click(sender As Object, e As EventArgs) Handles B_CONFIRM.Click
        Dim BE_Usuario As BE.BE_Usuario = DirectCast(Session("UsuarioInactivo"), BE.BE_Usuario)
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
        'Blanqueo la cantidad de intentos fallidos solo
        'si la misma es mayor a 0
        If BE_Usuario.CantidadDeIntentosFallidos > 0 Then
            BE_Usuario.CantidadDeIntentosFallidos = 0
            SEG_Usuario.ActualizarIntFallidos(BE_Usuario, True)
        End If

        BE_Usuario.Activo = True
        If SEG_Usuario.BajaLogica(BE_Usuario) Then
            Session.Add("Usuario_Conectado", BE_Usuario)
            _sesionActual.EstablecerUsuarioActual(BE_Usuario)
            Session.Remove("Idioma_Actual")
            Session.Add("Idioma_Actual", BE_Usuario.Idioma)
            Response.Redirect("~/Sites/Inicio.aspx")
        Else
            lblError.Text = _segIdioma.TraducirControl("ME_088", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
            lblError.ForeColor = Drawing.Color.Red
        End If
    End Sub
End Class