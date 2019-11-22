Imports System.Web.Services
Imports System.Web.Script.Services
Imports Newtonsoft.Json

Public Class Maquinas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod()>
    <ScriptMethod(UseHttpGet:=True, ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function Test() As String
        Dim BLLP As New BLL.BLL_Producto()

        Return Newtonsoft.Json.JsonConvert.SerializeObject(New With {.data = BLLP.ListarProductosG()})
    End Function

    <System.Web.Services.WebMethod()>
    <ScriptMethod(UseHttpGet:=True, ResponseFormat:=ResponseFormat.Json, XmlSerializeString:=False)>
    Public Shared Function GenerarPedido(JsonProductos As String) As String
        Dim Productos As List(Of BE.BE_Producto) = JsonConvert.DeserializeObject(JsonProductos)

        Dim BLLP As New BLL.BLL_Producto()

        Return ""
    End Function


End Class