Public Class Producto_Detalle
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim mp As MasterPage = Me.Master
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
            End If
            mp.Traducir(mp)
            mp.Traducir(Me)
            Me.LlenarDatos()
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

    Protected Sub B_VOLVER_Click(sender As Object, e As EventArgs) Handles B_VOLVER.Click
        Response.Redirect("~/Sites/Productos.aspx")
    End Sub

    Private Sub LlenarDatos()
        Dim BE_Producto As BE.BE_Producto = DirectCast(Session("ProductoDetalle"), BE.BE_Producto)
        imgProducto.ImageUrl = BE_Producto.FilePath
        txtNombre.Text = BE_Producto.Nombre
        txtDetalle.Text = BE_Producto.Detalle
        txtAlto.Text = BE_Producto.Alto.ToString
        txtAncho.Text = BE_Producto.Ancho.ToString
        txtLargo.Text = BE_Producto.Largo.ToString
    End Sub

End Class