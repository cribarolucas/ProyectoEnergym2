function Validar() {
    var nomUsu = document.getElementById("txtNomUsu");
    var clave = document.getElementById("txtClave");
    var claveR = document.getElementById("txtClaveR");
    var nombre = document.getElementById("txtNombre");
    var apellido = document.getElementById("txtApellido");
    var dni = document.getElementById("txtDNI");
    var cuit = document.getElementById("txtCUIT");
    var calle = document.getElementById("txtCalle");
    var altura = document.getElementById("txtAltura");
    var codPos = document.getElementById("txtCodPos");
    var email = document.getElementById("txtEmail");
    var telefono = document.getElementById("txtTelefono");

    if (nomUsu.value.trim() === '' || clave.value.trim() === '' ||
        claveR.value.trim() === '' || nombre.value.trim() === '' ||
        //apellido.value.trim() === '' || dni.value.trim() === '' ||
        cuit.value.trim() === '' || calle.value.trim() === '' ||
        altura.value.trim() === '' || codPos.value.trim() === '' ||
        email.value.trim() === '' || telefono.value.trim() === '') {
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
    if (clave.value != claveR.value) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Las claves elegidas deben ser iguales.
        lblError.innerHTML = arraySearch("ME_038", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var regex = /^[0-9]{1,9}$/;
    if (!regex.test(dni.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "DNI" debe contener solamente números y como máximo 9 caracteres.
        lblError.innerHTML = arraySearch("ME_040", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var regex = /^[0-9]{1,11}$/;
    if (!regex.test(cuit.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "CUIT" debe contener solamente números y como máximo 11 caracteres.
        lblError.innerHTML = arraySearch("ME_047", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var CUIT2 = CUIT.value.toString().substr(2, 9)
    if (dni.value != CUIT2 && dni.value.trim() != '') {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "CUIT" debe ser igual al campo "DNI"
        lblError.innerHTML = arraySearch("ME_099", deserializeString);
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
    var regex = /^[0-9]{1,5}$/;
    if (!regex.test(altura.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "altura" debe contener solamente números y como máximo 5 caracteres.
        lblError.innerHTML = arraySearch("ME_042", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var regex = /^[0-9]{1,5}$/;
    if (!regex.test(codPos.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "código postal" debe contener solamente números y como máximo 5 caracteres.
        lblError.innerHTML = arraySearch("ME_060", deserializeString);
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

function ActualizarClave() {
    var claveActual = document.getElementById("txtClave")
    var claveNueva = document.getElementById("txtClaveNew")
    var claveNuevaR = document.getElementById("txtClaveNewR")

    if (claveActual.value.trim() === '' ||
        claveNueva.value.trim() === '' ||
        claveNuevaR.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }

    if (claveActual.value == claveNueva.value) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //La nueva clave debe ser distinta a la actual.
        lblError.innerHTML = arraySearch("ME_054", deserializeString);
        lblError.style.color = "red";
        return false;
    }

    if (claveNueva.value !== claveNuevaR.value) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Las claves nuevas no coinciden entre sí.
        lblError.innerHTML = arraySearch("ME_055", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var regex = /^[0-9A-Za-z¡!@.,;:'"<>¿?_-]{1,32}$/;
    if (!regex.test(claveNueva.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //La clave debe contener 32 caracteres como máximo.
        lblError.innerHTML = arraySearch("ME_056", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    return true;
}

function ValidarActualizar() {
    var nomUsu = document.getElementById("txtNomUsu")
    var calle = document.getElementById("txtCalle")
    var altura = document.getElementById("txtAltura")
    var codPos = document.getElementById("txtCodPos")
    var email = document.getElementById("txtEmail")
    var telefono = document.getElementById("txtTelefono")

    if (nomUsu.value.trim() === '' || calle.value.trim() === '' ||
        altura.value.trim() === '' || codPos.value.trim() === '' ||
        email.value.trim() === '' || telefono.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var regex = /^[0-9]{1,5}$/;
    if (!regex.test(altura.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "altura" debe contener solamente números y como máximo 5 caracteres.
        lblError.innerHTML = arraySearch("ME_042", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var regex = /^[0-9]{1,5}$/;
    if (!regex.test(codPos.value)) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "código postal" debe contener solamente números y como máximo 5 caracteres.
        lblError.innerHTML = arraySearch("ME_060", deserializeString);
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

$(document).ready(function () {
    $(".chosen-select").chosen();
})