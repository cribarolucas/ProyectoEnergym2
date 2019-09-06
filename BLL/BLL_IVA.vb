Public Class BLL_IVA
    Private _dalIVA As DAL.DAL_IVA = New DAL.DAL_IVA

    Public Function ListarTodo() As List(Of BE.BE_IVA)
        Return _dalIVA.ListarTodo()
    End Function

End Class
