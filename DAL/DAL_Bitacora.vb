Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Public Class DAL_Bitacora
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia.ObtenerUsuarioActual()
    Public Function ActualizarBitacora(ByVal bitacora As BE.BE_Bitacora) As Boolean
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim swEx As BE.BE_SoftwareException

        Try

            Dim idUsu As SqlParameter = New SqlParameter("@ID_Usuario", bitacora.Usuario.ID)
            Dim fecHora As SqlParameter = New SqlParameter("@fechaHora", bitacora.FechaHora)
            Dim desc As SqlParameter = New SqlParameter("@descripcion", bitacora.Descripcion)
            Dim esError As SqlParameter = New SqlParameter("@esError", bitacora.EsError)

            parametros.Add(idUsu)
            parametros.Add(fecHora)
            parametros.Add(desc)
            parametros.Add(esError)

            resultado = (DAL_Acceso.Escribir("Bitacora_Agregar", parametros) > 0)

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = bitacora.Usuario
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            Me.ActualizarBitacora(BE_Bitacora)
            swEx = New BE.BE_SoftwareException("Cadena de conexión inválida.", ex2, "ERR_906")
        Catch ex1 As SqlClient.SqlException
            swEx = New BE.BE_SoftwareException("Error al actualizar.", ex1, "ERR_901")
            'Falló el guardado en la BD.
            Me.GuardarArchivoTexto(swEx)
        Catch ex As Exception
            swEx = New BE.BE_SoftwareException("Error al actualizar.", ex, "ERR_801")
            'Falló el guardado en la BD.
            Me.GuardarArchivoTexto(swEx)
        End Try

        Return resultado

    End Function

    Public Function LeerBitacora(ByVal bitacora As BE.BE_Bitacora, ByVal fHasta As DateTime) As List(Of BE.BE_Bitacora)
        Dim bitacoras As List(Of BE.BE_Bitacora) = New List(Of BE.BE_Bitacora)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim swEx As BE.BE_SoftwareException

        Try

            Dim idUsu As SqlParameter = New SqlParameter("@ID_Usuario", bitacora.Usuario.ID)
            Dim fecDesde As SqlParameter = New SqlParameter("@fecDesde", bitacora.FechaHora)
            Dim fecHasta As SqlParameter = New SqlParameter("@fecHasta", fHasta)
            Dim esError As SqlParameter = New SqlParameter("@esError", bitacora.EsError)

            parametros.Add(idUsu)
            parametros.Add(fecDesde)
            parametros.Add(fecHasta)
            parametros.Add(esError)

            Dim tabla As DataTable = DAL_Acceso.Leer("Bitacora_Listar", parametros)

            For Each registro As DataRow In tabla.Rows

                Dim b As BE.BE_Bitacora = New BE.BE_Bitacora
                b.ID = Convert.ToInt32(registro("ID_Mensaje"))
                b.FechaHora = Convert.ToDateTime(registro("FechaHora"))
                b.Descripcion = registro("Descripcion")
                b.EsError = Convert.ToBoolean(registro("EsError"))
                b.Usuario = New BE.BE_Usuario
                b.Usuario.NombreDeUsuario = registro("NombreUsuario")

                bitacoras.Add(b)

            Next

        Catch ex2 As ArgumentNullException
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = bitacora.Usuario
            BE_Bitacora.Descripcion = "Cadena de conexión inválida: " & ex2.ToString()
            BE_Bitacora.EsError = True
            Me.ActualizarBitacora(BE_Bitacora)
        Catch ex1 As SqlClient.SqlException
            swEx = New BE.BE_SoftwareException("Error al actualizar.", ex1, "ERR_801")
            'Falló el guardado en la BD.
            Me.GuardarArchivoTexto(swEx)
        Catch ex As Exception
            swEx = New BE.BE_SoftwareException("Error al actualizar.", ex, "ERR_901")
            'Falló el guardado en la BD.
            Me.GuardarArchivoTexto(swEx)
        End Try

        Return bitacoras

    End Function

    Public Sub GuardarArchivoTexto(swEx As BE.BE_SoftwareException)

        Dim sRenglon As String = Nothing
        Dim strStreamW As Stream = Nothing
        Dim strStreamWriter As StreamWriter = Nothing
        Dim ContenidoArchivo As String = Nothing
        ' Donde guardamos los paths de los archivos que vamos a estar utilizando ..
        Dim PathArchivo As String

        Try
            'Si no existe la carpeta, se crea la misma
            If Not Directory.Exists("C:\Users\All Users") Then
                Directory.CreateDirectory("C:\Users\All Users")
            End If

            'Se determina el nombre del archivo con la fecha actual
            PathArchivo = "C:\Users\All Users\" _
                & swEx.Descripcion & " _ " & Format(Date.Now, "yyyy-mm-dd") & ".txt"

            'Verifico si existe el archivo

            If File.Exists(PathArchivo) Then
                'Abro el archivo
                strStreamW = File.Open(PathArchivo, FileMode.Open)
            Else
                'Creo el archivo
                strStreamW = File.Create(PathArchivo)
            End If
            ' Tipo de codificacion para escritura
            strStreamWriter = New StreamWriter(strStreamW, System.Text.Encoding.Default)

            'Escribo en el archivo
            strStreamWriter.WriteLine(Format(Date.Now, "yyyy-mm-dd HH:mm:ss"))

            'strStreamWriter.WriteLine(_usuarioActual.NombreDeUsuario)

            strStreamWriter.WriteLine(swEx.Message)

            If swEx.StackTrace IsNot Nothing Then
                strStreamWriter.WriteLine(swEx.StackTrace)
            Else
                strStreamWriter.WriteLine(swEx.InnerException.StackTrace)
            End If

            'Cierro el archivo
            strStreamWriter.Close()

        Catch ex As Exception
            MsgBox("Error al Guardar la informacion en el archivo. " & ex.ToString, MsgBoxStyle.Critical)
            strStreamWriter.Close() ' cerramos
        End Try

    End Sub

End Class
