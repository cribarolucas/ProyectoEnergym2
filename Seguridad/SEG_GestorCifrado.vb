Imports System
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Security
Imports System.Security.Cryptography
Public Class SEG_GestorCifrado

    '/// <summary>
    '/// Hashes a text using MD5 algorithm
    '/// </summary>
    '/// <param name="value">text to hash</param>
    '/// <returns>hashed text (Base64String)</returns>
    Public Function GetHashMD5(ByVal value As String) As String

        Dim Ue As UnicodeEncoding = New UnicodeEncoding()
        Dim ByteSourceText As Byte() = Ue.GetBytes(value)
        Dim Md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
        '//obtener el valor del hash value desde el origen (byteSurceText)
        Dim ByteHash As Byte() = Md5.ComputeHash(ByteSourceText)
        Md5.Clear()
        '//retornarlo como un string
        Return Convert.ToBase64String(ByteHash)

    End Function

    '/// <summary>
    '/// Compares a Hash agains a non hashed value
    '/// </summary>
    '/// <param name="value">value to evaluate</param>
    '/// <param name="hash">hash to evaluate against</param>
    '/// <returns>True if the hash matches</returns>
    Public Function CompareHashMD5(ByVal value As String, ByVal hash As String) As Boolean

        Return (hash.Equals(Me.GetHashMD5(value)))

    End Function

    '/// <summary>
    '/// Cypher a text with TripleDES algorithm
    '/// </summary>
    '/// <param name="toEncrypt">text to cypher</param>
    '/// <param name="key">key to cypher</param>
    '/// <param name="useHashing">true if the key must be hashed (more secure)</param>
    '/// <returns>cypher text</returns>
    Public Function CypherTripleDES(ByVal toEncrypt As String, ByVal key As String, ByVal useHashing As Boolean) As String
        Dim r As String
        Dim keyArray As Byte()
        Dim toEncryptArray As Byte() = UTF8Encoding.UTF8.GetBytes(toEncrypt)

        Dim settingsReader As AppSettingsReader = New AppSettingsReader()

        '//System.Windows.Forms.MessageBox.Show(key);
        '//If hashing use get hashcode regards to your key
        If (useHashing) Then

            Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key))
            '//Always release the resources and flush data
            '// of the Cryptographic service provide. Best Practice
            hashmd5.Clear()

        Else

            keyArray = UTF8Encoding.UTF8.GetBytes(key)
        End If
        Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
        '//set the secret key for the tripleDES algorithm
        tdes.Key = keyArray
        '//mode of operation. there are other 4 modes.
        '//We choose ECB(Electronic code Book)
        tdes.Mode = CipherMode.ECB
        '//padding mode(if any extra byte added)
        tdes.Padding = PaddingMode.PKCS7
        Dim cTransform As ICryptoTransform = tdes.CreateEncryptor()
        '//transform the specified region of bytes array to resultArray
        Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
        '//Release resources held by TripleDes Encryptor
        tdes.Clear()
        '//Return the encrypted data into unreadable string format
        r = Convert.ToBase64String(resultArray, 0, resultArray.Length)
        Return Convert.ToBase64String(resultArray, 0, resultArray.Length)

    End Function

    '/// <summary>
    '/// Decypher a text using the TripleDES algorithm
    '/// </summary>
    '/// <param name="toEncrypt">text to decypher</param>
    '/// <param name="key">key to decypher</param>
    '/// <param name="useHashing">true if the key must be hashed (more secure)</param>
    '/// <returns>clear text</returns>
    Public Function DecypherTripleDES(ByVal cipherString As String, ByVal key As String, ByVal useHashing As Boolean) As String

        Dim strResult As String = cipherString
        Try

            Dim keyArray As Byte()
            '//get the byte code of the string
            Dim toEncryptArray As Byte() = Convert.FromBase64String(cipherString)
            If useHashing Then

                '//if hashing was used get the hash code with regards to your key
                Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key))
                '//release any resource held by the MD5CryptoServiceProvider
                hashmd5.Clear()

            Else

                '//if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key)

            End If

            Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
            '//set the secret key for the tripleDES algorithm
            tdes.Key = keyArray
            '//mode of operation. there are other 4 modes. 
            '//We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB
            '//padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7
            Dim cTransform As ICryptoTransform = tdes.CreateDecryptor()
            Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
            '//Release resources held by TripleDes Encryptor                
            tdes.Clear()
            '//return the Clear decrypted TEXT
            strResult = UTF8Encoding.UTF8.GetString(resultArray)

        Catch ex As Exception
            Dim str = ex.Message
            '//the string may not be a base64 string
        End Try
        Return strResult

    End Function

End Class
