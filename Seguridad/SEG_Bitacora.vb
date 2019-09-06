Imports System.IO
Public Class SEG_Bitacora
    Private DAL_Bitacora As DAL.DAL_Bitacora = New DAL.DAL_Bitacora
    Public Function ActualizarBitacora(ByVal bitacora As BE.BE_Bitacora) As Boolean
        Return DAL_Bitacora.ActualizarBitacora(bitacora)
    End Function

    Public Function LeerBitacora(ByVal bitacora As BE.BE_Bitacora, ByVal fHasta As DateTime) As List(Of BE.BE_Bitacora)
        Return DAL_Bitacora.LeerBitacora(bitacora, fHasta)
    End Function

End Class
