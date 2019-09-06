function ValidarAgregarModificarIdioma() {
    var nombre = document.getElementById("txtIdiomaNombre");
    if (nombre.value.trim() === '') {
        var lblError = document.getElementById("lblErrorI");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    else {
        return true;
    }
}

function ValidarModificarLeyenda() {
    var leyendaID = document.getElementById("txtLeyID");
    var desc = document.getElementById("txtLeyDesc");
    if (leyendaID.value.trim() === '') {
        var lblError = document.getElementById("lblErrorL");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe seleccionar la leyenda que quiere modificar.
        lblError.innerHTML = arraySearch("ME_044", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    if (desc.value.trim() === '') {
        var lblError = document.getElementById("lblErrorL");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe seleccionar la leyenda que quiere modificar.
        lblError.innerHTML = arraySearch("ME_045", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    return true;
}

function ValidarEliminarIdioma() {
    var ddl = document.getElementById("ddlIdioma");
    var codigo = ddl.options[ddl.selectedIndex].value;
    if (codigo == "es-AR" || codigo == "en-GB") {
        var lblError = document.getElementById("lblErrorI");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //No está permitido eliminar este idioma.
        lblError.innerHTML = arraySearch("ME_031", deserializeString);
        lblError.style.color = "red";
        return false;
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