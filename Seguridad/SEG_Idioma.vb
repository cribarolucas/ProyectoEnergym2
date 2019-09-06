Public Class SEG_Idioma
    Private DAL_Idioma As DAL.DAL_Idioma = New DAL.DAL_Idioma
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual
    Public Function Listar() As List(Of BE.BE_Idioma)
        Dim SEG_Leyenda As SEG_Leyenda = New SEG_Leyenda
        Dim idiomas As List(Of BE.BE_Idioma) = New List(Of BE.BE_Idioma)
        idiomas = DAL_Idioma.Listar()

        For i = 0 To idiomas.Count - 1
            idiomas(i).Leyendas.AddRange(SEG_Leyenda.ListarPorIdioma(idiomas(i)))
        Next

        Return idiomas

    End Function

    Public Function AgregarIdioma(ByRef idioma As BE.BE_Idioma) As Boolean
        Dim resultado As Boolean
        Dim DAL_Leyenda As DAL.DAL_Leyenda = New DAL.DAL_Leyenda
        Dim leyendas As List(Of BE.BE_Leyenda) = DAL_Leyenda.Listar()
        If DAL_Idioma.AgregarIdioma(idioma) AndAlso
            DAL_Leyenda.AgregarIdiomaLeyenda(idioma, leyendas) Then
            idioma.Leyendas = leyendas
            resultado = True
        End If
        Return resultado
    End Function

    Public Function ModificarIdioma(ByVal idioma As BE.BE_Idioma) As Boolean
        Return DAL_Idioma.ModificarIdioma(idioma)
    End Function

    Public Function EliminarIdioma(ByVal idioma As BE.BE_Idioma) As Boolean
        Dim resultado As Boolean
        Dim SEG_Leyenda As SEG_Leyenda = New SEG_Leyenda

        If SEG_Leyenda.ListarPorIdioma(idioma).Count > 0 Then
            If SEG_Leyenda.EliminarPorIdioma(idioma) AndAlso
                DAL_Idioma.EliminarIdioma(idioma) Then
                resultado = True
            End If
        Else
            'No existen leyendas para el idioma,
            'entonces lo elimino
            If DAL_Idioma.EliminarIdioma(idioma) Then
                resultado = True
            End If
        End If
        Return resultado
    End Function

    Public Function TraducirControl(ByVal codigo As String, ByVal idioma As BE.BE_Idioma) As String

        'Busco la leyenda dentro de la lista de leyendas del idioma del usuario
        Dim leyenda As BE.BE_Leyenda
        leyenda = idioma.Leyendas.Find(Function(x) x.Codigo = codigo)

        Return leyenda.Descripcion

    End Function

End Class
