function Validar() {
    var usuario = document.getElementById("txtUser");
    var contraseña = document.getElementById("txtClave");

    if (usuario.value.trim() === '' || contraseña.value.trim() === '') {
        var label = document.getElementById("lblError");
        label.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        label.innerHTML = arraySearch("ME_018", deserializeString);
        label.style.color = "red";
        return false;
    }
    else {
        return true;
    }
}

function redirigirHaciaLogin() {
    window.location.href = "Login.aspx";
    return true;
}

function arraySearch(nameKey, myArray) {
    for (var i = 0; i < myArray.length; i++) {
        if (myArray[i].Codigo === nameKey) {
            return myArray[i].Descripcion;
        }
    }
}