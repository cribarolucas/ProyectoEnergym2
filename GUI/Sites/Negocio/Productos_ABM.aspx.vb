Imports System.IO
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports System.Configuration
Imports System.Drawing
Public Class ProductosABM
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _bllProducto As BLL.BLL_Producto = New BLL.BLL_Producto
    Private _bllStock As BLL.BLL_Stock = New BLL.BLL_Stock
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            imgProducto.ImageUrl = "~/Images/Blanco.jpg"
            BE_Permiso.ID = 12 'Le paso el código del permiso que corresponde a la gestión de productos
            mp.VerificarAutorizacion(BE_Permiso)
            Me.BindData()
            Me.TraducirControles()
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
            Me.TraducirControles()
        End If

    End Sub
    Private Sub BindData()
        gvProductos.DataSource = Nothing
        Session.Remove("ProductosABM")
        Session.Add("ProductosABM", _bllProducto.ListarProductos())
        gvProductos.DataSource = DirectCast(Session("ProductosABM"), List(Of BE.BE_Producto))
        gvProductos.DataBind()
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gvProductos.SelectedIndexChanged
        txtID.Text = (DirectCast(gvProductos.SelectedRow.FindControl("lblID"), Label).Text)
        txtNombre.Text = (DirectCast(gvProductos.SelectedRow.FindControl("lblNombre"), Label).Text)
        txtDetalle.Text = (DirectCast(gvProductos.SelectedRow.FindControl("lblDetalle"), TextBox).Text)
        txtPrecio.Text = Convert.ToDecimal(DirectCast(Session("ProductosABM"), List(Of BE.BE_Producto)).Find(Function(x) x.ID = Convert.ToInt32(txtID.Text)).Precio)
        txtAlto.Text = (DirectCast(gvProductos.SelectedRow.FindControl("lblAlto"), Label).Text)
        txtLargo.Text = (DirectCast(gvProductos.SelectedRow.FindControl("lblLargo"), Label).Text)
        txtAncho.Text = (DirectCast(gvProductos.SelectedRow.FindControl("lblAncho"), Label).Text)
        imgProducto.ImageUrl = (DirectCast(gvProductos.SelectedRow.FindControl("imgProducto"), System.Web.UI.WebControls.Image).ImageUrl)
        Me.RestablecerColorFilasGrid()
        gvProductos.SelectedRow.BackColor = Color.DeepSkyBlue
        lblError.Text = ""
    End Sub
    Private Sub LimpiarCampos()
        Dim mp As MasterPage = Me.Master
        mp.LimpiarCampos(Me)
        Me.RestablecerColorFilasGrid()
    End Sub
    Private Sub RestablecerColorFilasGrid()
        For Each row As GridViewRow In gvProductos.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub
    Private Sub TraducirControles()
        gvProductos.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
        gvProductos.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_NOMBRE", _usuarioConectado.Idioma)
        gvProductos.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_DETAIL", _usuarioConectado.Idioma)
        gvProductos.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_PRECIO", _usuarioConectado.Idioma)
        gvProductos.HeaderRow.Cells(7).Text = _segIdioma.TraducirControl("C_IMAGEN", _usuarioConectado.Idioma)
        gvProductos.HeaderRow.Cells(8).Text = _segIdioma.TraducirControl("C_ALTO", _usuarioConectado.Idioma)
        gvProductos.HeaderRow.Cells(9).Text = _segIdioma.TraducirControl("C_LARGO", _usuarioConectado.Idioma)
        gvProductos.HeaderRow.Cells(10).Text = _segIdioma.TraducirControl("C_ANCHO", _usuarioConectado.Idioma)
        gvProductos.Caption = _segIdioma.TraducirControl("T_PRODUC", _usuarioConectado.Idioma)
        gvProductos.EmptyDataText = _segIdioma.TraducirControl("L_EDT_PROD", _usuarioConectado.Idioma)

        For Each row As GridViewRow In gvProductos.Rows
            DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_SELECT", _usuarioConectado.Idioma)
        Next

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvProductos.PageIndex = e.NewPageIndex
        Me.BindData()
    End Sub
    Protected Sub B_AGREGAR_Click(sender As Object, e As EventArgs) Handles B_AGREGAR.Click
        Dim BE_Producto As BE.BE_Producto = New BE.BE_Producto
        BE_Producto.Nombre = txtNombre.Text
        BE_Producto.Detalle = txtDetalle.Text
        BE_Producto.Precio = Convert.ToDecimal(txtPrecio.Text)
        BE_Producto.Tipo = ddltipo1.SelectedValue.ToString
        BE_Producto.Alto = Convert.ToDecimal(txtAlto.Text)
        BE_Producto.Ancho = Convert.ToDecimal(txtAncho.Text)
        BE_Producto.Largo = Convert.ToDecimal(txtLargo.Text)
        BE_Producto.FileName = Path.GetFileName(fuProducto.PostedFile.FileName)
        BE_Producto.FilePath = "~/Images/" + BE_Producto.FileName
        BE_Producto.FilePathThumbnail = "~/Images/Thumbnail_" + BE_Producto.FileName
        BE_Producto.Stock.Cantidad = 0

        Try
            Dim fileName As String = Path.GetFileName(fuProducto.PostedFile.FileName)
            fuProducto.PostedFile.SaveAs((Server.MapPath("~/Images/") + fileName))
            Dim imagen As Image = Image.FromFile((Server.MapPath("~/Images/") + fileName))
            Dim thumbnail As Image = imagen.GetThumbnailImage(100, 100, Nothing, Nothing)
            thumbnail.Save((Server.MapPath("~/Images/") + "Thumbnail_" + fileName))

            If _bllProducto.AgregarProducto(BE_Producto) Then
                Me.LimpiarCampos()
                lblError.Text = _segIdioma.TraducirControl("MS_014", _usuarioConectado.Idioma)
                lblError.ForeColor = Drawing.Color.Green
                Me.BindData()
            Else
                lblError.Text = _segIdioma.TraducirControl("ME_032", _usuarioConectado.Idioma)
                lblError.ForeColor = Drawing.Color.Red
            End If
        Catch ex As Exception
            lblError.Text = _segIdioma.TraducirControl("ME_052", _usuarioConectado.Idioma)
            lblError.ForeColor = Drawing.Color.Red
        End Try

    End Sub

    Protected Sub B_MODIFIC_Click(sender As Object, e As EventArgs) Handles B_MODIFIC.Click
        Dim BE_Producto As BE.BE_Producto = New BE.BE_Producto
        Dim BLL_Producto As BLL.BLL_Producto = New BLL.BLL_Producto
        Dim productos As New List(Of BE.BE_Producto)
        productos = BLL_Producto.ListarProductos

        BE_Producto.ID = Convert.ToInt32(txtID.Text)
        BE_Producto.Nombre = txtNombre.Text
        BE_Producto.Detalle = txtDetalle.Text
        BE_Producto.Precio = Convert.ToDecimal(txtPrecio.Text)
        BE_Producto.Tipo = ddltipo1.SelectedValue.ToString
        BE_Producto.Alto = Convert.ToDecimal(txtAlto.Text)
        BE_Producto.Ancho = Convert.ToDecimal(txtAncho.Text)
        BE_Producto.Largo = Convert.ToDecimal(txtLargo.Text)

        If Not fuProducto.HasFile Then
            BE_Producto.FileName = productos.Find(Function(x) x.ID = BE_Producto.ID).FilePath.Substring(9)
            BE_Producto.FilePath = productos.Find(Function(x) x.ID = BE_Producto.ID).FilePath
            BE_Producto.FilePathThumbnail = productos.Find(Function(x) x.ID = BE_Producto.ID).FilePathThumbnail
            'Dim fileName As String = Path.GetFileName(fuProducto.PostedFile.FileName)
            'fuProducto.PostedFile.SaveAs((Server.MapPath(fileName)))
            'Dim imagen As Image = Image.FromFile((Server.MapPath(fileName)))
            'Dim thumbnail As Image = imagen.GetThumbnailImage(100, 100, Nothing, Nothing)
            'thumbnail.Save((Server.MapPath(fileName)))
        Else
            BE_Producto.FileName = Path.GetFileName(fuProducto.PostedFile.FileName)
            BE_Producto.FilePath = "~/Images/" + BE_Producto.FileName
            BE_Producto.FilePathThumbnail = "~/Images/Thumbnail_" + BE_Producto.FileName
            Dim fileName As String = Path.GetFileName(fuProducto.PostedFile.FileName)
            fuProducto.PostedFile.SaveAs((Server.MapPath("~/Images/") + fileName))
            Dim imagen As Image = Image.FromFile((Server.MapPath("~/Images/") + fileName))
            Dim thumbnail As Image = imagen.GetThumbnailImage(100, 100, Nothing, Nothing)
            thumbnail.Save((Server.MapPath("~/Images/") + "Thumbnail_" + fileName))
        End If

        Try


            If _bllProducto.ModificarProducto(BE_Producto) Then
                Me.LimpiarCampos()
                lblError.Text = _segIdioma.TraducirControl("MS_015", _usuarioConectado.Idioma)
                lblError.ForeColor = Drawing.Color.Green
                Me.BindData()

            Else
                lblError.Text = _segIdioma.TraducirControl("ME_033", _usuarioConectado.Idioma)
                lblError.ForeColor = Drawing.Color.Red
            End If
        Catch ex As Exception
            lblError.Text = _segIdioma.TraducirControl("ME_052", _usuarioConectado.Idioma)
            lblError.ForeColor = Drawing.Color.Red
        End Try
        imgProducto.ImageUrl = "~/Images/Blanco.jpg"
    End Sub
    Protected Sub B_ELIMINAR_Click(sender As Object, e As EventArgs) Handles B_ELIMINAR.Click
        Dim BE_Producto As BE.BE_Producto = New BE.BE_Producto
        BE_Producto.ID = Convert.ToInt32(txtID.Text)

        If _bllStock.EliminarStock(BE_Producto) AndAlso
            _bllProducto.EliminarProducto(BE_Producto) Then
            Me.BindData()
            Me.LimpiarCampos()
            lblError.ForeColor = Drawing.Color.Green
            lblError.Text = _segIdioma.TraducirControl("MS_016", _usuarioConectado.Idioma)
        Else
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = _segIdioma.TraducirControl("ME_034", _usuarioConectado.Idioma)
        End If
        imgProducto.ImageUrl = "~/Images/Blanco.jpg"
    End Sub
    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        Me.LimpiarCampos()
    End Sub

    Protected Sub gvProductos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim culture As CultureInfo = New CultureInfo("es-AR")
            Dim lblPrecio As Label = DirectCast(e.Row.FindControl("lblPrecio"), Label)
            Dim precio As Decimal = Convert.ToDecimal(lblPrecio.Text)
            lblPrecio.Text = precio.ToString("C", culture)
        End If
    End Sub
End Class