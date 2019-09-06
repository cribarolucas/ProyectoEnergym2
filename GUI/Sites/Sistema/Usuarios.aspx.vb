Imports System.Web.Script.Serialization
Public Class Usuarios
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Private _esCliente As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 9 'Le paso el código del permiso que corresponde a la administración de usuarios
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                Me.BindData()
                Me.TraducirControles()
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                mp.Traducir(mp)
                mp.Traducir(Me)
                Me.ObtenerPermisos()
            End If
        End If
    End Sub
    Private Sub BindData()
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
        Dim SEG_Permiso As Seguridad.SEG_Permiso = New Seguridad.SEG_Permiso
        Dim usuariosClientes As List(Of BE.BE_Usuario) = New List(Of BE.BE_Usuario)

        Dim usuarios As List(Of BE.BE_Usuario) = SEG_Usuario.ListarTodos()
        Dim permisos As List(Of BE.BE_Permiso) = SEG_Permiso.listarPermisos()

        gvUsuarios.DataSource = Nothing
        gvUsuarios.DataSource = usuarios
        gvUsuarios.DataBind()

        gvPermisos.DataSource = Nothing
        gvPermisos.DataSource = permisos
        gvPermisos.DataBind()

        ddlIdiomas.DataSource = Nothing
        ddlIdiomas.DataSource = Application("Idiomas")
        ddlIdiomas.DataValueField = "Codigo"
        ddlIdiomas.DataTextField = "Nombre"
        ddlIdiomas.DataBind()

        'Seleccionar el idioma del usuario
        For i = 0 To gvUsuarios.Rows.Count - 1
            Dim u As BE.BE_Usuario = New BE.BE_Usuario
            u.ID = Convert.ToInt32(DirectCast(gvUsuarios.Rows(i).FindControl("lblID"), Label).Text)
            Dim ddl As DropDownList = DirectCast(gvUsuarios.Rows(i).FindControl("ddlIdioma"), DropDownList)
            If IsNothing(ddl) Then
                Dim lblIdioma As Label = DirectCast(gvUsuarios.Rows(i).FindControl("lblIdioma"), Label)
                'Obtengo el idioma del usuario logueado
                Dim idioma As BE.BE_Idioma = usuarios.Find(Function(x) x.ID = u.ID).Idioma
                lblIdioma.Text = idioma.Nombre
            Else
                ddl.Items.FindByValue(u.ID).Selected = True
            End If
        Next

    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gvUsuarios.SelectedIndexChanged
        Dim SEG_Permiso As Seguridad.SEG_Permiso = New Seguridad.SEG_Permiso
        Dim BE_Usuario As BE.BE_Usuario = New BE.BE_Usuario
        Dim esCliente As Boolean

        'Limpio todos los checkboxes
        For i = 1 To gvPermisos.Rows.Count - 1
            Dim checkbox As CheckBox = DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox)
            checkbox.Checked = False
        Next

        'Obtengo el ID del Usuario seleccionado
        BE_Usuario.ID = Convert.ToInt32(DirectCast(gvUsuarios.SelectedRow.FindControl("lblID"), Label).Text)

        'Obtengo los permisos del usuario seleccionado
        BE_Usuario.Perfil = SEG_Permiso.ListarPermisosPorUsuario(BE_Usuario)

        'Selecciono los permisos correspondientes en la pantalla
        For i = 0 To gvPermisos.Rows.Count - 1
            For j = 0 To BE_Usuario.Perfil.Count - 1
                If Convert.ToInt32(DirectCast(gvPermisos.Rows(i).FindControl("lblID"), Label).Text) = BE_Usuario.Perfil(j).ID Then
                    Dim checkbox As CheckBox = DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox)
                    checkbox.Checked = True
                    If BE_Usuario.Perfil(j).ID = 14 Then 'Cliente
                        esCliente = True
                    End If
                End If
            Next
        Next

        'Completo los textboxes con la información del usuario
        txtID.Text = DirectCast(gvUsuarios.SelectedRow.FindControl("lblID"), Label).Text
        txtNombre.Text = DirectCast(gvUsuarios.SelectedRow.FindControl("lblNombre"), Label).Text
        txtClave.Text = DirectCast(gvUsuarios.SelectedRow.FindControl("lblClave"), Label).Text
        cbBloqueado.Checked = Convert.ToBoolean(DirectCast(gvUsuarios.SelectedRow.FindControl("lblBloqueado"), Label).Text)
        txtIntFall.Text = DirectCast(gvUsuarios.SelectedRow.FindControl("lblIntFall"), Label).Text
        cbActivo.Checked = Convert.ToBoolean(DirectCast(gvUsuarios.SelectedRow.FindControl("lblActivo"), Label).Text)
        Dim idiomas As List(Of BE.BE_Idioma) = Application("Idiomas")
        'Busco la leyenda dentro de la lista de leyendas del idioma del usuario
        Dim idioma As BE.BE_Idioma = _
        idiomas.Find(Function(x) x.Nombre = DirectCast(gvUsuarios.SelectedRow.FindControl("lblIdioma"), Label).Text)

        ddlIdiomas.SelectedValue = idioma.Codigo

        If esCliente Then
            hfEsCliente.Value = True
            'Deshabilito los permisos, el nombre y el ID
            For i = 0 To gvPermisos.Rows.Count - 1
                DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox).Enabled = False
            Next
            'txtID.Enabled = False
            txtNombre.Enabled = False
            ddlIdiomas.Enabled = False
        Else
            'Habilito los permisos, el nombre y el ID
            For i = 0 To gvPermisos.Rows.Count - 1
                DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox).Enabled = True
            Next
            'txtID.Enabled = True
            txtNombre.Enabled = True
            ddlIdiomas.Enabled = True
        End If

        Me.RestablecerColorFilasGrid()
        gvUsuarios.SelectedRow.BackColor = Drawing.Color.DeepSkyBlue

    End Sub
    Private Sub TraducirControles()
        Dim SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

        gvUsuarios.HeaderRow.Cells(1).Text = SEG_Idioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
        gvUsuarios.HeaderRow.Cells(2).Text = SEG_Idioma.TraducirControl("C_NOM_USU", _usuarioConectado.Idioma)
        gvUsuarios.HeaderRow.Cells(3).Text = SEG_Idioma.TraducirControl("C_CLAVE", _usuarioConectado.Idioma)
        gvUsuarios.HeaderRow.Cells(4).Text = SEG_Idioma.TraducirControl("C_IDIOMA", _usuarioConectado.Idioma)
        gvUsuarios.HeaderRow.Cells(5).Text = SEG_Idioma.TraducirControl("C_BLOQ", _usuarioConectado.Idioma)
        gvUsuarios.HeaderRow.Cells(6).Text = SEG_Idioma.TraducirControl("C_INT_FALL", _usuarioConectado.Idioma)
        gvUsuarios.Caption = SEG_Idioma.TraducirControl("T_USUARIOS", _usuarioConectado.Idioma)
        gvUsuarios.EmptyDataText = SEG_Idioma.TraducirControl("L_EDT_USER", _usuarioConectado.Idioma)

        gvPermisos.Caption = SEG_Idioma.TraducirControl("T_PERMISOS", _usuarioConectado.Idioma)
        gvPermisos.HeaderRow.Cells(1).Text = SEG_Idioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
        gvPermisos.HeaderRow.Cells(2).Text = SEG_Idioma.TraducirControl("C_NOMBRE", _usuarioConectado.Idioma)
        gvPermisos.EmptyDataText = SEG_Idioma.TraducirControl("L_EDT_PROF", _usuarioConectado.Idioma)

        For Each row As GridViewRow In gvUsuarios.Rows
            DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_SELECT", _usuarioConectado.Idioma)
        Next

    End Sub

    Protected Sub B_AGREGAR_Click(sender As Object, e As EventArgs) Handles B_AGREGAR.Click
        Dim BE_Usuario As BE.BE_Usuario = New BE.BE_Usuario
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New Seguridad.SEG_GestorCifrado
        BE_Usuario.NombreDeUsuario = txtNombre.Text
        BE_Usuario.Clave = SEG_GestorCifrado.GetHashMD5(txtClave.Text)
        BE_Usuario.Idioma = New BE.BE_Idioma
        BE_Usuario.Idioma.Codigo = ddlIdiomas.SelectedItem.Value
        BE_Usuario.Bloqueado = False
        BE_Usuario.CantidadDeIntentosFallidos = 0
        BE_Usuario.Activo = True

        If Not SEG_Usuario.VerificarCambioNombre(BE_Usuario) Then
            'Obtengo los permisos seleccionados
            Me.ObtenerPermisosSeleccionados(BE_Usuario.Perfil)

            If SEG_Usuario.RegistrarUsuario(BE_Usuario) Then
                Me.BindData()
                Me.LimpiarCampos()
                lblError.ForeColor = Drawing.Color.Green
                lblError.Text = _segIdioma.TraducirControl("MS_004", _usuarioConectado.Idioma)
            Else
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = _segIdioma.TraducirControl("ME_007", _usuarioConectado.Idioma)
            End If
        Else
            'El nombre de usuario ya existe
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_008", _usuarioConectado.Idioma)
        End If

    End Sub

    Protected Sub B_MODIFIC_Click(sender As Object, e As EventArgs) Handles B_MODIFIC.Click
        Dim BE_Usuario As BE.BE_Usuario = New BE.BE_Usuario
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New Seguridad.SEG_GestorCifrado

        BE_Usuario.ID = Convert.ToInt32(txtID.Text)
        BE_Usuario.NombreDeUsuario = txtNombre.Text
        BE_Usuario.Clave = SEG_GestorCifrado.GetHashMD5(txtClave.Text)
        BE_Usuario.Idioma = New BE.BE_Idioma
        BE_Usuario.Idioma.Codigo = ddlIdiomas.SelectedValue.ToString
        BE_Usuario.Bloqueado = cbBloqueado.Checked
        BE_Usuario.Activo = cbActivo.Checked
        BE_Usuario.CantidadDeIntentosFallidos = Convert.ToInt32(txtIntFall.Text)

        'Obtengo los permisos seleccionados
        Me.ObtenerPermisosSeleccionados(BE_Usuario.Perfil)

        If Not SEG_Usuario.VerificarCambioNombre(BE_Usuario) Then
            If SEG_Usuario.ModificarUsuario(BE_Usuario) Then
                Me.BindData()
                Me.LimpiarCampos()
                lblError.ForeColor = Drawing.Color.Green
                lblError.Text = _segIdioma.TraducirControl("MS_003", _usuarioConectado.Idioma)
            Else
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = _segIdioma.TraducirControl("ME_006", _usuarioConectado.Idioma)
            End If
        Else
            'El nombre de usuario ya existe
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_008", _usuarioConectado.Idioma)
        End If

    End Sub

    Protected Sub B_ELIMINAR_Click(sender As Object, e As EventArgs) Handles B_ELIMINAR.Click
        Dim BE_Usuario As BE.BE_Usuario = New BE.BE_Usuario
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario

        BE_Usuario.ID = Convert.ToInt32(txtID.Text)

        If SEG_Usuario.EliminarUsuario(BE_Usuario) Then
            Me.BindData()
            Me.LimpiarCampos()
            lblError.ForeColor = Drawing.Color.Green
            lblError.Text = _segIdioma.TraducirControl("MS_002", _usuarioConectado.Idioma)
        Else
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_005", _usuarioConectado.Idioma)
        End If

    End Sub
    Private Sub LimpiarCampos()
        'Limpio todos los checkboxes
        For i = 0 To gvPermisos.Rows.Count - 1
            Dim checkbox As CheckBox = DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox)
            checkbox.Checked = False
        Next
        txtNombre.Enabled = True
        ddlIdiomas.Enabled = True
        'Completo los textboxes con la información del usuario
        Dim mp As MasterPage = Me.Master
        mp.LimpiarCampos(Me)
        ddlIdiomas.SelectedValue = "en-GB"
        Me.RestablecerColorFilasGrid()
    End Sub
    Private Sub RestablecerColorFilasGrid()
        For Each row As GridViewRow In gvUsuarios.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvUsuarios.PageIndex = e.NewPageIndex
        Me.BindData()
    End Sub
    Protected Sub gvUsuarios_RowDataBound(sender As Object, e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim row As GridViewRow = e.Row
            Dim i As Integer = row.RowIndex

            Dim selectCell As TableCell = row.Cells(0)

            If selectCell.Controls.Count > 0 Then
                Dim selectControl As LinkButton = DirectCast(selectCell.Controls(0), LinkButton)

                If Not IsNothing(selectControl) Then
                    Dim SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
                    selectControl.Text = SEG_Idioma.TraducirControl("L_SELECT", _usuarioConectado.Idioma)
                End If

            End If

        End If

    End Sub
    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        Me.LimpiarCampos()
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

    Private Sub ObtenerPermisos()
        Dim SEG_Permiso As Seguridad.SEG_Permiso = New Seguridad.SEG_Permiso
        Dim permisos As List(Of BE.BE_Permiso) = SEG_Permiso.listarPermisos()
        Dim permisos_session As List(Of BE.BE_Permiso) = New List(Of BE.BE_Permiso)
        Dim perfiles_session As List(Of BE.BE_GrupoPermiso) = New List(Of BE.BE_GrupoPermiso)
        For i = 0 To permisos.Count - 1
            If permisos(i).GetType Is GetType(BE.BE_Permiso) Then
                permisos_session.Add(permisos(i))
            Else
                permisos_session.Add(permisos(i))
                perfiles_session.Add(permisos(i))
            End If
        Next

        Session.Remove("PermisosUsuarios")
        Session.Remove("PerfilesUsuarios")
        Session.Add("PermisosUsuarios", permisos_session)
        Session.Add("PerfilesUsuarios", perfiles_session)

    End Sub

    Private Sub ObtenerPermisosSeleccionados(ByRef permisos As List(Of BE.BE_Permiso))
        For i = 0 To gvPermisos.Rows.Count - 1
            If (DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox).Checked) Then
                Dim id As Integer = (Convert.ToInt32(DirectCast(gvPermisos.Rows(i).FindControl("lblID"), Label).Text))
                Dim grupo As BE.BE_GrupoPermiso = DirectCast(Session("PerfilesUsuarios"), List(Of BE.BE_GrupoPermiso)).Find(Function(x) x.ID = id)

                If IsNothing(grupo) Then
                    Dim permiso As BE.BE_Permiso = New BE.BE_Permiso
                    permiso = DirectCast(Session("PermisosUsuarios"), List(Of BE.BE_Permiso)).Find(Function(x) x.ID = id)
                    permisos.Add(permiso)
                Else
                    permisos.Add(grupo)
                End If
            End If
        Next
    End Sub

End Class