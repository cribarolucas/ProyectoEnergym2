Imports System.Data
Imports System.Web.Script.Serialization
Public Class Permisos
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
            BE_Permiso.ID = 8 'Le paso el código del permiso que corresponde a la administración de perfiles
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                Me.BindGridViews()
                Me.TraducirControles()
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                mp.Traducir(mp)
                mp.Traducir(Me)
            End If
        End If
    End Sub
    Private Sub BindGridViews()
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

        gvPermisos.DataSource = Nothing
        gvPermisos.DataSource = permisos_session
        gvPermisos.DataBind()

        Session.Add("Permisos", permisos_session)

        gvGrupoPermisos.DataSource = Nothing
        gvGrupoPermisos.DataSource = perfiles_session
        gvGrupoPermisos.DataBind()

        Session.Add("Perfiles", perfiles_session)

    End Sub
    Protected Sub OnSelectedIndexChanged_GP(ByVal sender As Object, ByVal e As EventArgs)

        Dim BE_GrupoPermiso As BE.BE_GrupoPermiso = New BE.BE_GrupoPermiso
        Dim permisos_seleccionar As List(Of BE.BE_Permiso) = New List(Of BE.BE_Permiso)

        'Limpio todos los checkboxes
        For i = 1 To gvPermisos.Rows.Count - 1
            Dim checkbox As CheckBox = DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox)
            checkbox.Checked = False
        Next

        'Obtengo el ID del Perfil seleccionado
        BE_GrupoPermiso.ID = Convert.ToInt32(DirectCast(gvGrupoPermisos.SelectedRow.FindControl("lblID"), Label).Text)

        Me.ObtenerPermisosPerfilSeleccionado(permisos_seleccionar, BE_GrupoPermiso)

        'Selecciono los permisos correspondientes en la pantalla
        For i = 0 To gvPermisos.Rows.Count - 1
            For j = 0 To permisos_seleccionar.Count - 1
                If Convert.ToInt32(DirectCast(gvPermisos.Rows(i).FindControl("lblID"), Label).Text) = permisos_seleccionar(j).ID Then
                    Dim checkbox As CheckBox = DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox)
                    checkbox.Checked = True
                End If
            Next
        Next

        txtID.Text = (DirectCast(gvGrupoPermisos.SelectedRow.FindControl("lblID"), Label).Text)
        txtNombre.Text = (DirectCast(gvGrupoPermisos.SelectedRow.FindControl("lblNombre"), Label).Text)
        Me.RestablecerColorFilasGrid()
        gvGrupoPermisos.SelectedRow.BackColor = Drawing.Color.DeepSkyBlue
    End Sub
    Protected Sub B_AGREGAR_Click(sender As Object, e As EventArgs) Handles B_AGREGAR.Click
        Dim BE_GrupoPermiso As BE.BE_GrupoPermiso = New BE.BE_GrupoPermiso
        Dim SEG_Permiso As Seguridad.SEG_Permiso = New Seguridad.SEG_Permiso
        Dim SEG_GrupoPermiso As Seguridad.SEG_GrupoPermiso = New Seguridad.SEG_GrupoPermiso
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New Seguridad.SEG_GestorCifrado
        Dim perfiles As List(Of BE.BE_GrupoPermiso) = Session("Perfiles")
        Dim permisos As List(Of BE.BE_Permiso) = Session("Permisos")

        Me.ObtenerPermisosSeleccionados(BE_GrupoPermiso)

        BE_GrupoPermiso.Nombre = txtNombre.Text
        'Verifical que el nombre elegido no pertenezca a otro perfil
        If Not SEG_GrupoPermiso.VerificarCambioNombre(BE_GrupoPermiso) Then
            If SEG_GrupoPermiso.agregarPerfil(BE_GrupoPermiso) Then
                perfiles.Add(BE_GrupoPermiso)
                Session.Remove("Perfiles")
                Session.Add("Perfiles", perfiles)

                Me.BindGridViews()
                Me.LimpiarCampos()
                lblError.ForeColor = Drawing.Color.Green
                lblError.Text = _segIdioma.TraducirControl("MS_005", _usuarioConectado.Idioma)
            Else
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = _segIdioma.TraducirControl("ME_009", _usuarioConectado.Idioma)
            End If
        Else
            'Ya existe el nombre del perfil
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_013", _usuarioConectado.Idioma)
        End If

    End Sub
    Protected Sub B_MODIFIC_Click(sender As Object, e As EventArgs) Handles B_MODIFIC.Click
        Dim SEG_GrupoPermiso As Seguridad.SEG_GrupoPermiso = New Seguridad.SEG_GrupoPermiso
        Dim BE_GrupoPermiso As BE.BE_GrupoPermiso = New BE.BE_GrupoPermiso
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New Seguridad.SEG_GestorCifrado

        BE_GrupoPermiso.ID = Convert.ToInt32(txtID.Text)
        BE_GrupoPermiso.Nombre = txtNombre.Text
        BE_GrupoPermiso.DVH = SEG_GestorCifrado.GetHashMD5(BE_GrupoPermiso.ID & BE_GrupoPermiso.Nombre)
        If Not SEG_GrupoPermiso.VerificarRelacionUsuarioPerfil(BE_GrupoPermiso) Then
            Me.ObtenerPermisosSeleccionados(BE_GrupoPermiso)
            BE_GrupoPermiso.ID = Convert.ToInt32(txtID.Text)

            'Verifica que el nombre elegido no pertenezca a otro perfil
            If Not SEG_GrupoPermiso.VerificarCambioNombre(BE_GrupoPermiso) Then
                If SEG_GrupoPermiso.ModificarPerfil(BE_GrupoPermiso) Then
                    Me.BindGridViews()
                    Me.LimpiarCampos()
                    lblError.ForeColor = Drawing.Color.Green
                    lblError.Text = _segIdioma.TraducirControl("MS_006", _usuarioConectado.Idioma)
                Else
                    lblError.ForeColor = Drawing.Color.Red
                    lblError.Text = _segIdioma.TraducirControl("ME_010", _usuarioConectado.Idioma)
                End If
            Else
                'Ya existe el nombre del perfil
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = _segIdioma.TraducirControl("ME_013", _usuarioConectado.Idioma)
            End If
        Else
            'No se puede modificar este perfil. El mismo está siendo usado por al menos un usuario.
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_097", _usuarioConectado.Idioma)
        End If
    End Sub
    Protected Sub B_ELIMINAR_Click(sender As Object, e As EventArgs) Handles B_ELIMINAR.Click

        Dim SEG_GrupoPermiso As Seguridad.SEG_GrupoPermiso = New Seguridad.SEG_GrupoPermiso
        Dim BE_GrupoPermiso As BE.BE_GrupoPermiso = New BE.BE_GrupoPermiso
        BE_GrupoPermiso.ID = Convert.ToInt32(txtID.Text)

        If Not SEG_GrupoPermiso.VerificarRelacionUsuarioPerfil(BE_GrupoPermiso) Then
            If SEG_GrupoPermiso.eliminarPerfil(BE_GrupoPermiso) Then
                Me.BindGridViews()
                Me.LimpiarCampos()
                lblError.ForeColor = Drawing.Color.Green
                lblError.Text = _segIdioma.TraducirControl("MS_007", _usuarioConectado.Idioma)
            Else
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = _segIdioma.TraducirControl("ME_011", _usuarioConectado.Idioma)
            End If
        Else
            'No se puede eliminar este perfil. El mismo está siendo usado por al menos un usuario.
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_096", _usuarioConectado.Idioma)
        End If
    End Sub
    Private Sub ObtenerPermisosPerfilSeleccionado(ByRef permisos As List(Of BE.BE_Permiso), ByVal perfil As BE.BE_GrupoPermiso)

        'Selecciono los permisos de ese perfil
        Dim perfiles As List(Of BE.BE_GrupoPermiso) = Session("Perfiles")

        For i = 0 To perfiles.Count - 1
            If perfiles(i).ID = perfil.ID Then
                For j = 0 To perfiles(i).Permisos.Count - 1
                    permisos.Add(perfiles(i).Permisos(j))
                Next
                Exit For
            End If
        Next
    End Sub
    Private Sub ObtenerPermisosSeleccionados(ByRef perfil As BE.BE_GrupoPermiso)
        perfil = New BE.BE_GrupoPermiso
        For i = 0 To gvPermisos.Rows.Count - 1
            If (DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox).Checked) Then
                Dim id As Integer = (Convert.ToInt32(DirectCast(gvPermisos.Rows(i).FindControl("lblID"), Label).Text))
                Dim grupo As BE.BE_GrupoPermiso = DirectCast(Session("Perfiles"), List(Of BE.BE_GrupoPermiso)).Find(Function(x) x.ID = id)
                If IsNothing(grupo) Then
                    Dim permiso As BE.BE_Permiso = DirectCast(Session("Permisos"), List(Of BE.BE_Permiso)).Find(Function(x) x.ID = id)
                    perfil.Permisos.Add(permiso)
                Else
                    perfil.Permisos.Add(grupo)
                End If
            End If
        Next

    End Sub
    Private Sub LimpiarCampos()
        'Limpio todos los checkboxes
        For i = 0 To gvPermisos.Rows.Count - 1
            Dim checkbox As CheckBox = DirectCast(gvPermisos.Rows(i).FindControl("cbSeleccionar"), CheckBox)
            checkbox.Checked = False
        Next

        Dim mp As MasterPage = Me.Master
        mp.LimpiarCampos(Me)
        Me.RestablecerColorFilasGrid()
    End Sub
    Private Sub RestablecerColorFilasGrid()
        For Each row As GridViewRow In gvGrupoPermisos.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub
    Private Sub TraducirControles()
        Dim SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

        gvGrupoPermisos.HeaderRow.Cells(1).Text = SEG_Idioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
        gvGrupoPermisos.HeaderRow.Cells(2).Text = SEG_Idioma.TraducirControl("C_NOMBRE", _usuarioConectado.Idioma)
        gvGrupoPermisos.Caption = SEG_Idioma.TraducirControl("T_PERFILES", _usuarioConectado.Idioma)
        gvGrupoPermisos.EmptyDataText = SEG_Idioma.TraducirControl("L_EDT_PROF", _usuarioConectado.Idioma)
        gvPermisos.HeaderRow.Cells(1).Text = SEG_Idioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
        gvPermisos.HeaderRow.Cells(2).Text = SEG_Idioma.TraducirControl("C_NOMBRE", _usuarioConectado.Idioma)
        gvPermisos.Caption = SEG_Idioma.TraducirControl("T_PERMISOS", _usuarioConectado.Idioma)
        gvPermisos.EmptyDataText = SEG_Idioma.TraducirControl("L_EDT_PERM", _usuarioConectado.Idioma)

        For Each row As GridViewRow In gvGrupoPermisos.Rows
            DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_SELECT", _usuarioConectado.Idioma)
        Next

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGrupoPermisos.PageIndex = e.NewPageIndex
        Me.BindGridViews()
    End Sub
    Protected Sub RowDataBound_GP(sender As Object, e As GridViewRowEventArgs)

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
End Class