Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.VerificarUsuarioLogueado()
        If Not IsPostBack Then
            ddlIdiomas.DataSource = Nothing
            ddlIdiomas.DataSource = Application("Idiomas")
            ddlIdiomas.DataTextField = "Nombre"
            ddlIdiomas.DataValueField = "Codigo"
            ddlIdiomas.DataBind()
            Me.EstablecerIdiomaDDL()
            Me.EstablecerIdiomaActual()
        End If
    End Sub
    Public Sub Traducir(Objeto As Object)
        Dim idioma As BE.BE_Idioma = Session("Idioma_Actual")
        For Each c As Control In Objeto.Controls
            If TypeOf c Is MasterPage OrElse
               TypeOf c Is HtmlHead OrElse
               TypeOf c Is HtmlForm OrElse
               TypeOf c Is ContentPlaceHolder OrElse
               TypeOf c Is UpdatePanel OrElse
               TypeOf c Is DataList OrElse
               TypeOf c Is Menu Then
                Traducir(c)
            Else
                If TypeOf c Is Label Then
                    'Busco la leyenda dentro de la lista de leyendas del idioma del usuario
                    If DirectCast(c, Label).ID = "L_USU_LOG" Then
                        Me.VerificarUsuarioLogueado()
                    Else
                        If Not IsNothing(idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, Label).ID).Descripcion) Then
                            Dim descripcion As String = idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, Label).ID).Descripcion
                            DirectCast(c, Label).Text = descripcion
                        End If
                    End If
                ElseIf TypeOf c Is Button Then
                    If Not IsNothing(idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, Button).ID).Descripcion) Then
                        Dim descripcion As String = idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, Button).ID).Descripcion
                        'Busco la leyenda dentro de la lista de leyendas del idioma del usuario
                        DirectCast(c, Button).Text = descripcion
                    End If
                ElseIf TypeOf c Is LinkButton Then
                    If Not IsNothing(idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, LinkButton).ID).Descripcion) Then
                        Dim descripcion As String = idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, LinkButton).ID).Descripcion
                        'Busco la leyenda dentro de la lista de leyendas del idioma del usuario
                        DirectCast(c, LinkButton).Text = descripcion
                    End If
                ElseIf TypeOf c Is RadioButton Then
                    If Not IsNothing(idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, RadioButton).ID).Descripcion) Then
                        Dim descripcion As String = idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, RadioButton).ID).Descripcion
                        'Busco la leyenda dentro de la lista de leyendas del idioma del usuario
                        DirectCast(c, RadioButton).Text = descripcion
                    End If
                ElseIf TypeOf c Is HyperLink Then
                    If Not IsNothing(idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, HyperLink).ID).Descripcion) Then
                        Dim descripcion As String = idioma.Leyendas.Find(Function(x) x.Codigo = DirectCast(c, HyperLink).ID).Descripcion
                        'Busco la leyenda dentro de la lista de leyendas del idioma del usuario
                        DirectCast(c, HyperLink).Text = descripcion
                    End If
                End If
            End If
        Next
        'Traducir los items del menú
        For i = 0 To MenuPrincipal.Items.Count - 1
            MenuPrincipal.Items(i).Text = idioma.Leyendas.Find(Function(x) x.Codigo = MenuPrincipal.Items(i).Value).Descripcion
            If MenuPrincipal.Items(i).ChildItems.Count > 0 Then
                For j = 0 To MenuPrincipal.Items(i).ChildItems.Count - 1
                    MenuPrincipal.Items(i).ChildItems(j).Text =
                        idioma.Leyendas.Find(Function(x) x.Codigo = MenuPrincipal.Items(i).ChildItems(j).Value).Descripcion
                Next
            End If
        Next
    End Sub

    Public Sub LimpiarCampos(Objeto As Object)
        For Each c As Control In Objeto.Controls
            If TypeOf c Is MasterPage OrElse
               TypeOf c Is HtmlHead OrElse
               TypeOf c Is HtmlForm OrElse
               TypeOf c Is ContentPlaceHolder OrElse
               TypeOf c Is Menu Then
                LimpiarCampos(c)
            Else
                If TypeOf c Is TextBox Then
                    DirectCast(c, TextBox).Text = ""
                ElseIf TypeOf c Is Label Then
                    If DirectCast(c, Label).ID.Contains("lblError") Then
                        DirectCast(c, Label).Text = ""
                    End If
                ElseIf TypeOf c Is CheckBox Then
                    DirectCast(c, CheckBox).Checked = False
                ElseIf TypeOf c Is Image Then
                    If DirectCast(c, Image).ImageUrl <> "~/Images/Energym - Titulo.jpg" Then
                        DirectCast(c, Image).ImageUrl = ""
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub AgregarItemsMenuPorPermiso(ByVal permisos As List(Of BE.BE_Permiso))
        Dim menuSistema As MenuItem = New MenuItem("Sistema", "M_SYSTEM")
        Dim menuNegocio As MenuItem = New MenuItem("Negocio", "M_NEGOC")
        Dim menuCliente As MenuItem = New MenuItem("Cliente", "M_CLIENTE")
        For i = 0 To permisos.Count - 1
            If TypeOf (permisos.Item(i)) Is BE.BE_GrupoPermiso Then
                If permisos.Item(i).ID = 14 Then 'Cliente
                    Session("EsCliente") = True
                End If
                Me.AgregarItemsMenuPorPermiso(DirectCast(permisos.Item(i), BE.BE_GrupoPermiso).Permisos)
            Else
                Select Case permisos.Item(i).ID
                    Case 3
                        Me.AddItem("Bitácora", "M_BITACORA", "~/Sites/Sistema/Bitacora.aspx", menuSistema)
                    Case 4
                        Me.AddItem("Resguardo y restauración", "M_RESTRESG", "~/Sites/Sistema/Backup_Restore.aspx", menuSistema)
                    Case 5
                        Me.AddItem("Integridad del sistema", "M_INTEGRI", "~/Sites/Sistema/Integridad.aspx", menuSistema)
                    Case 6
                        Me.AddItem("Gestión de idiomas", "M_GEST_IDI", "~/Sites/Sistema/Idiomas.aspx", menuSistema)
                    Case 8
                        Me.AddItem("Gestión de perfiles", "M_GEST_PER", "~/Sites/Sistema/Permisos.aspx", menuSistema)
                    Case 9
                        Me.AddItem("Gestión de usuarios", "M_GEST_USU", "~/Sites/Sistema/Usuarios.aspx", menuSistema)
                    Case 10
                        Me.AddItem("Gestión de clientes", "M_GEST_CLI", "~/Sites/Negocio/Clientes.aspx", menuNegocio, "?Action=Add")
                    Case 12
                        Me.AddItem("Gestión de productos", "M_GEST_PRO", "~/Sites/Negocio/Productos_ABM.aspx", menuNegocio)
                    Case 15
                        Me.AddItem("Cambiar contraseña", "M_CHNG_PWD", "~/Sites/Cliente/Cambiar_Contraseña.aspx", menuCliente)
                    Case 16
                        Me.AddItem("Mi cuenta", "M_EDIT_ACC", "~/Sites/Negocio/Clientes.aspx", menuCliente, "?Action=Edit")
                    Case 17
                        Me.AddItem("Historial de ventas", "M_HIS_VEN", "~/Sites/Negocio/HistorialVentas.aspx", menuNegocio)
                    Case 18
                        Me.AddItem("Pedidos", "M_PEDIDOS", "~/Sites/Negocio/Pedidos.aspx", menuNegocio)
                    Case 19
                        Me.AddItem("Reportes de ventas", "M_REP_VEN", "~/Sites/Negocio/Reportes_Ventas.aspx", menuNegocio)
                    Case 21
                        Me.AddItem("Ordenes de producción", "M_ORD_PROD", "~/Sites/Negocio/Ordenes_Produccion.aspx", menuNegocio)
                    Case 22
                        Me.AddItem("Stock", "M_STOCK", "~/Sites/Negocio/Stock.aspx", menuNegocio)
                    Case 24
                        Me.AddItem("Devoluciones", "M_DEVOL", "~/Sites/Negocio/Devoluciones.aspx", menuNegocio)
                    Case 25
                        Me.AddItem("Diseñar espacio", "M_DISE_ESP", "~/Sites/Cliente/Diseñar_Espacio.aspx", menuCliente)
                    Case 26
                        Me.AddItem("Envios", "M_ENVIOS", "~/Sites/Negocio/Envios.aspx", menuNegocio)
                End Select
            End If
        Next
        If menuSistema.ChildItems.Count > 0 Then
            Dim m As MenuItem = MenuPrincipal.FindItem("M_SYSTEM")
            If IsNothing(m) Then
                MenuPrincipal.Items.Add(menuSistema)
            Else
                For i = 0 To menuSistema.ChildItems.Count - 1
                    m.ChildItems.Add(menuSistema.ChildItems(i))
                Next
            End If
        End If
        If menuNegocio.ChildItems.Count > 0 Then
            Dim m As MenuItem = MenuPrincipal.FindItem("M_NEGOC")
            If IsNothing(m) Then
                MenuPrincipal.Items.Add(menuNegocio)
            Else
                For i = 0 To menuNegocio.ChildItems.Count - 1
                    m.ChildItems.Add(menuNegocio.ChildItems(i))
                Next
            End If
        End If
        If menuCliente.ChildItems.Count > 0 Then
            Dim m As MenuItem = MenuPrincipal.FindItem("M_CLIENTE")
            If IsNothing(m) Then
                MenuPrincipal.Items.Add(menuCliente)
            Else
                For i = 0 To menuCliente.ChildItems.Count - 1
                    m.ChildItems.Add(menuCliente.ChildItems(i))
                Next
            End If
        End If

    End Sub

    Private Sub AddItem(ByVal texto As String, ByVal valor As String, ByVal url As String, ByRef menu As MenuItem, Optional ByVal queryString As String = Nothing)
        Dim m As New MenuItem
        m.Text = texto
        m.Value = valor
        m.NavigateUrl = url + queryString
        menu.ChildItems.Add(m)
    End Sub



    Protected Sub L_LOGOUT_Click(sender As Object, e As EventArgs) Handles L_LOGOUT.Click
        Session.Remove("Usuario_Conectado")

        Session.Remove("EsCliente")
        Response.Redirect("~/Sites/Inicio.aspx")
    End Sub

    Private Sub VerificarUsuarioLogueado()
        Dim usuario As BE.BE_Usuario = Session("Usuario_Conectado")
        If IsNothing(usuario) Then
            L_LOGIN.Visible = True
            L_REGISTRO.Visible = True
            L_LOGOUT.Visible = False
            L_USER_CON.Visible = False
        Else
            L_LOGIN.Visible = False
            L_REGISTRO.Visible = False
            L_LOGOUT.Visible = True
            L_USER_CON.Visible = True
            L_USU_LOG.Text = usuario.NombreDeUsuario
            L_USU_LOG.Visible = True
        End If
    End Sub

    Private Sub EstablecerIdiomaActual()
        Dim idioma As BE.BE_Idioma = New BE.BE_Idioma
        Dim idiomas As List(Of BE.BE_Idioma) = Application("Idiomas")
        idioma.Codigo = DirectCast(ddlIdiomas.SelectedValue, String)

        idioma.Nombre = idiomas.Find(Function(x) x.Codigo = idioma.Codigo).Nombre
        idioma.Leyendas = idiomas.Find(Function(x) x.Codigo = idioma.Codigo).Leyendas

        Session.Remove("Idioma_Actual")
        Session.Add("Idioma_Actual", idioma)

        If Not IsNothing(Session("Usuario_Conectado")) Then
            Dim usuario As BE.BE_Usuario = Session("Usuario_Conectado")
            usuario.Idioma = idioma
            Session("Usuario_Conectado") = usuario
        End If

    End Sub

    Public Sub VerificarAutorizacion(ByVal permiso As BE.BE_Permiso)
        Dim usuario As BE.BE_Usuario = Session("Usuario_Conectado")
        Dim tienePermiso As Boolean
        Dim SEG_GrupoPermiso As Seguridad.SEG_GrupoPermiso = New Seguridad.SEG_GrupoPermiso
        If IsNothing(usuario) Then
            Response.Redirect("~/Sites/Unauthorized.aspx")
        Else
            tienePermiso = SEG_GrupoPermiso.Validar(permiso, usuario.Perfil)
        End If
        If Not tienePermiso Then
            Response.Redirect("~/Sites/Unauthorized.aspx")
        End If
    End Sub

    Public Sub EstablecerIdiomaDDL()
        If IsNothing(Session("Idioma_Actual")) Then
            ddlIdiomas.SelectedValue = "es-AR" 'Idioma por defecto
        Else
            ddlIdiomas.SelectedValue = DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo
        End If
    End Sub

    Public Event ddlIdiomas_SelIdxChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Protected Sub ddlIdiomas_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlIdiomas.SelectedIndexChanged
        RaiseEvent ddlIdiomas_SelIdxChanged(Me, e)
    End Sub

    Protected Sub L_LOGIN_Click(sender As Object, e As EventArgs) Handles L_LOGIN.Click
        Response.Redirect("~/Sites/Sistema/Login.aspx")
    End Sub

    Protected Sub L_REGISTRO_Click(sender As Object, e As EventArgs) Handles L_REGISTRO.Click
        Response.Redirect("~/Sites/Negocio/Clientes.aspx")
    End Sub


End Class