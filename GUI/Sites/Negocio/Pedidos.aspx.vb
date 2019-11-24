Imports System.IO
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Configuration
Imports System.Drawing
Imports System.Threading
Imports System.Globalization
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Public Class Pedidos
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _bllPedido As BLL.BLL_Pedido = New BLL.BLL_Pedido
    Private _bllPedidoDetalle As BLL.BLL_PedidoDetalle = New BLL.BLL_PedidoDetalle
    Private _bllEstado As BLL.BLL_Estado = New BLL.BLL_Estado
    Private _bllStock As BLL.BLL_Stock = New BLL.BLL_Stock
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Private _pedidos As List(Of BE.BE_Pedido)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 18 'Le paso el código del permiso que corresponde a la gestión de pedidos
            mp.VerificarAutorizacion(BE_Permiso)
            Me.BindDataEstadoPedidos()
            Me.TraducirControles()
            If Not IsNothing(_usuarioConectado) Then
                mp.AgregarItemsMenuPorPermiso(_usuarioConectado.Perfil)
                mp.Traducir(mp)
                mp.Traducir(Me)
            End If
            B_EXPORTP.Visible = False
            B_EXPORTE.Visible = False
            B_EXPORTP1.Visible = False
            B_EXPORTE1.Visible = False
        End If
        Thread.CurrentThread.CurrentCulture = New CultureInfo(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo)
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
    Protected Sub gvPedidos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvPedidos.SelectedIndexChanged
        hfPedidoID.Value = Convert.ToInt32(DirectCast(gvPedidos.SelectedRow.FindControl("lblID"), Label).Text)
        hfEstadoPedidoID.Value = Convert.ToInt32(DirectCast(gvPedidos.SelectedRow.FindControl("lblEstadoID"), Label).Text)
        Me.RestablecerColorFilasGrid()
        gvPedidos.SelectedRow.BackColor = Color.DeepSkyBlue
        Dim BE_Pedido As BE.BE_Pedido = New BE.BE_Pedido
        BE_Pedido.ID = Convert.ToInt32(DirectCast(gvPedidos.SelectedRow.FindControl("lblID"), Label).Text)
        _pedidos = DirectCast(Session("Pedidos"), List(Of BE.BE_Pedido))
        gvDetalles.DataSource = Nothing
        gvDetalles.DataSource = _pedidos.Find(Function(x) x.ID = BE_Pedido.ID).DetallesPedido
        gvDetalles.DataBind()
        Me.TraducirControles()
    End Sub
    Private Sub RestablecerColorFilasGrid()
        For Each row As GridViewRow In gvPedidos.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub
    Private Sub BindDataPedidos(Optional ByVal mensaje As String = Nothing)
        Dim fDesde As DateTime = Convert.ToDateTime(hfDateFrom.Value)
        Dim fHasta As DateTime = Convert.ToDateTime(hfDateTo.Value)
        Dim estadoPedido As BE.BE_Estado = New BE.BE_Estado
        estadoPedido.ID = ddlEstado.SelectedValue
        gvPedidos.DataSource = Nothing
        _pedidos = _bllPedido.ListarPorFechaEstado(fDesde, fHasta, estadoPedido)
        For i = 0 To _pedidos.Count - 1
            _pedidos.Item(i).DetallesPedido = _bllPedidoDetalle.ListarPorPedido(_pedidos.Item(i))
        Next
        gvPedidos.DataSource = _pedidos

        If IsNothing(mensaje) Then
            If _pedidos.Count = 0 Then
                lblError.Text = _segIdioma.TraducirControl("ME_063", _usuarioConectado.Idioma)
                lblError.ForeColor = Color.Red
            Else
                lblError.Text = ""
                hfEstadoPedidoID.Value = ""
                hfPedidoID.Value = ""
            End If
        End If

        gvPedidos.DataBind()
        Session.Remove("Pedidos")
        Session.Add("Pedidos", _pedidos)
        gvDetalles.DataSource = Nothing
        gvDetalles.DataBind()
        Me.TraducirControles()
        hfPedidoID.Value = ""
    End Sub
    Private Sub BindDataDetalles(ByVal pedido As BE.BE_Pedido)
        gvDetalles.DataSource = Nothing
        gvDetalles.DataSource = pedido.DetallesPedido
        gvDetalles.DataBind()
        Me.TraducirControles()
    End Sub
    Private Sub BindDataEstadoPedidos()
        ddlEstado.DataSource = Nothing
        ddlEstado.DataSource = _bllEstado.ListarTodosPorPedido()
        ddlEstado.DataValueField = "ID"
        ddlEstado.DataTextField = "Nombre"
        ddlEstado.DataBind()
    End Sub
    Private Sub TraducirControles()
        If gvPedidos.Rows.Count > 0 Then
            gvPedidos.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_FECHA", _usuarioConectado.Idioma)
            gvPedidos.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_CLIENTE", _usuarioConectado.Idioma)
            gvPedidos.HeaderRow.Cells(5).Text = _segIdioma.TraducirControl("C_PRECIO_T", _usuarioConectado.Idioma)
            gvPedidos.HeaderRow.Cells(7).Text = _segIdioma.TraducirControl("C_ESTADO", _usuarioConectado.Idioma)
            gvPedidos.Caption = _segIdioma.TraducirControl("T_PEDIDOS", _usuarioConectado.Idioma)
            gvPedidos.CaptionAlign = TableCaptionAlign.Top
            'gvPedidos.EmptyDataText = _segIdioma.TraducirControl("L_EDT_PED", _usuarioConectado.Idioma)

            For Each row As GridViewRow In gvPedidos.Rows
                DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_VER_DET", _usuarioConectado.Idioma)
            Next

        End If
        If gvDetalles.Rows.Count > 0 Then
            gvDetalles.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_PROD", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_CANT", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_PRECIO_U", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(5).Text = _segIdioma.TraducirControl("C_PRECIO_S", _usuarioConectado.Idioma)
            gvDetalles.Caption = _segIdioma.TraducirControl("T_PED_DET", _usuarioConectado.Idioma)
            gvDetalles.CaptionAlign = TableCaptionAlign.Top
        End If
        Thread.CurrentThread.CurrentCulture = New CultureInfo(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo)
    End Sub

    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        hfPedidoID.Value = ""
        lblError.Text = ""
        Me.RestablecerColorFilasGrid()
        gvPedidos.DataSource = Nothing
        gvPedidos.DataBind()
        gvDetalles.DataSource = Nothing
        gvDetalles.DataBind()
        B_EXPORTP.Visible = False
        B_EXPORTE.Visible = False
        B_EXPORTP1.Visible = False
        B_EXPORTE1.Visible = False
    End Sub

    Protected Sub gvPedidos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvPedidos.PageIndex = e.NewPageIndex
        Me.BindDataPedidos()
    End Sub

    Protected Sub gvDetalles_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvDetalles.PageIndex = e.NewPageIndex
        Dim pedido As BE.BE_Pedido = New BE.BE_Pedido
        pedido.ID = Convert.ToInt32(hfPedidoID.Value)
        If pedido.ID > 0 Then
            Me.BindDataDetalles(pedido)
            B_EXPORTP1.Visible = True
            B_EXPORTE1.Visible = True
        End If

    End Sub

    Protected Sub B_PED_LIS_Click(sender As Object, e As EventArgs) Handles B_PED_LIS.Click
        Me.BindDataPedidos()
        B_EXPORTP.Visible = True
        B_EXPORTE.Visible = True
    End Sub

    Protected Sub B_PED_APR_Click(sender As Object, e As EventArgs) Handles B_PED_APR.Click
        Dim BE_EstadoPedido As BE.BE_Estado = New BE.BE_Estado
        BE_EstadoPedido.ID = 2
        Me.ActualizarEstadoPedido(BE_EstadoPedido)
        B_EXPORTP.Visible = False
        B_EXPORTE.Visible = False

    End Sub
    Private Sub ActualizarEstadoPedido(ByVal estado As BE.BE_Estado)
        Dim BE_Pedido As BE.BE_Pedido = New BE.BE_Pedido
        BE_Pedido.ID = Convert.ToInt32(DirectCast(gvPedidos.SelectedRow.FindControl("lblID"), Label).Text)
        BE_Pedido.Fecha = Convert.ToDateTime(DirectCast(gvPedidos.SelectedRow.FindControl("lblFecha"), Label).Text)
        BE_Pedido.Cliente.ID = Convert.ToInt32(DirectCast(gvPedidos.SelectedRow.FindControl("lblClienteID"), Label).Text)
        BE_Pedido.Cliente.IVA.ID = Convert.ToInt32(DirectCast(gvPedidos.SelectedRow.FindControl("lblClienteIVAID"), Label).Text)
        BE_Pedido.PrecioTotal = DirectCast(Session("Pedidos"), List(Of BE.BE_Pedido)).Find(Function(x) x.ID = BE_Pedido.ID).PrecioTotal
        BE_Pedido.Estado = estado

        For i = 0 To gvDetalles.Rows.Count - 1
            Dim BE_PedidoDetalle As BE.BE_PedidoDetalle = New BE.BE_PedidoDetalle
            BE_PedidoDetalle.Producto.ID = Convert.ToInt32(DirectCast(gvDetalles.Rows.Item(i).FindControl("lblProductoID"), Label).Text)
            BE_PedidoDetalle.Cantidad = Convert.ToInt32(DirectCast(gvDetalles.Rows.Item(i).FindControl("lblCantidad"), Label).Text)
            BE_PedidoDetalle.PrecioUnitario = DirectCast(Session("Pedidos"), List(Of BE.BE_Pedido)).Find(Function(x) x.ID = BE_Pedido.ID).DetallesPedido.Find(Function(y) y.Producto.ID = BE_PedidoDetalle.Producto.ID).PrecioUnitario
            BE_PedidoDetalle.PrecioSubtotal = DirectCast(Session("Pedidos"), List(Of BE.BE_Pedido)).Find(Function(x) x.ID = BE_Pedido.ID).DetallesPedido.Find(Function(y) y.Producto.ID = BE_PedidoDetalle.Producto.ID).PrecioSubtotal
            BE_Pedido.DetallesPedido.Add(BE_PedidoDetalle)
        Next

        If _bllPedido.ActualizarEstadoPedido(BE_Pedido) Then
            'El pedido ha sido actualizado.
            lblError.Text = _segIdioma.TraducirControl("MS_024", _usuarioConectado.Idioma)
            lblError.ForeColor = Color.Green
            Me.BindDataPedidos(lblError.Text)
        Else
            'Error: el pedido no ha sido actualizado.
            lblError.Text = _segIdioma.TraducirControl("ME_069", _usuarioConectado.Idioma)
            lblError.ForeColor = Color.Red
        End If

    End Sub

    Protected Sub B_PED_CAN_Click(sender As Object, e As EventArgs) Handles B_PED_CAN.Click
        Dim BE_EstadoPedido As BE.BE_Estado = New BE.BE_Estado
        BE_EstadoPedido.ID = 3
        Me.ActualizarEstadoPedido(BE_EstadoPedido)
        B_EXPORTP.Visible = False
        B_EXPORTE.Visible = False
    End Sub

    Protected Sub gvPedidos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim lblPrecioTotal As Label = DirectCast(e.Row.FindControl("lblPrecioTotal"), Label)
            Dim precioTotal As Decimal = Convert.ToDecimal(lblPrecioTotal.Text)
            Dim culture As CultureInfo = New CultureInfo("es-AR")
            lblPrecioTotal.Text = precioTotal.ToString("C", culture)
        End If
    End Sub

    Protected Sub gvDetalles_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim culture As CultureInfo = New CultureInfo("es-AR")
            Dim lblPrecioUnitario As Label = DirectCast(e.Row.FindControl("lblPrecioUnitario"), Label)
            Dim precioUnitario As Decimal = Convert.ToDecimal(lblPrecioUnitario.Text)
            lblPrecioUnitario.Text = precioUnitario.ToString("C", culture)

            Dim lblPrecioSubtotal As Label = DirectCast(e.Row.FindControl("lblPrecioSubtotal"), Label)
            Dim precioSubtotal As Decimal = Convert.ToDecimal(lblPrecioSubtotal.Text)
            lblPrecioSubtotal.Text = precioSubtotal.ToString("C", culture)
        End If
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'MyBase.VerifyRenderingInServerForm(control)

    End Sub

    Protected Sub ExportToPDF(sender As Object, e As EventArgs)
        Using sw As New StringWriter()
            Using hw As New HtmlTextWriter(sw)
                gvPedidos.RenderControl(hw)
                Dim sr As New StringReader(sw.ToString())
                Dim pdfDoc As New Document(PageSize.A4, 10.0F, 10.0F, 10.0F, 0.0F)
                Dim writer As PdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
                pdfDoc.Open()
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr)
                pdfDoc.Close()
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=Pedidos.pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.Write(pdfDoc)
                Response.End()
            End Using
        End Using
    End Sub

    Protected Sub ExportToExcel(sender As Object, e As EventArgs)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Pedidos.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvPedidos.AllowPaging = False
        Me.BindDataPedidos()
        'gvProductos.DataBind()

        'Change the Header Row back to white color

        gvPedidos.HeaderRow.Style.Add("background-color", "#FFFFFF")

        'Apply style to Individual Cells

        gvPedidos.HeaderRow.Cells(0).Style.Add("background-color", "green")
        gvPedidos.HeaderRow.Cells(1).Style.Add("background-color", "green")
        gvPedidos.HeaderRow.Cells(2).Style.Add("background-color", "green")
        gvPedidos.HeaderRow.Cells(3).Style.Add("background-color", "green")
        gvPedidos.HeaderRow.Cells(4).Style.Add("background-color", "green")

        For i As Integer = 0 To gvPedidos.Rows.Count - 1

            Dim row As GridViewRow = gvPedidos.Rows(i)
            'Change Color back to white
            row.BackColor = System.Drawing.Color.White

            'Apply text style to each Row
            row.Attributes.Add("class", "textmode")

            'Apply style to Individual Cells of Alternating Row

            If i Mod 2 <> 0 Then

                row.Cells(0).Style.Add("background-color", "#C2D69B")
                row.Cells(1).Style.Add("background-color", "#C2D69B")
                row.Cells(2).Style.Add("background-color", "#C2D69B")
                row.Cells(3).Style.Add("background-color", "#C2D69B")
                row.Cells(4).Style.Add("background-color", "#C2D69B")
            End If
        Next

        gvPedidos.RenderControl(hw)

        'style to format numbers to string

        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()


    End Sub

    Protected Sub B_EXPORTP_Click(sender As Object, e As EventArgs) Handles B_EXPORTP.Click

        Me.ExportToPDF(sender, e)

    End Sub

    Protected Sub B_EXPORTE_Click(sender As Object, e As EventArgs) Handles B_EXPORTE.Click
        Me.ExportToExcel(sender, e)
    End Sub


    Protected Sub ExportToPDF1(sender As Object, e As EventArgs)
        Using sw As New StringWriter()
            Using hw As New HtmlTextWriter(sw)
                gvDetalles.RenderControl(hw)
                Dim sr As New StringReader(sw.ToString())
                Dim pdfDoc As New Document(PageSize.A4, 10.0F, 10.0F, 10.0F, 0.0F)
                Dim writer As PdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
                pdfDoc.Open()
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr)
                pdfDoc.Close()
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=Detalles.pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.Write(pdfDoc)
                Response.End()
            End Using
        End Using
    End Sub

    Protected Sub ExportToExcel1(sender As Object, e As EventArgs)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Detalles.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvDetalles.AllowPaging = False
        'Me.BindDataDetalles()
        'gvProductos.DataBind()

        'Change the Header Row back to white color

        gvDetalles.HeaderRow.Style.Add("background-color", "#FFFFFF")

        'Apply style to Individual Cells

        gvDetalles.HeaderRow.Cells(0).Style.Add("background-color", "green")
        gvDetalles.HeaderRow.Cells(1).Style.Add("background-color", "green")
        gvDetalles.HeaderRow.Cells(2).Style.Add("background-color", "green")
        gvDetalles.HeaderRow.Cells(3).Style.Add("background-color", "green")


        For i As Integer = 0 To gvDetalles.Rows.Count - 1

            Dim row As GridViewRow = gvDetalles.Rows(i)
            'Change Color back to white
            row.BackColor = System.Drawing.Color.White

            'Apply text style to each Row
            row.Attributes.Add("class", "textmode")

            'Apply style to Individual Cells of Alternating Row

            If i Mod 2 <> 0 Then

                row.Cells(0).Style.Add("background-color", "#C2D69B")
                row.Cells(1).Style.Add("background-color", "#C2D69B")
                row.Cells(2).Style.Add("background-color", "#C2D69B")
                row.Cells(3).Style.Add("background-color", "#C2D69B")

            End If
        Next

        gvDetalles.RenderControl(hw)

        'style to format numbers to string

        Dim style As String = "<style>.textmode{mso-number-format:\@;}</style>"
        Response.Write(style)
        Response.Output.Write(sw.ToString())
        Response.Flush()
        Response.End()


    End Sub



    Protected Sub B_EXPORTP1_Click(sender As Object, e As EventArgs) Handles B_EXPORTP1.Click
        Me.ExportToPDF1(sender, e)
    End Sub

    Protected Sub B_EXPORTE1_Click(sender As Object, e As EventArgs) Handles B_EXPORTE1.Click
        Me.ExportToExcel1(sender, e)
    End Sub
End Class