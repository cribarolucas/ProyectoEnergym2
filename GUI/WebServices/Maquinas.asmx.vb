Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Newtonsoft.Json

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Maquinas1
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function GenerarPedido(JsonProductos As String) As String
        Dim Productos As List(Of BE.BE_Producto) = JsonConvert.DeserializeObject(JsonProductos)

        Dim BLLP As New BLL.BLL_Producto()

        Return ""
    End Function

End Class