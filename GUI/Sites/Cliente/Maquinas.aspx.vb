Imports System.Web.Services
Imports System.Web.Script.Services
Public Class Maquinas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod()>
    <ScriptMethod(UseHttpGet:=True, ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function Test() As String
        Return " { key: 'Pec', color: orange, size: '60 30' }"
    End Function


End Class