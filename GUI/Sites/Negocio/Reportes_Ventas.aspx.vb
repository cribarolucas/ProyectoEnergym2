Imports System.Object
Imports System.Threading
Imports System.Globalization
Imports System.Web.Script.Serialization
Public Class Reportes_Ventas
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
            BE_Permiso.ID = 19 'Le paso el código del permiso que corresponde a la visualización de los reportes de ventas
            mp.VerificarAutorizacion(BE_Permiso)
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
        End If
    End Sub

    Protected Sub B_ACEPTAR_Click(sender As Object, e As EventArgs) Handles B_ACEPTAR.Click
        Dim BLL_ReporteVentas As BLL.BLL_ReporteVentas = New BLL.BLL_ReporteVentas
        Dim reportes As List(Of BE.BE_ReporteVentas)
        Dim fDesde As DateTime = Convert.ToDateTime(hfDateFrom.Value)
        Dim fHasta As DateTime = Convert.ToDateTime(hfDateTo.Value)

        If RB_CLIENT.Checked Then
            reportes = BLL_ReporteVentas.VentasCliente(fDesde, fHasta)
        Else
            reportes = BLL_ReporteVentas.VentasProducto(fDesde, fHasta)
        End If
        
        If reportes.Count > 0 Then
            lblError.Text = ""
            GraficoTorta.Series.Remove(GraficoTorta.Series.Add("ReporteVentas"))
            GraficoTorta.Series.Add("ReporteVentas")
            GraficoTorta.Series("ReporteVentas").IsValueShownAsLabel = True
            GraficoTorta.Series("ReporteVentas").AxisLabel = "ReporteVentas"
            For i = 0 To reportes.Count - 1
                GraficoTorta.Series("ReporteVentas").LabelFormat = "{#'%'}"
                GraficoTorta.Series("ReporteVentas").IsValueShownAsLabel = True
                GraficoTorta.Series("ReporteVentas").Points.AddXY(reportes(i).Nombre, reportes(i).Porcentaje)
                GraficoTorta.Series("ReporteVentas").Points(GraficoTorta.Series("ReporteVentas").Points.Count - 1).LegendText = reportes(i).Nombre
            Next

            For Each p As DataVisualization.Charting.DataPoint In GraficoTorta.Series("ReporteVentas").Points
                If p.YValues.GetValue(0) = 0 Then
                    p.IsEmpty = True
                Else
                    p.IsEmpty = False
                End If
            Next

            GraficoTorta.Series("ReporteVentas").ChartType = DataVisualization.Charting.SeriesChartType.Pie
            If RB_CLIENT.Checked Then
                L_REP_CLI.Visible = True
                L_REP_PROD.Visible = False
            Else
                L_REP_CLI.Visible = False
                L_REP_PROD.Visible = True
            End If
        Else
            lblError.Text = _segIdioma.TraducirControl("ME_063", _usuarioConectado.Idioma)
            lblError.ForeColor = Drawing.Color.Red
            L_REP_CLI.Visible = False
            L_REP_PROD.Visible = False
        End If

    End Sub

    Protected Sub RB_CLIENT_CheckedChanged(sender As Object, e As EventArgs) Handles RB_CLIENT.CheckedChanged
        L_REP_CLI.Visible = False
        L_REP_PROD.Visible = False
        lblError.Text = ""
    End Sub

    Protected Sub RB_PROD_CheckedChanged(sender As Object, e As EventArgs) Handles RB_PROD.CheckedChanged
        L_REP_CLI.Visible = False
        L_REP_PROD.Visible = False
        lblError.Text = ""
    End Sub

End Class