Imports System.Text
Public Class SEG_BackupRestore
    Private DAL_BackupRestore As DAL.DAL_BackupRestore = New DAL.DAL_BackupRestore
    Private _directorioDestino As String
    Public Property DirectorioDestino() As String
        Get
            Return _directorioDestino
        End Get
        Set(ByVal value As String)
            _directorioDestino = value
        End Set
    End Property

    Public Function RealizarBackup(ByVal usuario As BE.BE_Usuario) As Boolean
        Return DAL_BackupRestore.RealizarBackup(usuario)
    End Function

    Public Function RealizarRestore(ByVal path As String) As Boolean
        Return DAL_BackupRestore.RealizarRestore(path)
    End Function

    Public Function ListarBackups() As List(Of String)
        Return DAL_BackupRestore.ListarBackups()
    End Function
End Class
