Imports System.Data
Imports System.Web.Script.Serialization
Public Class Clientes
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _bllCliente As BLL.BLL_Cliente = New BLL.BLL_Cliente
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            If Not IsNothing(_usuarioConectado) Then
                Dim mp As MasterPage = Me.Master
                mp.VerificarAutorizacion(Me.VerificarPerfil()) 'Le paso el código del permiso que corresponde a la edición de datos personales
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                mp.Traducir(mp)
                mp.Traducir(Me)
            End If
            Me.BindData()
            Me.VerificarAccion()
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
            Me.TraducirControles()
        End If

    End Sub
    Private Sub BindData()
        Dim BLL_Provincia As BLL.BLL_Provincia = New BLL.BLL_Provincia
        Dim BLL_Localidad As BLL.BLL_Localidad = New BLL.BLL_Localidad
        Dim BLL_IVA As BLL.BLL_IVA = New BLL.BLL_IVA

        ddlIVA.DataSource = Nothing
        ddlIVA.DataSource = BLL_IVA.ListarTodo()
        ddlIVA.DataValueField = "ID"
        ddlIVA.DataTextField = "Nombre"
        ddlIVA.DataBind()

        ddlProvincia.DataSource = Nothing
        ddlProvincia.DataSource = BLL_Provincia.ListarProvincias()
        ddlProvincia.DataValueField = "ID"
        ddlProvincia.DataTextField = "Nombre"
        ddlProvincia.DataBind()

        Dim p As BE.BE_Provincia = New BE.BE_Provincia
        p.ID = ddlProvincia.SelectedValue
        ddlLocalidad.DataSource = Nothing
        ddlLocalidad.DataSource = BLL_Localidad.ListarPorProvincia(p)
        ddlLocalidad.DataValueField = "ID"
        ddlLocalidad.DataTextField = "Nombre"
        ddlLocalidad.DataBind()

    End Sub

    Protected Sub ddlProvincia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvincia.SelectedIndexChanged
        txtClave.Attributes.Add("value", txtClave.Text)
        txtClaveR.Attributes.Add("value", txtClaveR.Text)
        Me.ActualizarLocalidades()
    End Sub
    Private Sub ActualizarLocalidades()
        Dim BLL_Localidad As BLL.BLL_Localidad = New BLL.BLL_Localidad
        Dim p As BE.BE_Provincia = New BE.BE_Provincia
        p.ID = ddlProvincia.SelectedValue
        ddlLocalidad.DataSource = Nothing
        ddlLocalidad.DataSource = BLL_Localidad.ListarPorProvincia(p)
        ddlLocalidad.DataValueField = "ID"
        ddlLocalidad.DataTextField = "Nombre"
        ddlLocalidad.DataBind()
    End Sub

    Protected Sub B_REG_CLI_Click(sender As Object, e As EventArgs) Handles B_REG_CLI.Click
        Dim BE_Cliente As BE.BE_Cliente = New BE.BE_Cliente
        Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New Seguridad.SEG_GestorCifrado
        'Datos cliente
        BE_Cliente.Nombre = txtNombre.Text
        BE_Cliente.Apellido = txtApellido.Text
        BE_Cliente.DNI = Convert.ToInt32(txtDNI.Text)
        BE_Cliente.CUIT = Convert.ToInt64(txtCUIT.Text)
        BE_Cliente.Calle = txtCalle.Text
        BE_Cliente.Altura = Convert.ToInt16(txtAltura.Text)
        If Not String.IsNullOrWhiteSpace(txtPiso.Text) Then
            BE_Cliente.Piso = Convert.ToInt16(txtPiso.Text)
        End If
        If Not String.IsNullOrWhiteSpace(txtDepto.Text) Then
            BE_Cliente.Departamento = txtDepto.Text
        End If
        BE_Cliente.CodigoPostal = Convert.ToInt16(txtCodPos.Text)
        BE_Cliente.Email = txtEmail.Text
        BE_Cliente.Telefono = Convert.ToInt64(txtTelefono.Text)
        BE_Cliente.IVA.ID = ddlIVA.SelectedValue
        BE_Cliente.Provincia.ID = ddlProvincia.SelectedValue
        BE_Cliente.Localidad.ID = ddlLocalidad.SelectedValue
        'Datos usuario
        BE_Cliente.NombreDeUsuario = txtNomUsu.Text
        BE_Cliente.Clave = SEG_GestorCifrado.GetHashMD5(txtClave.Text)
        BE_Cliente.Idioma.Codigo = DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo
        BE_Cliente.Bloqueado = False
        BE_Cliente.Activo = False
        BE_Cliente.CantidadDeIntentosFallidos = 0
        BE_Permiso.ID = 14 'Le hardcodeo el permiso "Cliente"
        BE_Cliente.Perfil.Add(BE_Permiso)
        BE_Cliente.Activo = True

        If Not SEG_Usuario.VerificarCambioNombre(BE_Cliente) Then
            If _bllCliente.AgregarCliente(BE_Cliente) Then
                lblError.ForeColor = Drawing.Color.Green
                lblError.Text = _segIdioma.TraducirControl("MS_017", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
            Else
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = _segIdioma.TraducirControl("ME_048", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
            End If
        Else
            'El nombre de usuario ya existe
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_008", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
        End If

    End Sub

    Private Sub VerificarAccion()
        Dim accion As String = Request.QueryString("Action")
        If IsNothing(accion) Then
            L_ID.Visible = False
            txtID.Visible = False
            B_EDIT_CLI.Visible = False
            B_LIMPIAR.Visible = False
            B_CLOSE_AC.Visible = False
        Else
            If accion = "Edit" Then
                L_NOM_USU.Enabled = False
                L_ID.Visible = False
                txtID.Visible = False
                'Datos cliente
                txtNombre.Enabled = False
                txtApellido.Enabled = False
                txtDNI.Enabled = False
                txtCUIT.Enabled = False
                L_CLAVE.Visible = False
                txtClave.Visible = False
                L_CLAVE_R.Visible = False
                txtClaveR.Visible = False
                B_REG_CLI.Visible = False
                B_LIMPIAR.Visible = False

                Dim BLL_Cliente As BLL.BLL_Cliente = New BLL.BLL_Cliente
                Dim BE_Cliente As BE.BE_Cliente = New BE.BE_Cliente
                BE_Cliente.ID = _usuarioConectado.ID
                BE_Cliente = BLL_Cliente.ObtenerDatosCliente(BE_Cliente)
                'Datos cliente
                txtNombre.Text = BE_Cliente.Nombre
                txtApellido.Text = BE_Cliente.Apellido
                txtDNI.Text = BE_Cliente.DNI.ToString
                txtCUIT.Text = BE_Cliente.CUIT.ToString
                txtCalle.Text = BE_Cliente.Calle
                txtAltura.Text = BE_Cliente.Altura.ToString
                If BE_Cliente.Piso <> 0 Then
                    txtPiso.Text = BE_Cliente.Piso.ToString
                End If
                If Not String.IsNullOrWhiteSpace(BE_Cliente.Departamento) Then
                    txtDepto.Text = BE_Cliente.Departamento
                End If
                txtCodPos.Text = BE_Cliente.CodigoPostal.ToString
                txtEmail.Text = BE_Cliente.Email
                txtTelefono.Text = BE_Cliente.Telefono.ToString
                ddlIVA.SelectedValue = BE_Cliente.IVA.ID
                ddlProvincia.SelectedValue = BE_Cliente.Provincia.ID
                Me.ActualizarLocalidades()

                txtNomUsu.Text = _usuarioConectado.NombreDeUsuario

            Else
                B_EDIT_CLI.Visible = False
                B_CLOSE_AC.Visible = False
                If Not IsNothing(_usuarioConectado) Then
                    Me.BindDataClientes()
                End If
            End If
        End If
    End Sub
    Private Sub BindDataClientes()
        gvClientes.DataSource = Nothing
        gvClientes.DataSource = _bllCliente.ListarTodos()
        gvClientes.DataBind()
        Me.TraducirControles()
    End Sub
    Protected Sub B_EDIT_CLI_Click(sender As Object, e As EventArgs) Handles B_EDIT_CLI.Click
        Dim BE_Cliente As BE.BE_Cliente = New BE.BE_Cliente
        Dim BLL_Cliente As BLL.BLL_Cliente = New BLL.BLL_Cliente
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario

        'Datos cliente
        BE_Cliente.Calle = txtCalle.Text
        BE_Cliente.Altura = Convert.ToInt16(txtAltura.Text)
        If Not String.IsNullOrWhiteSpace(txtPiso.Text) Then
            BE_Cliente.Piso = Convert.ToInt16(txtPiso.Text)
        End If
        If Not String.IsNullOrWhiteSpace(txtDepto.Text) Then
            BE_Cliente.Departamento = txtDepto.Text
        End If
        BE_Cliente.CodigoPostal = Convert.ToInt16(txtCodPos.Text)
        BE_Cliente.Email = txtEmail.Text
        BE_Cliente.Telefono = Convert.ToInt64(txtTelefono.Text)
        BE_Cliente.IVA.ID = ddlIVA.SelectedValue
        BE_Cliente.Provincia.ID = ddlProvincia.SelectedValue
        BE_Cliente.Localidad.ID = ddlLocalidad.SelectedValue
        BE_Cliente.ID = _usuarioConectado.ID

        If BLL_Cliente.ModificarCliente(BE_Cliente) Then
            lblError.ForeColor = Drawing.Color.Green
            lblError.Text = _segIdioma.TraducirControl("MS_019", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
        Else
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_059", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
        End If

    End Sub

    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        Dim mp As MasterPage = Me.Master
        mp.LimpiarCampos(Me)
    End Sub

    Private Sub TraducirControles()
        If gvClientes.Rows.Count > 0 Then
            gvClientes.HeaderRow.Cells(0).Text = _segIdioma.TraducirControl("C_NOMBRE", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_APELLIDO", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_DNI", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_CUIT", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_CALLE", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(5).Text = _segIdioma.TraducirControl("C_ALTURA", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(6).Text = _segIdioma.TraducirControl("C_PISO", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(7).Text = _segIdioma.TraducirControl("C_DEPTO", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(8).Text = _segIdioma.TraducirControl("C_CODPOS", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(9).Text = _segIdioma.TraducirControl("C_EMAIL", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(10).Text = _segIdioma.TraducirControl("C_TELEFONO", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(11).Text = _segIdioma.TraducirControl("C_IVA", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(12).Text = _segIdioma.TraducirControl("C_PROV", _usuarioConectado.Idioma)
            gvClientes.HeaderRow.Cells(13).Text = _segIdioma.TraducirControl("C_LOC", _usuarioConectado.Idioma)
            gvClientes.Caption = _segIdioma.TraducirControl("T_CLIENTES", _usuarioConectado.Idioma)

        End If

    End Sub

    Protected Sub gvClientes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvClientes.PageIndex = e.NewPageIndex
        Me.BindDataClientes()
    End Sub

    Private Function VerificarPerfil() As BE.BE_Permiso
        Dim permiso As BE.BE_Permiso = New BE.BE_Permiso

        For i = 0 To _usuarioConectado.Perfil.Count - 1
            If TypeOf (_usuarioConectado.Perfil.Item(i)) Is BE.BE_GrupoPermiso Then
                For j = 0 To DirectCast(_usuarioConectado.Perfil.Item(i), BE.BE_GrupoPermiso).Permisos.Count - 1
                    If (DirectCast(_usuarioConectado.Perfil.Item(i), BE.BE_GrupoPermiso).Permisos(j).ID = 10) Then 'Clientes_ABM
                        permiso.ID = 10
                        Exit For
                    End If
                    If (DirectCast(_usuarioConectado.Perfil.Item(i), BE.BE_GrupoPermiso).Permisos(j).ID = 16) Then 'Editar_Datos_Personales
                        permiso.ID = 16
                        Exit For
                    End If
                Next
                If permiso.ID <> 0 Then
                    Exit For
                End If
            Else
                If _usuarioConectado.Perfil.Item(i).ID = 10 Then 'Clientes_ABM
                    permiso.ID = 10
                    Exit For
                ElseIf _usuarioConectado.Perfil.Item(i).ID = 16 Then 'Editar_Datos_Personales
                    permiso.ID = 16
                    Exit For
                End If
            End If
        Next
        Return permiso

    End Function
    Protected Sub B_CLOSE_AC_Click(sender As Object, e As EventArgs) Handles B_CLOSE_AC.Click
        B_CLOSE_AC.Visible = False
        B_EDIT_CLI.Visible = False
        B_CONFIRM.Visible = True
        B_CANCELAR.Visible = True
        lblError.Text = _segIdioma.TraducirControl("MI_001", _usuarioConectado.Idioma)
        lblError.ForeColor = Drawing.Color.Red
    End Sub
    Protected Sub B_CANCELAR_Click(sender As Object, e As EventArgs) Handles B_CANCELAR.Click
        B_CLOSE_AC.Visible = True
        B_EDIT_CLI.Visible = True
        B_CONFIRM.Visible = False
        B_CANCELAR.Visible = False
        lblError.Text = ""
    End Sub

    Protected Sub B_CONFIRM_Click(sender As Object, e As EventArgs) Handles B_CONFIRM.Click
        Dim BE_Cliente As BE.BE_Cliente = New BE.BE_Cliente
        Dim BLL_Cliente As BLL.BLL_Cliente = New BLL.BLL_Cliente
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario

        BE_Cliente.ID = _usuarioConectado.ID
        BE_Cliente.NombreDeUsuario = _usuarioConectado.NombreDeUsuario
        BE_Cliente.Bloqueado = _usuarioConectado.Bloqueado
        BE_Cliente.Clave = _usuarioConectado.Clave
        BE_Cliente.Idioma.Codigo = _usuarioConectado.Idioma.Codigo
        BE_Cliente.Idioma.Nombre = _usuarioConectado.Idioma.Nombre
        BE_Cliente.Activo = False

        If BLL_Cliente.BajaCliente(BE_Cliente) Then
            lblError.ForeColor = Drawing.Color.Green
            lblError.Text = _segIdioma.TraducirControl("MS_022", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
            Session.Remove("Usuario_Conectado")
            Response.Redirect("~/Sites/Inicio.aspx")
        Else
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_065", DirectCast(Session("Idioma_Actual"), BE.BE_Idioma))
        End If
    End Sub
End Class