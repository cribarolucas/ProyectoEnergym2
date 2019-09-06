function ValidarAgregar() {

    var nombre = document.getElementById("txtNombre");

    if (nombre.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
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
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //Debe seleccionar al menos un permiso.
            lblError.innerHTML = arraySearch("ME_019", deserializeString);
            lblError.style.color = "red";
            return false;
        }
        else {
            return true;
        }
    }
}

function ValidarModificar() {

    var id = document.getElementById("txtID");
    var nombre = document.getElementById("txtNombre");

    if (id.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe seleccionar el perfil que desea modificar.
        lblError.innerHTML = arraySearch("ME_021", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    else {
        if (nombre.value.trim() === '') {
            var lblError = document.getElementById("lblError");
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //Debe completar todos los campos.
            lblError.innerHTML = arraySearch("ME_018", deserializeString);
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
                var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                var deserializeString = JSON.parse(leyendas.value);
                //Debe seleccionar al menos un permiso.
                lblError.innerHTML = arraySearch("ME_019", deserializeString);
                lblError.style.color = "red";
                return false;
            }
            else {
                return true;
            }
        }
    }
}

function ValidarEliminar() {
    var id = document.getElementById("txtID");
    var lblError = document.getElementById("lblError");
    if (id.value.trim() === '') {
        if (lblError) {
            lblError.style.display = 'block';
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //Por favor, seleccione el perfil que desea eliminar
            lblError.innerHTML = arraySearch("ME_020", deserializeString);
            lblError.style.color = "red";
            return false;
        }
        else {
            return true;
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