'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class Pedidos
    
    '''<summary>
    '''Control hfLeyendasIdiomaActual.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfLeyendasIdiomaActual As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfPedidoID.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfPedidoID As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfEstadoPedidoID.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfEstadoPedidoID As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfDateFrom.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfDateFrom As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfDateTo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfDateTo As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control L_PED_EST.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents L_PED_EST As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlEstado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlEstado As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control L_FEC_D.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents L_FEC_D As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control L_FEC_H.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents L_FEC_H As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control gvPedidos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvPedidos As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control gvDetalles.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvDetalles As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control lblError.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblError As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control B_PED_LIS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents B_PED_LIS As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control B_PED_APR.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents B_PED_APR As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control B_PED_CAN.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents B_PED_CAN As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control B_LIMPIAR.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents B_LIMPIAR As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control B_EXPORTP.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents B_EXPORTP As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control B_EXPORTE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents B_EXPORTE As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control B_EXPORTP1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents B_EXPORTP1 As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control B_EXPORTE1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents B_EXPORTE1 As Global.System.Web.UI.WebControls.Button
End Class
