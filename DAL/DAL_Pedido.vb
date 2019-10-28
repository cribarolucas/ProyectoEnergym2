Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_Pedido
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL.DAL_Acceso

    Public Function GenerarPedido(ByRef pedido As BE.BE_Pedido) As Boolean

        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim fecha As SqlParameter = New SqlParameter("@fecha", pedido.Fecha)
            Dim id_cliente As SqlParameter = New SqlParameter("@id_cliente", pedido.Cliente.ID)
            Dim precioTotal As SqlParameter = New SqlParameter("@precioTotal", pedido.PrecioTotal)
            Dim id_estadoPedido As SqlParameter = New SqlParameter("@id_estadoPedido", pedido.Estado.ID)
            Dim id_pedido As SqlParameter = New SqlParameter("@id_pedido", SqlDbType.Int)
            id_pedido.Direction = System.Data.ParameterDirection.Output

            parametros.Add(fecha)
            parametros.Add(id_cliente)
            parametros.Add(precioTotal)
            parametros.Add(id_estadoPedido)
            parametros.Add(id_pedido)

            resultado = (_dalAcceso.Escribir("Pedido_Generar", parametros) > 0)

            If resultado Then
                pedido.ID = parametros(4).Value
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Pedido generado exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al generar pedido: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al generar pedido: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado

    End Function

    Public Function ListarPorFechaEstado(ByVal fdesde As DateTime, ByVal fhasta As DateTime,
                                         ByVal estado As BE.BE_Estado) As List(Of BE.BE_Pedido)
        Dim pedidos As List(Of BE.BE_Pedido) = New List(Of BE.BE_Pedido)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim pedido As BE.BE_Pedido
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            Dim fd As SqlParameter = New SqlParameter("@fDesde", fdesde)
            Dim fh As SqlParameter = New SqlParameter("@fHasta", fhasta)
            Dim id_estado As SqlParameter = New SqlParameter("@idEstado", estado.ID)

            parametros.Add(fd)
            parametros.Add(fh)
            parametros.Add(id_estado)

            Dim tabla As DataTable = DAL_Acceso.Leer("Pedido_ListarPorFechaEstado", parametros)

            For Each registro As DataRow In tabla.Rows

                pedido = New BE.BE_Pedido
                pedido.ID = Convert.ToInt32(registro("ID"))
                pedido.Fecha = Convert.ToDateTime(registro("Fecha"))
                pedido.Cliente.ID = Convert.ToInt32(registro("ID_Cliente"))
                pedido.Cliente.IVA.ID = Convert.ToInt32(registro("ID_IVA"))
                pedido.Cliente.Nombre = registro("NombreCliente")
                pedido.Cliente.Apellido = registro("ApellidoCliente")
                pedido.Estado.ID = Convert.ToInt32(registro("ID_EstadoPedido"))
                pedido.Estado.Nombre = registro("EstadoPedido")
                pedido.PrecioTotal = Convert.ToDecimal(registro("PrecioTotal"))

                pedidos.Add(pedido)

            Next

        Catch ex2 As ArgumentNullException

            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex1 As SqlClient.SqlException

            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar pedidos por fechas: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar pedidos por fechas: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return pedidos

    End Function

    Public Function ActualizarEstadoPedido(ByRef pedido As BE.BE_Pedido) As Boolean

        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim id_pedido As SqlParameter = New SqlParameter("@id_pedido", pedido.ID)
            Dim id_estadoPedido As SqlParameter = New SqlParameter("@id_estadoPedido", pedido.Estado.ID)

            parametros.Add(id_pedido)
            parametros.Add(id_estadoPedido)

            resultado = (_dalAcceso.Escribir("Pedido_ActualizarEstado", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Estado de pedido actualizado exitosamente."
                BE_Bitacora.EsError = False
                DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
            End If

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex1 As SqlClient.SqlException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actualizar estado de pedido: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al actaulizar estado de pedido: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado

    End Function

    Public Function ListarAprobados() As List(Of BE.BE_Pedido)
        Dim pedidos As List(Of BE.BE_Pedido) = New List(Of BE.BE_Pedido)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim pedido As BE.BE_Pedido
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            Dim tabla As DataTable = DAL_Acceso.Leer("Pedido_ListarAprobados")

            For Each registro As DataRow In tabla.Rows

                pedido = New BE.BE_Pedido
                pedido.ID = Convert.ToInt32(registro("ID"))
                pedido.Fecha = Convert.ToDateTime(registro("Fecha"))
                pedido.Cliente.ID = Convert.ToInt32(registro("ID_Cliente"))
                pedido.Cliente.Nombre = registro("NombreCliente")
                pedido.Cliente.Apellido = registro("ApellidoCliente")
                pedido.Estado.ID = Convert.ToInt32(registro("ID_EstadoPedido"))
                pedido.Estado.Nombre = registro("EstadoPedido")
                pedido.PrecioTotal = Convert.ToDecimal(registro("PrecioTotal"))

                pedidos.Add(pedido)

            Next

        Catch ex2 As ArgumentNullException

            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex1 As SqlClient.SqlException

            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar los pedidos aprobados: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar los pedidos aprobados: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return pedidos

    End Function

End Class
