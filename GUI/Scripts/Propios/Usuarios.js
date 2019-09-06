function ValidarAgregar() {

    var nombre = document.getElementById("txtNombre");
    var clave = document.getElementById("txtClave");
    var claveR = document.getElementById("txtClaveR");
    var idiomas = document.getElementById("ddlIdiomas");
    var intFall = document.getElementById("txtIntFall");

    if (nombre.value.trim() === '' || clave.value.trim() === '' ||
        claveR.value.trim() === '' || intFall.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    else {
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
        else {
            var valid = false;
            //Get the gridview object
            var gvPermisos = document.getElementById("gvPermisos");
            //Loop thorugh items
            for (var i = 0; i < gvPermisos.getElementsByTagName("input").length; i++) {
                //Get the object of input type
                var node = gvPermisos.getElementsByTagName("input")[i];
                //check if object is of type checkbox and checked or not
                if (node != null && node.type == "checkbox" && node.checked) {
                    valid = true;
                    break;
                }
            }
            if (!valid) {
                var lblError = document.getElementById("lblError");
                lblError.style.display = 'block';
                var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                var deserializeString = JSON.parse(leyendas.value);
                //Debe seleccionar al menos un permiso.
                lblError.innerHTML = arraySearch("ME_019", deserializeString);
                lblError.style.color = "red";
                return false;
            }
            else {
                var regex = /^[0-9]{1}$/;
                if (!regex.test(intFall.value)) {
                    var lblError = document.getElementById("lblError");
                    lblError.style.display = 'block';
                    var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                    var deserializeString = JSON.parse(leyendas.value);
                    //El campo "intentos fallidos" debe contener solamente números.
                    lblError.innerHTML = arraySearch("ME_077", deserializeString);
                    lblError.style.color = "red";
                    return false;
                }
                else {
                    return true;
                }
            }
        }
    }
}

function ValidarModificar() {

    var id = document.getElementById("txtID");
    var nombre = document.getElementById("txtNombre");
    var clave = document.getElementById("txtClave");
    var claveR = document.getElementById("txtClaveR");
    var idiomas = document.getElementById("ddlIdiomas");
    var intFall = document.getElementById("txtIntFall");

    if (id.value.trim() === '' || nombre.value.trim() === '' ||
        clave.value.trim() === '' || claveR.value.trim() === '' ||
        intFall.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    else {
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
        else {
            if (id.value == 1) {
                var lblError = document.getElementById("lblError");
                lblError.style.display = 'block';
                var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                var deserializeString = JSON.parse(leyendas.value);
                //No está permitido modificar este usuario.
                lblError.innerHTML = arraySearch("ME_030", deserializeString);
                lblError.style.color = "red";
                return false;
            }
            else {
                var esCliente = document.getElementById("hfEsCliente");
                if (esCliente.value == false) {
                    var valid = false;
                    //Get the gridview object
                    var gvPermisos = document.getElementById("gvPermisos");
                    //Loop thorugh items
                    for (var i = 0; i < gvPermisos.getElementsByTagName("input").length; i++) {
                        //Get the object of input type
                        var node = gvPermisos.getElementsByTagName("input")[i];
                        //check if object is of type checkbox and checked or not
                        if (node != null && node.type == "checkbox" && node.checked) {
                            valid = true;
                            break;
                        }
                    }
                    if (!valid) {
                        var lblError = document.getElementById("lblError");
                        lblError.style.display = 'block';
                        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                        var deserializeString = JSON.parse(leyendas.value);
                        //Debe seleccionar al menos un permiso.
                        lblError.innerHTML = arraySearch("ME_019", deserializeString);
                        lblError.style.color = "red";
                        return false;
                    }
                }
                else {
                    var regex = /^[0-9]{1}$/;
                    if (!regex.test(intFall.value)) {
                        var lblError = document.getElementById("lblError");
                        lblError.style.display = 'block';
                        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                        var deserializeString = JSON.parse(leyendas.value);
                        //El campo "intentos fallidos" debe contener solamente números.
                        lblError.innerHTML = arraySearch("ME_077", deserializeString);
                        lblError.style.color = "red";
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
        }
    }
}

function ValidarEliminar() {
    var id = document.getElementById("txtID");
    if (id.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        if (lblError) {
            lblError.style.display = 'block';
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //Debe seleccionar el usuario que quiere eliminar.
            lblError.innerHTML = arraySearch("ME_022", deserializeString);
            lblError.style.color = "red";
            return false;
        }
        else {
            return true;
        }
    }
    else {
        if (id.value == 1) {
            var lblError = document.getElementById("lblError");
            lblError.style.display = 'block';
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //Debe seleccionar el usuario que quiere eliminar.
            lblError.innerHTML = arraySearch("ME_029", deserializeString);
            lblError.style.color = "red";
            return false;
        }
    }
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