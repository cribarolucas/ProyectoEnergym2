Public Class BLL_Localidad
    Private _dalLocalidad As DAL.DAL_Localidad = New DAL.DAL_Localidad
    Public Function ListarPorProvincia(ByVal provincia As BE.BE_Provincia) As List(Of BE.BE_Localidad)
        Return _dalLocalidad.ListarPorProvincia(provincia)
    End Function
End Class
