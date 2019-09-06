Public Class BLL_Provincia
    Private _dalProvincia As DAL.DAL_Provincia = New DAL.DAL_Provincia
    Public Function ListarProvincias() As List(Of BE.BE_Provincia)
        Return _dalProvincia.ListarProvincias()
    End Function
End Class
