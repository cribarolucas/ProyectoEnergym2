Imports System.IO
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Configuration
Imports System.Drawing
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Public Class Stock
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Private _bllProducto As BLL.BLL_Producto = New BLL.BLL_Producto

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 22 'Le paso el código del permiso que corresponde a la gestión de stock
            mp.VerificarAutorizacion(BE_Permiso)
            If Not IsNothing(_usuarioConectado) Then
                Me.BindDataStock()
                Me.TraducirControles()
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

    Private Sub TraducirControles()
        If gvStock.Rows.Count > 0 Then
            gvStock.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_PROD", _usuarioConectado.Idioma)
            gvStock.HeaderRow.Cells(4).Text = _segIdioma.TraducirControl("C_CANT_ST", _usuarioConectado.Idioma)
            gvStock.Caption = _segIdioma.TraducirControl("T_STOCK", _usuarioConectado.Idioma)

            For Each row As GridViewRow In gvStock.Rows
                DirectCast(row.Cells(0).Controls(0), LinkButton).Text = _segIdioma.TraducirControl("L_SELECT", _usuarioConectado.Idioma)
            Next

        End If
    End Sub

    Private Sub BindDataStock()
        gvStock.DataSource = Nothing
        gvStock.DataSource = _bllProducto.ListarStock()
        gvStock.DataBind()
    End Sub

    Protected Sub gvStock_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvStock.PageIndex = e.NewPageIndex
        Me.BindDataStock()
    End Sub

    Protected Sub gvStock_SelectedIndexChanged(sender As Object, e As EventArgs)
        txtProdID.Text = Convert.ToInt32(DirectCast(gvStock.SelectedRow.FindControl("lblProductoID"), Label).Text)
        txtNombreProducto.Text = DirectCast(gvStock.SelectedRow.FindControl("lblProducto"), Label).Text
        Me.RestablecerColorFilasGrid()
        gvStock.SelectedRow.BackColor = Color.DeepSkyBlue
    End Sub

    Private Sub RestablecerColorFilasGrid()
        For Each row As GridViewRow In gvStock.Rows
            If row.RowIndex Mod 2 <> 0 Then
                row.BackColor = Drawing.Color.LightBlue
            Else
                row.BackColor = Drawing.Color.White
            End If
        Next
    End Sub

    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        Me.LimpiarCampos()
    End Sub

    Protected Sub B_ODP_GEN_Click(sender As Object, e As EventArgs) Handles B_ODP_GEN.Click
        Dim BLL_OrdenProduccion As BLL.BLL_OrdenProduccion = New BLL.BLL_OrdenProduccion
        Dim BE_OrdenProduccion As BE.BE_OrdenProduccion = New BE.BE_OrdenProduccion
        Dim BE_OrdenProduccionDetalle As BE.BE_OrdenProduccionDetalle = New BE.BE_OrdenProduccionDetalle

        BE_OrdenProduccionDetalle.Producto.ID = Convert.ToInt32(txtProdID.Text)
        BE_OrdenProduccionDetalle.Producto.Nombre = txtNombreProducto.Text
        BE_OrdenProduccionDetalle.Cantidad = Convert.ToInt32(txtCantidad.Text)

        BE_OrdenProduccion.Estado.ID = 1
        BE_OrdenProduccion.FechaInicio = Date.Now
        BE_OrdenProduccion.Detalles.Add(BE_OrdenProduccionDetalle)

        If BLL_OrdenProduccion.GenerarOrdenProduccion(BE_OrdenProduccion) Then
            Me.LimpiarCampos()
            'La orden de producción ha sido creada.
            lblError.Text = _segIdioma.TraducirControl("MS_026", _usuarioConectado.Idioma)
            lblError.ForeColor = Color.Green
        Else
            'Error: la orden de producción no ha sido creada.
            lblError.Text = _segIdioma.TraducirControl("ME_076", _usuarioConectado.Idioma)
            lblError.ForeColor = Color.Red
        End If

    End Sub

    Private Sub LimpiarCampos()
        txtProdID.Text = ""
        txtNombreProducto.Text = ""
        txtCantidad.Text = ""
        Me.RestablecerColorFilasGrid()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'MyBase.VerifyRenderingInServerForm(control)

    End Sub

    Protected Sub ExportToPDF(sender As Object, e As EventArgs)
        Using sw As New StringWriter()
            Using hw As New HtmlTextWriter(sw)
                gvStock.RenderControl(hw)
                Dim sr As New StringReader(sw.ToString())
                Dim pdfDoc As New Document(PageSize.A4, 10.0F, 10.0F, 10.0F, 0.0F)
                Dim writer As PdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
                pdfDoc.Open()
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr)
                pdfDoc.Close()
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=Stock.pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.Write(pdfDoc)
                Response.End()
            End Using
        End Using
    End Sub


    Protected Sub ExportToExcel(sender As Object, e As EventArgs)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Stock.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvStock.AllowPaging = False
        Me.BindDataStock()
        'gvProductos.DataBind()

        'Change the Header Row back to white color

        gvStock.HeaderRow.Style.Add("background-color", "#FFFFFF")

        'Apply style to Individual Cells

        gvStock.HeaderRow.Cells(0).Style.Add("background-color", "green")
        gvStock.HeaderRow.Cells(1).Style.Add("background-color", "green")
        gvStock.HeaderRow.Cells(2).Style.Add("background-color", "green")


        For i As Integer = 0 To gvStock.Rows.Count - 1

            Dim row As GridViewRow = gvStock.Rows(i)
            'Change Color back to white
            row.BackColor = System.Drawing.Color.White

            'Apply text style to each Row
            row.Attributes.Add("class", "textmode")

            'Apply style to Individual Cells of Alternating Row

            If i Mod 2 <> 0 Then

                row.Cells(0).Style.Add("background-color", "#C2D69B")
                row.Cells(1).Style.Add("background-color", "#C2D69B")
                row.Cells(2).Style.Add("background-color", "#C2D69B")

            End If
        Next

        gvStock.RenderControl(hw)

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

End Class