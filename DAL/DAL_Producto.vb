Imports System.Data
Imports System.Data.SqlClient

Public Class DAL_Producto
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia().ObtenerUsuarioActual()
    Private _dalAcceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
    Public Function ListarProductos() As List(Of BE.BE_Producto)
        Dim productos As List(Of BE.BE_Producto) = New List(Of BE.BE_Producto)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim producto As BE.BE_Producto

        Try

            Dim tabla As DataTable = _dalAcceso.Leer("Producto_Listar")

            For Each registro As DataRow In tabla.Rows

                producto = New BE.BE_Producto
                producto.ID = Convert.ToInt32(registro("ID_P"))
                producto.Nombre = registro("Nombre").ToString
                producto.Detalle = registro("Detalle").ToString
                producto.Precio = Convert.ToDecimal(registro("Precio"))
                producto.FilePath = registro("FilePath")
                producto.FilePathThumbnail = registro("FilePathThumbnail")
                producto.Alto = Convert.ToDecimal(registro("Alto"))
                producto.Largo = Convert.ToDecimal(registro("Largo"))
                producto.Ancho = Convert.ToDecimal(registro("Ancho"))
                producto.Stock.ID = Convert.ToInt32(registro("ID_S"))
                producto.Stock.Cantidad = Convert.ToInt32(registro("Cantidad"))
                productos.Add(producto)

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
            BE_Bitacora.Descripcion = "Error al listar todos los productos: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar todos los productos: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return productos

    End Function
    Public Function AgregarProducto(ByRef producto As BE.BE_Producto) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim nombre As SqlParameter = New SqlParameter("@nombre", producto.Nombre)
            Dim detalle As SqlParameter = New SqlParameter("@detalle", producto.Detalle)
            Dim precio As SqlParameter = New SqlParameter("@precio", producto.Precio)
            Dim cant As SqlParameter = New SqlParameter("@cant", producto.Stock.Cantidad)
            Dim alto As SqlParameter = New SqlParameter("@alto", producto.Alto)
            Dim largo As SqlParameter = New SqlParameter("@largo", producto.Largo)
            Dim ancho As SqlParameter = New SqlParameter("@ancho", producto.Ancho)
            Dim fileName As SqlParameter = New SqlParameter("@fileName", producto.FileName)
            Dim filePath As SqlParameter = New SqlParameter("@filePath", producto.FilePath)
            Dim filePathThumbnail As SqlParameter = New SqlParameter("@filePathThumbnail", producto.FilePathThumbnail)

            parametros.Add(nombre)
            parametros.Add(detalle)
            parametros.Add(precio)
            parametros.Add(cant)
            parametros.Add(alto)
            parametros.Add(largo)
            parametros.Add(ancho)
            parametros.Add(fileName)
            parametros.Add(filePath)
            parametros.Add(filePathThumbnail)

            resultado = (_dalAcceso.Escribir("Producto_Agregar", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Producto agregado exitosamente."
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
            BE_Bitacora.Descripcion = "Error al agregar producto: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al agregar producto: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado
    End Function
    Public Function ModificarProducto(ByVal producto As BE.BE_Producto) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim id As SqlParameter = New SqlParameter("@id", producto.ID)
            Dim nombre As SqlParameter = New SqlParameter("@nombre", producto.Nombre)
            Dim detalle As SqlParameter = New SqlParameter("@detalle", producto.Detalle)
            Dim precio As SqlParameter = New SqlParameter("@precio", SqlDbType.Decimal)
            precio.Value = producto.Precio
            'Dim cant As SqlParameter = New SqlParameter("@cant", producto.Stock.Cantidad)
            Dim alto As SqlParameter = New SqlParameter("@alto", SqlDbType.Decimal)
            alto.Value = producto.Alto
            Dim largo As SqlParameter = New SqlParameter("@largo", SqlDbType.Decimal)
            largo.Value = producto.Largo
            Dim ancho As SqlParameter = New SqlParameter("@ancho", SqlDbType.Decimal)
            ancho.Value = producto.Ancho
            Dim fileName As SqlParameter = New SqlParameter("@fileName", producto.FileName)
            Dim filePath As SqlParameter = New SqlParameter("@filePath", producto.FilePath)
            Dim filePathThumbnail As SqlParameter = New SqlParameter("@filePathThumbnail", producto.FilePathThumbnail)

            parametros.Add(id)
            parametros.Add(nombre)
            parametros.Add(detalle)
            parametros.Add(precio)
            'parametros.Add(cant)
            parametros.Add(alto)
            parametros.Add(largo)
            parametros.Add(ancho)
            parametros.Add(fileName)
            parametros.Add(filePath)
            parametros.Add(filePathThumbnail)

            resultado = (_dalAcceso.Escribir("Producto_Modificar", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Producto modificado exitosamente."
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
            BE_Bitacora.Descripcion = "Error al modificar producto: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al modificar producto: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado
    End Function
    Public Function EliminarProducto(ByVal producto As BE.BE_Producto) As Boolean
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim resultado As Boolean

        Try

            Dim id As SqlParameter = New SqlParameter("@id", producto.ID)

            parametros.Add(id)

            resultado = (_dalAcceso.Escribir("Producto_Eliminar", parametros) > 0)

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Producto eliminado exitosamente."
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
            BE_Bitacora.Descripcion = "Error al eliminar producto: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al eliminar producto: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return resultado
    End Function

    Public Function ListarMasVendidos(ByVal fDesde As DateTime, ByVal fHasta As DateTime) As List(Of BE.BE_Producto)
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim productos As List(Of BE.BE_Producto) = New List(Of BE.BE_Producto)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim producto As BE.BE_Producto

        Try
            Dim fecDesde As SqlParameter = New SqlParameter("@fDesde", fDesde)
            Dim fecHasta As SqlParameter = New SqlParameter("@fHasta", fHasta)
            parametros.Add(fecDesde)
            parametros.Add(fecHasta)

            Dim tabla As DataTable = _dalAcceso.Leer("Producto_ListarMasVendidos", parametros)

            For Each registro As DataRow In tabla.Rows

                producto = New BE.BE_Producto
                producto.Nombre = registro("Nombre").ToString
                producto.Stock.Cantidad = Convert.ToInt32(registro("Cantidad"))
                productos.Add(producto)

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
            BE_Bitacora.Descripcion = "Error al listar los productos más vendidos: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar los productos más vendidos: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return productos

    End Function

    Public Function ListarStock() As List(Of BE.BE_Producto)
        Dim productos As List(Of BE.BE_Producto) = New List(Of BE.BE_Producto)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL_Bitacora = New DAL_Bitacora
        Dim producto As BE.BE_Producto

        Try

            Dim tabla As DataTable = _dalAcceso.Leer("Producto_ListarStock")

            For Each registro As DataRow In tabla.Rows

                producto = New BE.BE_Producto
                producto.ID = Convert.ToInt32(registro("ID_Producto"))
                producto.Nombre = registro("Nombre").ToString
                producto.Stock.ID = Convert.ToInt32(registro("ID_Stock"))
                producto.Stock.Cantidad = Convert.ToInt32(registro("Cantidad"))
                productos.Add(producto)

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
            BE_Bitacora.Descripcion = "Error al listar el stock de los productos: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar el stock de los productos: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return productos

    End Function

End Class
