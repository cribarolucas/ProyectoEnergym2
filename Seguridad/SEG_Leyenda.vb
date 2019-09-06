Public Class SEG_Leyenda
    Private DAL_Leyenda As DAL.DAL_Leyenda = New DAL.DAL_Leyenda
    Public Function ListarPorIdioma(ByVal idioma As BE.BE_Idioma) As List(Of BE.BE_Leyenda)
        Return DAL_Leyenda.ListarPorIdioma(idioma)
    End Function
    Public Function EliminarPorIdioma(ByVal idioma As BE.BE_Idioma) As Boolean
        Return DAL_Leyenda.EliminarPorIdioma(idioma)
    End Function
    Public Function ModificarIdiomaLeyenda(ByVal leyenda As BE.BE_Leyenda, ByVal idioma As BE.BE_Idioma) As Boolean
        Return DAL_Leyenda.ModificarIdiomaLeyenda(leyenda, idioma)
    End Function
End Class
