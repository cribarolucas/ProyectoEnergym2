Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_Cliente
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL.DAL_Acceso

    Public Function ObtenerDatosCliente(ByVal cliente As BE.BE_Cliente) As BE.BE_Cliente
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim c As BE.BE_Cliente = New BE.BE_Cliente

        Try
            Dim id As SqlParameter = New SqlParameter("@id", cliente.ID)
            parametros.add(id)

            Dim dt As DataTable = _dalAcceso.Leer("Cliente_ObtenerDatos", parametros)
            If dt.Rows.Count > 0 Then
                For Each registro As DataRow In dt.Rows
                    c.ID = Convert.ToInt32(registro("idCliente"))
                    c.Nombre = registro("NCli").ToString
                    c.Apellido = registro("ApeCli").ToString
                    c.DNI = Convert.ToInt32(registro("DNI"))
                    c.CUIT = Convert.ToInt64(registro("CUIT"))
                    c.Calle = registro("Calle")
                    c.Altura = Convert.ToInt16(registro("Altura"))
                    c.Piso = Convert.ToInt16(registro("Piso"))
                    c.Departamento = registro("Departamento").ToString
                    c.CodigoPostal = Convert.ToInt16(registro("CodigoPostal"))
                    c.Email = registro("Email")
                    c.Telefono = Convert.ToInt64(registro("Telefono"))
                    c.IVA.ID = Convert.ToInt32(registro("idIVA"))
                    c.IVA.Nombre = registro("nombreIVA")
                    c.Provincia.ID = Convert.ToInt32(registro("idProvincia"))
                    c.Provincia.Nombre = registro("nombreProvincia")
                    c.Localidad.ID = Convert.ToInt32(registro("idLocalidad"))
                    c.Localidad.Nombre = registro("nombreLocalidad")
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
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return c

    End Function
    Public Function AgregarCliente(ByVal cliente As BE.BE_Cliente) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim id As SqlParameter = New SqlParameter("@id", cliente.ID)
            Dim nombre As SqlParameter = New SqlParameter("@nombre", cliente.Nombre)
            Dim ape As SqlParameter = New SqlParameter("@apellido", cliente.Apellido)
            Dim dni As SqlParameter = New SqlParameter("@dni", cliente.DNI)
            Dim cuit As SqlParameter = New SqlParameter("@cuit", cliente.CUIT)
            Dim calle As SqlParameter = New SqlParameter("@calle", cliente.Calle)
            Dim altura As SqlParameter = New SqlParameter("@altura", cliente.Altura)
            Dim piso As SqlParameter = New SqlParameter("@piso", cliente.Piso)
            Dim depto As SqlParameter = New SqlParameter("@departamento", cliente.Departamento)
            Dim cp As SqlParameter = New SqlParameter("@codigoPostal", cliente.CodigoPostal)
            Dim email As SqlParameter = New SqlParameter("@email", cliente.Email)
            Dim tel As SqlParameter = New SqlParameter("@telefono", cliente.Telefono)
            Dim iva As SqlParameter = New SqlParameter("@id_iva", cliente.IVA.ID)
            Dim prov As SqlParameter = New SqlParameter("@id_provincia", cliente.Provincia.ID)
            Dim loc As SqlParameter = New SqlParameter("@id_localidad", cliente.Localidad.ID)
            Dim idUsu As SqlParameter = New SqlParameter("@id_usuario", cliente.ID)

            parametros.Add(id)
            parametros.Add(nombre)
            parametros.Add(ape)
            parametros.Add(dni)
            parametros.Add(cuit)
            parametros.Add(calle)
            parametros.Add(altura)
            parametros.Add(piso)
            parametros.Add(depto)
            parametros.Add(cp)
            parametros.Add(email)
            parametros.Add(tel)
            parametros.Add(iva)
            parametros.Add(prov)
            parametros.Add(loc)
            parametros.Add(idUsu)

            resultado = (_dalAcceso.Escribir("Cliente_Agregar", parametros) > 0)
            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Usuario registrado exitosamente."
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
            BE_Bitacora.Descripcion = "Error al registrar usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al registrar usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado
    End Function

    Public Function ModificarCliente(ByVal cliente As BE.BE_Cliente) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try
            Dim id As SqlParameter = New SqlParameter("@id", cliente.ID)
            Dim calle As SqlParameter = New SqlParameter("@calle", cliente.Calle)
            Dim altura As SqlParameter = New SqlParameter("@altura", cliente.Altura)
            Dim piso As SqlParameter = New SqlParameter("@piso", cliente.Piso)
            Dim depto As SqlParameter = New SqlParameter("@departamento", cliente.Departamento)
            Dim cp As SqlParameter = New SqlParameter("@codigoPostal", cliente.CodigoPostal)
            Dim email As SqlParameter = New SqlParameter("@email", cliente.Email)
            Dim tel As SqlParameter = New SqlParameter("@telefono", cliente.Telefono)
            Dim iva As SqlParameter = New SqlParameter("@id_iva", cliente.IVA.ID)
            Dim prov As SqlParameter = New SqlParameter("@id_provincia", cliente.Provincia.ID)
            Dim loc As SqlParameter = New SqlParameter("@id_localidad", cliente.Localidad.ID)

            parametros.Add(id)
            parametros.Add(calle)
            parametros.Add(altura)
            parametros.Add(piso)
            parametros.Add(depto)
            parametros.Add(cp)
            parametros.Add(email)
            parametros.Add(tel)
            parametros.Add(iva)
            parametros.Add(prov)
            parametros.Add(loc)

            resultado = (_dalAcceso.Escribir("Cliente_Modificar", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Usuario registrado exitosamente."
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
            BE_Bitacora.Descripcion = "Error al modificar usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al modificar usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado

    End Function

    Public Function ListarTodos() As List(Of BE.BE_Cliente)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim c As BE.BE_Cliente
        Dim clientes As List(Of BE.BE_Cliente) = New List(Of BE.BE_Cliente)

        Try

            Dim dt As DataTable = _dalAcceso.Leer("Cliente_ListarTodos")
            If dt.Rows.Count > 0 Then
                For Each registro As DataRow In dt.Rows
                    c = New BE.BE_Cliente
                    c.ID = Convert.ToInt32(registro("idCliente"))
                    c.Nombre = registro("NCli").ToString
                    c.Apellido = registro("ApeCli").ToString
                    c.DNI = Convert.ToInt32(registro("DNI"))
                    c.CUIT = Convert.ToInt64(registro("CUIT"))
                    c.Calle = registro("Calle")
                    c.Altura = Convert.ToInt16(registro("Altura"))
                    c.Piso = Convert.ToInt16(registro("Piso"))
                    c.Departamento = registro("Departamento").ToString
                    c.CodigoPostal = Convert.ToInt16(registro("CodigoPostal"))
                    c.Email = registro("Email")
                    c.Telefono = Convert.ToInt64(registro("Telefono"))
                    c.IVA.ID = Convert.ToInt32(registro("idIVA"))
                    c.IVA.Nombre = registro("nombreIVA")
                    c.Provincia.ID = Convert.ToInt32(registro("idProvincia"))
                    c.Provincia.Nombre = registro("nombreProvincia")
                    c.Localidad.ID = Convert.ToInt32(registro("idLocalidad"))
                    c.Localidad.Nombre = registro("nombreLocalidad")
                    clientes.Add(c)
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
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al leer datos del usuario: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)
        End Try

        Return clientes

    End Function

End Class

