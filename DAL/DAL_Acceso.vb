Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Friend Class DAL_Acceso

    Private _tx As SqlTransaction
    Private _cn As SqlConnection
    Private _strConn As String = Configuration.ConfigurationSettings.AppSettings.Get("ConexionBD").ToString

    Public Sub CrearTransaccion()
        _tx = _cn.BeginTransaction
    End Sub
    Public Sub RollBack()
        _tx.Rollback()
    End Sub
    Public Sub Commit()
        _tx.Commit()
    End Sub
    Public Sub Conectar()
        _cn = New SqlConnection(_strConn)
        _cn.Open()
    End Sub
    Public Sub Desconectar()
        _cn.Close()
        _cn = Nothing
    End Sub
    Private Function Crearcomando(ByVal nombre As String, Optional ByVal parametros As List(Of SqlParameter) = Nothing) As SqlCommand
        Dim comando As New SqlCommand(nombre, _cn)
        comando.CommandType = CommandType.StoredProcedure
        If _tx IsNot Nothing Then
            comando.Transaction = _tx
        End If
        If parametros IsNot Nothing Then
            comando.Parameters.AddRange(parametros.ToArray())
        End If
        Return comando
    End Function

    Public Function Leer(ByVal nombre As String, Optional ByVal parametros As List(Of SqlParameter) = Nothing) As DataTable
        Me.Conectar()
        Dim tabla As New DataTable
        Dim da As New SqlDataAdapter
        da.SelectCommand = Crearcomando(nombre, parametros)
        da.Fill(tabla)
        Me.Desconectar()
        Return tabla
    End Function
    Public Function LeerEscalar(ByVal nombre As String, Optional ByVal parametros As List(Of SqlParameter) = Nothing) As Object
        Me.Conectar()
        Dim obj As Object
        obj = Crearcomando(nombre, parametros).ExecuteScalar
        Me.Desconectar()
        Return obj
    End Function

    Public Function Escribir(ByVal nombre As String, Optional ByVal parametros As List(Of SqlParameter) = Nothing) As Integer
        Dim res As Integer
        Me.Conectar()
        Try
            Me.CrearTransaccion()
            res = Crearcomando(nombre, parametros).ExecuteNonQuery
            Me.Commit()
        Catch ex As Exception
            res = -1
            Me.RollBack()
        End Try
        Me.Desconectar()
        Return res
    End Function
End Class
