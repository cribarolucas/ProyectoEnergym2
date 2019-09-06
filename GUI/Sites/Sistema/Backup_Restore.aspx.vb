Imports System.Web.Script.Serialization
Public Class Backup_Restore
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 4 'Le paso el código del permiso que corresponde al backup y restore
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                Me.BindData()
                mp.Traducir(mp)
                mp.Traducir(Me)
            End If
        End If
    End Sub

    Protected Sub B_ACEPTAR_Click(sender As Object, e As EventArgs) Handles B_ACEPTAR.Click
        Dim SEG_BackupRestore As Seguridad.SEG_BackupRestore = New Seguridad.SEG_BackupRestore
        If RB_BACKUP.Checked Then
            _usuarioConectado = Session("Usuario_Conectado")
            If SEG_BackupRestore.RealizarBackup(_usuarioConectado) Then
                lblError.Text = SEG_Idioma.TraducirControl("MS_012", _usuarioConectado.Idioma)
                lblError.ForeColor = Drawing.Color.Green
                Me.BindData()
            Else
                lblError.Text = SEG_Idioma.TraducirControl("ME_027", _usuarioConectado.Idioma)
                lblError.ForeColor = Drawing.Color.Red
            End If
        Else
            If Not IsNothing(lbBackups.SelectedItem) Then
                If SEG_BackupRestore.RealizarRestore(DirectCast(lbBackups.SelectedValue, String).ToString) Then
                    lblError.Text = SEG_Idioma.TraducirControl("MS_013", _usuarioConectado.Idioma)
                    lblError.ForeColor = Drawing.Color.Green
                Else
                    lblError.Text = SEG_Idioma.TraducirControl("ME_028", _usuarioConectado.Idioma)
                    lblError.ForeColor = Drawing.Color.Red
                End If
            End If
        End If
    End Sub

    Private Sub BindData()
        Dim SEG_BackupRestore As Seguridad.SEG_BackupRestore = New Seguridad.SEG_BackupRestore
        lbBackups.DataSource = Nothing
        lbBackups.DataSource = SEG_BackupRestore.ListarBackups()
        lbBackups.DataBind()
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

End Class