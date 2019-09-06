Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Public Class DAL_BackupRestore
    Private _usuarioActual As BE.BE_Usuario = BE.BE_SesionActual.ObtenerInstancia.ObtenerUsuarioActual()
    Private _masterConn As String = Configuration.ConfigurationSettings.AppSettings.Get("ConexionMaster").ToString
    Private _backupCmd As String = Configuration.ConfigurationSettings.AppSettings.Get("ScriptBackup").ToString
    Private _restoreCmd As String = Configuration.ConfigurationSettings.AppSettings.Get("ScriptRestore").ToString
    Private _backupsPath As String = Configuration.ConfigurationSettings.AppSettings.Get("BackupsPath").ToString
    Private _conn As SqlConnection
    Public Function RealizarBackup(ByVal usuario As BE.BE_Usuario) As Boolean
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL.DAL_Bitacora = New DAL_Bitacora

        Try

            Dim año As String = DateTime.Now.Year.ToString
            Dim mes As String = DateTime.Now.Month.ToString
            Dim dia As String = DateTime.Now.Day.ToString
            Dim hora As String = DateTime.Now.Hour.ToString
            Dim min As String = DateTime.Now.Minute.ToString
            Dim seg As String = DateTime.Now.Minute.ToString

            If Not Directory.Exists(_backupsPath) Then
                Directory.CreateDirectory(_backupsPath)
            End If

            Dim nombreArchivo As String = usuario.NombreDeUsuario + "_" + _
                                          DateTime.Now.ToString("ddMMyyyy_HHmmss").ToString + ".bak"

            _backupCmd = String.Format(_backupCmd, _backupsPath, nombreArchivo)

            _conn = New SqlConnection(_masterConn)
            Dim cmd As SqlCommand = New SqlCommand(_backupCmd, _conn)
            _conn.Open()
            If _conn.State = ConnectionState.Open Then
                If cmd.ExecuteNonQuery() Then
                    resultado = True
                End If
                _conn.Close()
            End If

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Backup realizado exitosamente."
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
            BE_Bitacora.Descripcion = "Error al resguardar datos: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al resguardar datos: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Finally

            If _conn.State = ConnectionState.Open Then
                _conn.Close()
            End If

        End Try

        Return resultado

    End Function

    Public Function RealizarRestore(ByVal path As String) As Boolean
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim resultado As Boolean
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL.DAL_Bitacora = New DAL_Bitacora

        Try

            _restoreCmd = String.Format(_restoreCmd, _backupsPath, path)
            _conn = New SqlConnection(_masterConn)

            Dim cmd As SqlCommand = New SqlCommand(_restoreCmd, _conn)
            _conn.Open()
            If _conn.State = ConnectionState.Open Then
                If cmd.ExecuteNonQuery() Then
                    resultado = True
                End If
                _conn.Close()
            End If

            If resultado Then
                BE_Bitacora.FechaHora = DateTime.Now
                BE_Bitacora.Usuario = _usuarioActual
                BE_Bitacora.Descripcion = "Restauración de BD exitosa."
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
            BE_Bitacora.Descripcion = "Error al restaurar BD: " & ex1.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al restaurar BD: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        Finally

            If _conn.State = ConnectionState.Open Then
                _conn.Close()
            End If

        End Try

        Return resultado

    End Function
    Public Function ListarBackups() As List(Of String)
        Dim DAL_Acceso As DAL.DAL_Acceso = New DAL.DAL_Acceso
        Dim parametros As List(Of SqlParameter) = New List(Of SqlParameter)
        Dim files As List(Of String) = New List(Of String)
        Dim BE_Bitacora As BE.BE_Bitacora = New BE.BE_Bitacora
        Dim DAL_Bitacora As DAL.DAL_Bitacora = New DAL_Bitacora

        Try

            Dim fileList() As String = Directory.GetFiles(_backupsPath)

            For Each file As String In fileList
                files.Add(file.Replace(_backupsPath, ""))
            Next

        Catch ex As Exception
            BE_Bitacora.FechaHora = DateTime.Now
            BE_Bitacora.Usuario = _usuarioActual
            BE_Bitacora.Descripcion = "Error al listar backups: " & ex.ToString()
            BE_Bitacora.EsError = True
            DAL_Bitacora.ActualizarBitacora(BE_Bitacora)

        End Try

        Return files

    End Function


End Class
