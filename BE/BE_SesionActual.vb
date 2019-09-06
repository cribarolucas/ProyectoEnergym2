Public Class BE_SesionActual

    Private Shared ReadOnly _instancia As BE_SesionActual = New BE_SesionActual

    Public Shared Function ObtenerInstancia() As BE_SesionActual
        Return _instancia
    End Function

    Private _usuarioActual As BE.BE_Usuario = Nothing
    Public Function ObtenerUsuarioActual() As BE.BE_Usuario
        Return _usuarioActual
    End Function

    Public Sub EstablecerUsuarioActual(ByVal usuario As BE.BE_Usuario)
        _usuarioActual = usuario
    End Sub

End Class
