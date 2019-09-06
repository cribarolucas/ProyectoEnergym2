Public Class SEG_GestorIntegridad

    Dim gestorCifrado As SEG_GestorCifrado = New SEG_GestorCifrado
    Dim DAL_GestorIntegridad As DAL.DAL_GestorIntegridad = New DAL.DAL_GestorIntegridad

    Public Function VerificarIntegridad() As Boolean
        Dim resultado As Boolean
        Dim SEG_Usuario As SEG_Usuario = New SEG_Usuario
        Dim SEG_Permiso As SEG_Permiso = New SEG_Permiso


        If (Me.listarDVV("Permiso") = CalcularDVV("Permiso")) AndAlso
            (Me.listarDVV("Usuario") = CalcularDVV("Usuario")) Then

            If (Me.VerificarDVH(SEG_Permiso.ListarTodoString(), Me.listarDVH("Permiso"))) AndAlso
                (Me.VerificarDVH(SEG_Usuario.ListarTodoString(), Me.listarDVH("Usuario"))) Then

                resultado = True

            End If

        End If

        Return resultado

    End Function

    Private Function listarDVV(tabla As String) As String
        Return DAL_GestorIntegridad.listarDVV(tabla)
    End Function

    Public Function CalcularDVV(tabla As String) As String
        Dim dvv As String
        Dim SEG_GestorCifrado As SEG_GestorCifrado = New SEG_GestorCifrado
        Dim listadoDVH As List(Of String) = Me.listarDVH(tabla)

        For i = 0 To listadoDVH.Count - 1
            dvv += listadoDVH.Item(i)
        Next

        If dvv IsNot Nothing Then
            dvv = SEG_GestorCifrado.GetHashMD5(dvv)
        End If

        Return dvv
    End Function
    Public Sub ActualizarDVV(tabla As String)
        DAL_GestorIntegridad.ActualizarDVV(tabla, Me.CalcularDVV(tabla))
    End Sub
    Private Function listarDVH(tabla As String) As List(Of String)
        Return DAL_GestorIntegridad.ListarDVH(tabla)
    End Function

    Private Function VerificarDVH(listadoPropiedades As List(Of String), listadoDVH As List(Of String)) As Boolean
        Dim SEG_GestorCifrado As SEG_GestorCifrado = New SEG_GestorCifrado
        Dim resultado As Boolean = True
        For i = 0 To listadoPropiedades.Count - 1
            If Not SEG_GestorCifrado.CompareHashMD5(listadoPropiedades.Item(i), listadoDVH.Item(i)) Then
                Dim hash As String = SEG_GestorCifrado.GetHashMD5(listadoPropiedades.Item(i))
                resultado = False
            End If
        Next
        Return resultado

    End Function

End Class
