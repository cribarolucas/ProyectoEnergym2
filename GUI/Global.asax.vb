Imports System.Web.SessionState
Imports System.IO

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        'Obtener idiomas y leyendas
        Dim SEG_Idioma As Seguridad.SEG_Idioma = New Seguridad.SEG_Idioma
        Dim idiomas As List(Of BE.BE_Idioma) = New List(Of BE.BE_Idioma)

        idiomas = SEG_Idioma.Listar()
        Application.Add("Idiomas", idiomas)

        'Verificar la integridad del sistema
        Dim SEG_GestorIntegridad As Seguridad.SEG_GestorIntegridad = New Seguridad.SEG_GestorIntegridad

        If Not SEG_GestorIntegridad.VerificarIntegridad() Then
            Application.Add("BD_Corrupta", True)
        End If

        Me.CreateChartPath()

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Session.Add("Idioma_Actual", DirectCast(Application("Idiomas"), List(Of BE.BE_Idioma)).Item(1))
        Session.Add("EsCliente", False)
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim ex As Exception = Server.GetLastError()
        If TypeOf (ex) Is System.Web.HttpException Then
            Response.Redirect("~/Sites/NotFound.aspx")
        End If
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        Session.Abandon()
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

    Private Sub CreateChartPath()
        Dim ChartPath As String = ConfigurationSettings.AppSettings.Get("ChartImageHandler").ToString
        Dim sDelimStart As String = "c:\" 'First delimiting word
        Dim sDelimEnd As String = "\;" 'Second delimiting word
        Dim nIndexStart As Integer = ChartPath.IndexOf(sDelimStart) 'Find the first occurrence of f1
        Dim nIndexEnd As Integer = ChartPath.IndexOf(sDelimEnd) 'Find the first occurrence of f2
        Dim path As String

        If nIndexStart > -1 AndAlso nIndexEnd > -1 Then '-1 means the word was not found.
            path = Strings.Mid(ChartPath, nIndexStart + sDelimStart.Length + 1, nIndexEnd - nIndexStart - sDelimStart.Length) 'Crop the text between
        End If

        path = "c:\" + path + "\"

        If Not Directory.Exists(path) Then
            Directory.CreateDirectory(path)
        End If
    End Sub

End Class