Public Class BLL_TipoFactura
    Private _dalFactura As DAL.DAL_TipoFactura = New DAL.DAL_TipoFactura

    Public Function ListarPorID(ByVal tipoFactura As BE.BE_TipoFactura) As BE.BE_TipoFactura
        Return _dalFactura.ListarPorID(tipoFactura)
    End Function

End Class
