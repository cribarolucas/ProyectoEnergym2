function Validar() {

    var nombre = document.getElementById("txtNombre");
    var apellido = document.getElementById("txtApellido");
    var email = document.getElementById("txtEmail");
    var telefono = document.getElementById("txtTelefono");
    var consulta = document.getElementById("txtConsulta");

    if ( nombre.value.trim() === '' || apellido.value.trim() === '' ||
         email.value.trim() === '' || telefono.value.trim() === '' ||
        consulta.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }

    var regex = /^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{1,50}$/;
    if (!regex.test(nombre.value) || !regex.test(apellido.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Los campos "nombre" y "apellido" deben contener solamente letras y como máximo 50 caracteres.
        lblError.innerHTML = arraySearch("ME_041", deserializeString);
        lblError.style.color = "red";
        return false;
    }

    var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!regex.test(email.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Escriba una dirección de correo electrónico con el formato correcto.
        lblError.innerHTML = arraySearch("ME_039", deserializeString);
        lblError.style.color = "red";
        return false;
    }

    var regex = /^[0-9]{1,20}$/;
    if (!regex.test(telefono.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "teléfono" debe contener solamente números y como máximo 20 caracteres.
        lblError.innerHTML = arraySearch("ME_043", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    return true;

}

function arraySearch(nameKey, myArray) {
    for (var i = 0; i < myArray.length; i++) {
        if (myArray[i].Codigo === nameKey) {
            return myArray[i].Descripcion;
        }
    }
}