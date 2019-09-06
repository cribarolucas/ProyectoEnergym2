function OcultarLabelLogin()
{
    var usuario = '<%= Session["Usuario_Conectado"] %>';
    var lblLogin = document.getElementById('L_LOGIN');
    var lblLogout = document.getElementById('L_LOGOUT');
    lblLogin.style.display = 'none';
    lblLogout.style.display = 'block';
    return true;
}

function OcultarabelLogout() {
    var usuario = '<%= Session["Usuario_Conectado"] %>';
    var lblLogout = document.getElementById('L_LOGOUT');
    var lblLogin = document.getElementById('L_LOGIN');
    lblLogin.style.display = 'block';
    lblLogout.style.display = 'none';
    return true;
}