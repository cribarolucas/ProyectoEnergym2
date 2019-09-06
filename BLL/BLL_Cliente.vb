Public Class BLL_Cliente
    Private _dalCliente As DAL.DAL_Cliente = New DAL.DAL_Cliente
    Private _dalUsuario As DAL.DAL_Usuario = New DAL.DAL_Usuario
    Private _segGestorCifrado As Seguridad.SEG_GestorCifrado = New Seguridad.SEG_GestorCifrado
    Private _segGestorIntegridad As Seguridad.SEG_GestorIntegridad = New Seguridad.SEG_GestorIntegridad
    Public Function AgregarCliente(ByVal cliente As BE.BE_Cliente) As Boolean
        Dim resultado As Boolean
        Dim SEG_Permiso As Seguridad.SEG_Permiso = New Seguridad.SEG_Permiso
        cliente.ID = _dalUsuario.ObtenerMaxID() + 1
        cliente.DVH = _segGestorCifrado.GetHashMD5(cliente.ID.ToString & cliente.NombreDeUsuario & cliente.Clave & _
                                                   cliente.Idioma.Codigo & cliente.CantidadDeIntentosFallidos.ToString & _
                                                   cliente.Bloqueado.ToString & cliente.Activo.ToString)

        If _dalUsuario.RegistrarUsuario(cliente) AndAlso
            _dalCliente.AgregarCliente(cliente) Then
            _segGestorIntegridad.ActualizarDVV("Usuario")
            resultado = SEG_Permiso.AgregarPermisoUsuario(cliente)
        End If

        Return resultado
    End Function
    Public Function ModificarCliente(ByVal cliente As BE.BE_Cliente) As Boolean
        Return _dalCliente.ModificarCliente(cliente)
    End Function
    Public Function ObtenerDatosCliente(ByVal cliente As BE.BE_Cliente) As BE.BE_Cliente
        Return _dalCliente.ObtenerDatosCliente(cliente)
    End Function
    Public Function BajaCliente(ByVal cliente As BE.BE_Cliente) As Boolean
        Dim SEG_Usuario As Seguridad.SEG_Usuario = New Seguridad.SEG_Usuario
        Return SEG_Usuario.BajaLogica(cliente)
    End Function
    Public Function ListarTodos() As List(Of BE.BE_Cliente)
        Return _dalCliente.ListarTodos()
    End Function
End Class