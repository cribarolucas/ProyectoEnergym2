Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_Factura
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL.DAL_Acceso

    Public Function GenerarFactura(ByRef factura As BE.BE_Factura) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim fecha As SqlParameter = New SqlParameter("@fecha", factura.Fecha)
            Dim id_cliente As SqlParameter = New SqlParameter("@id_cliente", factura.Cliente.ID)
            Dim id_tipof As SqlParameter = New SqlParameter("@id_tipof", factura.TipoFactura.ID)
            Dim id_iva As SqlParameter = New SqlParameter("@id_iva", factura.IVA.ID)
            Dim id_pedido As SqlParameter = New SqlParameter("@id_pedido", factura.Pedido.ID)
            Dim precioTotal As SqlParameter = New SqlParameter("@precioTotal", factura.PrecioTotal)
            Dim dvh As SqlParameter = New SqlParameter("@dvh", factura.DVH)
            Dim id_factura As SqlParameter = New SqlParameter("@id_f", SqlDbType.Int)
            id_factura.Direction = System.Data.ParameterDirection.Output

            parametros.Add(fecha)
            parametros.Add(id_cliente)
            parametros.Add(id_tipof)
            parametros.Add(id_iva)
            parametros.Add(id_pedido)
            parametros.Add(precioTotal)
            parametros.Add(dvh)
            parametros.Add(id_factura)

            resultado = (_dalAcceso.Escribir("Factura_Generar", parametros) > 0)

            If resultado Then
                factura.ID = parametros(7).Value
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Factura generada exitosamente."
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
            BE_Bitacora.Descripcion = "Error al generar factura: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al generar factura: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado

    End Function

    Public Function ListarTodos() As List(Of BE.BE_Factura)
        Dim facturas As List(Of BE.BE_Factura) = New List(Of BE.BE_Factura)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim factura As BE.BE_Factura

        Try

            Dim tabla As DataTable = DAL_Acceso.Leer("Factura_ListarTodos")

            For Each registro As DataRow In tabla.Rows

                factura = New BE.BE_Factura
                factura.Fecha = Convert.ToDateTime(registro("Fecha"))
                factura.Cliente.ID = Convert.ToInt32(registro("ID_Cliente"))
                factura.TipoFactura.ID = Convert.ToInt32(registro("ID_Tipo_Factura"))
                factura.IVA.ID = Convert.ToInt32(registro("ID_IVA"))
                factura.Pedido.ID = Convert.ToInt32(registro("ID_Pedido"))
                factura.PrecioTotal = Convert.ToDecimal(registro("PrecioTotal"))

                facturas.Add(factura)

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
            BE_Bitacora.Descripcion = "Error al listar todas las facturas: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar todas las facturas: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return facturas

    End Function

    Public Function ListarPorFechas(ByVal fdesde As DateTime, ByVal fhasta As DateTime) As List(Of BE.BE_Factura)
        Dim facturas As List(Of BE.BE_Factura) = New List(Of BE.BE_Factura)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim factura As BE.BE_Factura
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            Dim fd As SqlParameter = New SqlParameter("@fDesde", fdesde)
            Dim fh As SqlParameter = New SqlParameter("@fHasta", fhasta)

            parametros.Add(fd)
            parametros.Add(fh)

            Dim tabla As DataTable = DAL_Acceso.Leer("Factura_ListarPorFechas", parametros)

            For Each registro As DataRow In tabla.Rows

                factura = New BE.BE_Factura
                factura.ID = Convert.ToInt32(registro("ID"))
                factura.Fecha = Convert.ToDateTime(registro("Fecha"))
                factura.Cliente.ID = Convert.ToInt32(registro("ID_Cliente"))
                factura.Cliente.Nombre = registro("NombreCliente")
                factura.Cliente.Apellido = registro("ApellidoCliente")
                factura.TipoFactura.ID = Convert.ToInt32(registro("ID_Tipo_Factura"))
                factura.TipoFactura.Tipo = registro("TipoFactura")
                factura.IVA.ID = Convert.ToInt32(registro("ID_IVA"))
                factura.IVA.Nombre = registro("IVA_Nombre")
                factura.Pedido.ID = Convert.ToInt32(registro("ID_Pedido"))
                factura.PrecioTotal = Convert.ToDecimal(registro("PrecioTotal"))

                facturas.Add(factura)

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
            BE_Bitacora.Descripcion = "Error al listar facturas por fechas: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar facturas por fechas: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return facturas

    End Function

    Public Function ObtenerClienteMayorComprador(ByVal fDesde As DateTime, ByVal fHasta As DateTime) As List(Of BE.BE_Factura)
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim facturas As List(Of BE.BE_Factura) = New List(Of BE.BE_Factura)

        Try

            Dim fecDesde As SqlParameter = New SqlParameter("@fDesde", fDesde)
            Dim fecHasta As SqlParameter = New SqlParameter("@fHasta", fHasta)
            parametros.Add(fecDesde)
            parametros.Add(fecHasta)

            Dim dt As DataTable = _dalAcceso.Leer("Factura_ObtenerClienteMayorComprador", parametros)

            If dt.Rows.Count > 0 Then
                For Each registro As DataRow In dt.Rows
                    Dim f As BE.BE_Factura = New BE.BE_Factura
                    If TypeOf (registro("Monto")) Is DBNull Then
                        f.PrecioTotal = 0
                    Else
                        f.PrecioTotal = Convert.ToDecimal(registro("Monto"))
                    End If
                    f.Cliente.ID = Convert.ToInt32(registro("idCliente"))
                    f.Cliente.Nombre = registro("NCli").ToString
                    f.Cliente.Apellido = registro("ApeCli").ToString
                    facturas.Add(f)
                Next
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
            BE_Bitacora.Descripcion = "Error al obtener los clientes que más compraron: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al obtener los clientes que más compraron: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return facturas

    End Function

    Public Function ListarPorID(ByVal f As BE.BE_Factura) As List(Of BE.BE_Factura)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim factura As BE.BE_Factura
        Dim facturas As List(Of BE.BE_Factura) = New List(Of BE.BE_Factura)
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)

        Try

            Dim id As SqlParameter = New SqlParameter("@id", f.ID)

            parametros.Add(id)

            Dim tabla As DataTable = DAL_Acceso.Leer("Factura_ListarPorID", parametros)

            For Each registro As DataRow In tabla.Rows

                factura = New BE.BE_Factura
                factura.ID = Convert.ToInt32(registro("ID"))
                factura.Fecha = Convert.ToDateTime(registro("Fecha"))
                factura.Cliente.ID = Convert.ToInt32(registro("ID_Cliente"))
                factura.Cliente.Nombre = registro("NombreCliente")
                factura.Cliente.Apellido = registro("ApellidoCliente")
                factura.TipoFactura.ID = Convert.ToInt32(registro("ID_Tipo_Factura"))
                factura.TipoFactura.Tipo = registro("TipoFactura")
                factura.IVA.ID = Convert.ToInt32(registro("ID_IVA"))
                factura.IVA.Nombre = registro("IVA_Nombre")
                factura.Pedido.ID = Convert.ToInt32(registro("ID_Pedido"))
                factura.Pedido.Estado.ID = Convert.ToInt32(registro("ID_Estado"))
                factura.PrecioTotal = Convert.ToDecimal(registro("PrecioTotal"))

                facturas.Add(factura)
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
            BE_Bitacora.Descripcion = "Error al obtener factura por ID: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al obtener factura por ID: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return facturas

    End Function


End Class
