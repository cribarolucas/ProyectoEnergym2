Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Public Class SEG_Consulta

    Public Function EnviarCorreo(ByVal consulta As BE.BE_Consulta) As Boolean
        Dim resultado As Boolean = True
        Dim SEG_GestorCifrado As Seguridad.SEG_GestorCifrado = New SEG_GestorCifrado
        Dim remitente As String = ConfigurationSettings.AppSettings.Get("email-usuario").ToString
        Dim contraseña As String = SEG_GestorCifrado.DecypherTripleDES( _
                                ConfigurationSettings.AppSettings.Get("email-contraseña").ToString, "123456789", True)
        Dim asunto As String = ConfigurationSettings.AppSettings.Get("email-asunto").ToString
        Dim mensajeCuerpo As String = consulta.Consulta & vbCrLf & "Enviado por: " & consulta.Nombre & " " & _
                                      consulta.Apellido & ", Telefono: " & consulta.Telefono.ToString & _
                                      ", Email: " & consulta.Email

        Try
            Dim mensaje As MailMessage = New MailMessage()
            Dim smtp As SmtpClient = New SmtpClient()
            mensaje.From = New MailAddress(remitente)
            mensaje.To.Add(New MailAddress(remitente))
            mensaje.Subject = asunto
            mensaje.Body = mensajeCuerpo
            mensaje.IsBodyHtml = False

            smtp.Host = ConfigurationSettings.AppSettings.Get("smtphost").ToString
            smtp.Port = Convert.ToInt16(ConfigurationSettings.AppSettings.Get("smtport").ToString)
            smtp.EnableSsl = ConfigurationSettings.AppSettings.Get("smtpEnableSsl").ToString
            smtp.Credentials = New NetworkCredential(remitente, contraseña)
            smtp.Send(mensaje)

        Catch ex As SmtpException
            resultado = False
        End Try

        Return resultado

    End Function

End Class
