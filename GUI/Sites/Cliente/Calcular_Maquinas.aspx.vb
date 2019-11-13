Imports System.IO
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports System.Configuration
Imports System.Drawing
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Public Class Calcular_Maquinas
    Inherits System.Web.UI.Page
    Private _usuarioConectado As BE.BE_Usuario
    Private _bllProducto As BLL.BLL_Producto = New BLL.BLL_Producto
    Private _bllStock As BLL.BLL_Stock = New BLL.BLL_Stock
    Private _segIdioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
    Dim productos2 As New List(Of BE.BE_Producto)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _usuarioConectado = Session("Usuario_Conectado")
        If Not IsPostBack Then
            Dim jss As JavaScriptSerializer = New JavaScriptSerializer
            hfLeyendasIdiomaActual.Value = jss.Serialize(DirectCast(Session("Idioma_Actual"), BE.BE_Idioma).Leyendas)
            Dim mp As MasterPage = Me.Master
            Dim BE_Permiso As BE.BE_Permiso = New BE.BE_Permiso
            BE_Permiso.ID = 25 'Le paso el código del permiso que corresponde al calculo de maquinas
            mp.VerificarAutorizacion(BE_Permiso)
            Me.BindData()

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
            idioma.Leyendas = DirectCast(Application("Idiomas"),
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
        'Session.Remove("Productos2")

        gvProductos.DataSource = DirectCast(Session("Productos2"), List(Of BE.BE_Producto))
        gvProductos.DataBind()
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gvProductos.SelectedIndexChanged

        gvProductos.SelectedRow.BackColor = Color.DeepSkyBlue
        lblError.Text = ""
        lblMensaje.Text = ""
    End Sub
    Private Sub LimpiarCampos()
        Dim mp As MasterPage = Me.Master
        mp.LimpiarCampos(Me)
        gvProductos.DataSource = Nothing
        gvProductos.DataBind()
        lblMensaje.Text = ""
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
        If gvProductos.Rows.Count > 0 Then
            gvProductos.HeaderRow.Cells(0).Text = _segIdioma.TraducirControl("C_ID", _usuarioConectado.Idioma)
            gvProductos.HeaderRow.Cells(1).Text = _segIdioma.TraducirControl("C_NOMBRE", _usuarioConectado.Idioma)
            gvProductos.HeaderRow.Cells(2).Text = _segIdioma.TraducirControl("C_DETAIL", _usuarioConectado.Idioma)
            gvProductos.HeaderRow.Cells(3).Text = _segIdioma.TraducirControl("C_PRECIO", _usuarioConectado.Idioma)

            gvProductos.Caption = _segIdioma.TraducirControl("T_PRODUC", _usuarioConectado.Idioma)
            gvProductos.EmptyDataText = _segIdioma.TraducirControl("L_EDT_PROD", _usuarioConectado.Idioma)

        End If

    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvProductos.PageIndex = e.NewPageIndex
        Me.BindData()
    End Sub

    Protected Sub gvProductos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim culture As CultureInfo = New CultureInfo("es-AR")
            Dim lblPrecio As Label = DirectCast(e.Row.FindControl("lblPrecio"), Label)
            Dim precio As Decimal = Convert.ToDecimal(lblPrecio.Text)
            lblPrecio.Text = precio.ToString("C", culture)
        End If
    End Sub

    Protected Sub B_LIMPIAR_Click(sender As Object, e As EventArgs) Handles B_LIMPIAR.Click
        Me.LimpiarCampos()
    End Sub

    Protected Sub B_CONFIRM_Click(sender As Object, e As EventArgs) Handles B_CONFIRM.Click
        Dim musculacion As New List(Of BE.BE_Producto)
        Dim cardio As New List(Of BE.BE_Producto)
        Dim monto As New Decimal
        Dim montoInicial As New Decimal
        Dim productos As New List(Of BE.BE_Producto)

        montoInicial = Convert.ToDecimal(txtMonto.Text)
        monto = Convert.ToDecimal(txtMonto.Text)
        musculacion = _bllProducto.ListarMusculacion
        cardio = _bllProducto.ListarCardio

        If RBList.SelectedValue = "RBMuscu" Then
            If monto >= _bllProducto.MuscuMin Then 'menor de los de musculacion
                While monto >= _bllProducto.MuscuMin
                    For Each p As BE.BE_Producto In musculacion
                        If p.Precio <= monto Then
                            productos.Add(p)
                            monto = monto - p.Precio
                        End If
                    Next
                End While
                If monto >= _bllProducto.CardioMin Then 'menor de los de cardio
                    While monto >= _bllProducto.CardioMin
                        For Each p As BE.BE_Producto In cardio
                            If p.Precio <= monto Then
                                productos.Add(p)
                                monto = monto - p.Precio
                            End If
                        Next
                    End While
                End If
            Else 'entra en la lista de cardio de una
                If monto >= _bllProducto.CardioMin Then 'menor de los de cardio
                    While monto >= _bllProducto.CardioMin
                        For Each p As BE.BE_Producto In cardio
                            If p.Precio <= monto Then
                                productos.Add(p)
                                monto = monto - p.Precio
                            End If
                        Next
                    End While
                End If
            End If

        ElseIf RBList.SelectedValue = "RBCardio" Then
            If monto >= _bllProducto.CardioMin Then 'menor de los cardio
                While monto >= _bllProducto.CardioMin
                    For Each p As BE.BE_Producto In cardio
                        If p.Precio <= monto Then
                            productos.Add(p)
                            monto = monto - p.Precio
                        End If
                    Next
                End While
                If monto >= _bllProducto.MuscuMin Then 'menor de los de musculacion
                    While monto >= _bllProducto.MuscuMin
                        For Each p As BE.BE_Producto In musculacion
                            If p.Precio <= monto Then
                                productos.Add(p)
                                monto = monto - p.Precio
                            End If
                        Next
                    End While
                End If
            Else 'entra en la lista de musculacion de una
                If monto >= _bllProducto.MuscuMin Then 'menor de los de musculacion
                    While monto >= _bllProducto.MuscuMin
                        For Each p As BE.BE_Producto In musculacion
                            If p.Precio <= monto Then
                                productos.Add(p)
                                monto = monto - p.Precio
                            End If
                        Next
                    End While
                End If
            End If


        End If



        productos2 = DirectCast(productos.GroupBy(Function(item) item.ID).Select(Function(group) New BE.BE_Producto With {.ID = group.Key, .Nombre = group.First.Nombre, .Detalle = group.First.Detalle, .Precio = group.First.Precio, .Cantidad = group.Count(Function(item) item.ID)}).ToList(), List(Of BE.BE_Producto))

        Dim invertido As New Decimal
        invertido = montoInicial - monto


        Session.Add("Productos2", productos2)
        Me.BindData()
        lblMensaje.Text = "El monto invertido es de " & invertido & " y el monto devuelto es de " & monto

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'MyBase.VerifyRenderingInServerForm(control)

    End Sub

    Protected Sub ExportToPDF(sender As Object, e As EventArgs)
        Using sw As New StringWriter()
            Using hw As New HtmlTextWriter(sw)
                'To Export all pages
                gvProductos.AllowPaging = False
                Me.BindData()

                gvProductos.RenderControl(hw)
                Dim sr As New StringReader(sw.ToString())
                Dim pdfDoc As New Document(PageSize.A2, 10.0F, 10.0F, 10.0F, 0.0F)
                Dim htmlparser As New HTMLWorker(pdfDoc)
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
                pdfDoc.Open()
                htmlparser.Parse(sr)
                pdfDoc.Close()

                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=Maquinas.pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.Write(pdfDoc)
                Response.[End]()
            End Using
        End Using
    End Sub

    Protected Sub ExportToExcel(sender As Object, e As EventArgs)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Maquinas.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        gvProductos.AllowPaging = False
        Me.BindData()
        'gvProductos.DataBind()

        'Change the Header Row back to white color

        gvProductos.HeaderRow.Style.Add("background-color", "#FFFFFF")

        'Apply style to Individual Cells

        gvProductos.HeaderRow.Cells(0).Style.Add("background-color", "green")
        gvProductos.HeaderRow.Cells(1).Style.Add("background-color", "green")
        gvProductos.HeaderRow.Cells(2).Style.Add("background-color", "green")
        gvProductos.HeaderRow.Cells(3).Style.Add("background-color", "green")
        gvProductos.HeaderRow.Cells(4).Style.Add("background-color", "green")

        For i As Integer = 0 To gvProductos.Rows.Count - 1

            Dim row As GridViewRow = gvProductos.Rows(i)
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

        gvProductos.RenderControl(hw)

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