Imports System.Object
Imports System.Threading
Imports System.Globalization
Imports System.Web.Script.Serialization

Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Drawing
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html
Imports iTextSharp.tool.xml


Public Class HistorialVentas
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack() Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 17 'Le paso el código del permiso que corresponde a la visualización del historial de ventas
            mp.VerificarAutorizacion(BE_Permiso)
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
            Me.TraducirControlesGridFacturas()
            Me.TraducirControlesGridDetalles()
            Thread.CurrentThread.CurrentCulture = New CultureInfo(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Codigo)
        End If
    End Sub
    Private Sub TraducirControlesGridFacturas()
        If gvFacturas.Rows.Count > 0 Then
            gvFacturas.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_FECHA", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_CLIENTE", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_TIPO_FAC", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(5).Text = _segIdioma.TraducirControl("C_IVA", _usuarioConectado.Idioma)
            gvFacturas.HeaderRow.Cells(6).Text = _segIdioma.TraducirControl("C_PRECIO_T", _usuarioConectado.Idioma)
            gvFacturas.Caption = _segIdioma.TraducirControl("T_FACT", _usuarioConectado.Idioma)

            For Each row As GridViewRow In gvFacturas.Rows
                DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_VER_DET", _usuarioConectado.Idioma)
            Next
        End If
    End Sub

    Private Sub TraducirControlesGridDetalles()
        If gvDetalles.Rows.Count > 0 Then
            gvDetalles.HeaderRow.Cells(0).Text = _segIdioma.TraducirControl("C_PROD", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_CANT", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_PRECIO_U", _usuarioConectado.Idioma)
            gvDetalles.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_PRECIO_S", _usuarioConectado.Idioma)
            gvDetalles.Caption = _segIdioma.TraducirControl("T_DETALLES", _usuarioConectado.Idioma)
        End If
    End Sub

    Protected Sub OnPaging_gvFacturas(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvFacturas.PageIndex = e.NewPageIndex
        Me.BindGridFacturas()
        gvDetalles.DataSource = Nothing
        gvDetalles.DataBind()
    End Sub

    Protected Sub OnPaging_gvDetalles(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvDetalles.PageIndex = e.NewPageIndex
        Me.BindGridDetalles()
        B_EXPORTP1.Visible = True
        B_EXPORTE1.Visible = True
    End Sub
    Private Sub BindGridDetalles()
        Dim BE_Factura As BE.BE_Factura = New BE.BE_Factura
        Dim BLL_FacturaDetalle As BLL.BLL_FacturaDetalle = New BLL.BLL_FacturaDetalle
        BE_Factura.ID = DirectCast(gvFacturas.SelectedRow.FindControl("lblID"), Label).Text

        gvDetalles.DataSource = Nothing
        gvDetalles.DataSource = BLL_FacturaDetalle.ListarPorFactura(BE_Factura)
        gvDetalles.DataBind()
        Me.TraducirControlesGridDetalles()
    End Sub
    Protected Sub B_ACEPTAR_Click(sender As Object, e As EventArgs) Handles B_ACEPTAR.Click
        Me.BindGridFacturas()
        B_EXPORTP.Visible = True
        B_EXPORTE.Visible = True
    End Sub
    Private Sub BindGridFacturas()
        Dim fDesde As DateTime = hfDateFrom.Value
        Dim fHasta As DateTime = hfDateTo.Value
        Dim BLL_Factura As BLL.BLL_Factura = New BLL.BLL_Factura
        Dim listadoFacturas As List(Of BE.BE_Factura) = New List(Of BE.BE_Factura)
        gvFacturas.DataSource = Nothing
        listadoFacturas = BLL_Factura.ListarPorFechas(fDesde, fHasta)
        If listadoFacturas.Count > 0 Then
            gvFacturas.DataSource = listadoFacturas
            gvFacturas.DataBind()
            Me.TraducirControlesGridFacturas()
            lblError.Text = ""
        Else
            lblError.Text = _segIdioma.TraducirControl("ME_063", _usuarioConectado.Idioma)
            lblError.ForeColor = Drawing.Color.Red
        End If
        Me.RestablecerColorFilasGrid()
    End Sub

    Private Function BindGridFacturas2(f As GridView) As GridView
        Dim gvFacturas2 As New GridView
        f.Columns.RemoveAt(0)
        gvFacturas2 = f
        Return gvFacturas2
    End Function
    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        gvDetalles.DataSource = Nothing
        gvDetalles.DataBind()
        gvFacturas.DataSource = Nothing
        gvFacturas.DataBind()
        Me.RestablecerColorFilasGrid()
        B_EXPORTP.Visible = False
        B_EXPORTE.Visible = False
        B_EXPORTP1.Visible = False
        B_EXPORTE1.Visible = False
    End Sub

    Private Sub RestablecerColorFilasGrid()
        For Each row As GridViewRow In gvFacturas.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub

    Protected Sub OnSelectedIndexChanged_gvFacturas(ByVal sender As Object, ByVal e As EventArgs) Handles gvFacturas.SelectedIndexChanged
        Me.BindGridDetalles()
        Me.RestablecerColorFilasGrid()
        gvFacturas.SelectedRow.BackColor = Drawing.Color.DeepSkyBlue
        B_EXPORTP1.Visible = True
        B_EXPORTE1.Visible = True
    End Sub

    Protected Sub gvFacturas_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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
                gvFacturas.RenderControl(hw)
                Dim sr As New StringReader(sw.ToString())
                Dim pdfDoc As New Document(PageSize.A4, 10.0F, 10.0F, 10.0F, 0.0F)
                Dim writer As PdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
                pdfDoc.Open()
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr)
                pdfDoc.Close()
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=Facturas.pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.Write(pdfDoc)
                Response.End()
            End Using
        End Using
    End Sub



    Protected Sub ExportToExcel(sender As Object, e As EventArgs)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Facturas.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvFacturas.AllowPaging = False
        Me.BindGridFacturas()
        'gvProductos.DataBind()

        'Change the Header Row back to white color

        gvFacturas.HeaderRow.Style.Add("background-color", "#FFFFFF")

        'Apply style to Individual Cells

        gvFacturas.HeaderRow.Cells(0).Style.Add("background-color", "green")
        gvFacturas.HeaderRow.Cells(1).Style.Add("background-color", "green")
        gvFacturas.HeaderRow.Cells(2).Style.Add("background-color", "green")
        gvFacturas.HeaderRow.Cells(3).Style.Add("background-color", "green")
        gvFacturas.HeaderRow.Cells(4).Style.Add("background-color", "green")
        gvFacturas.HeaderRow.Cells(5).Style.Add("background-color", "green")
        gvFacturas.HeaderRow.Cells(6).Style.Add("background-color", "green")

        For i As Integer = 0 To gvFacturas.Rows.Count - 1

            Dim row As GridViewRow = gvFacturas.Rows(i)
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
                row.Cells(5).Style.Add("background-color", "#C2D69B")
                row.Cells(6).Style.Add("background-color", "#C2D69B")
            End If
        Next

        gvFacturas.RenderControl(hw)

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
                'To Export all pages
                gvDetalles.AllowPaging = False
                Me.BindGridDetalles()

                gvDetalles.RenderControl(hw)
                Dim sr As New StringReader(sw.ToString())
                Dim pdfDoc As New Document(PageSize.A2, 10.0F, 10.0F, 10.0F, 0.0F)
                Dim htmlparser As New HTMLWorker(pdfDoc)
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
                pdfDoc.Open()
                htmlparser.Parse(sr)
                pdfDoc.Close()

                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=Detalle.pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.Write(pdfDoc)
                Response.[End]()
            End Using
        End Using
    End Sub


    Protected Sub ExportToExcel1(sender As Object, e As EventArgs)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Detalle.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvDetalles.AllowPaging = False
        Me.BindGridDetalles()
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