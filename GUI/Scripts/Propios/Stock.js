function GenerarOrdenProduccion() {

    var id = document.getElementById("txtProdID");
    var cant = document.getElementById("txtCantidad");

    if (id.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe seleccionar el producto para el cual quiere generar una orden de producción
        lblError.innerHTML = arraySearch("ME_072", deserializeString);
        lblError.style.color = "red";
        return false;
    }

    if (cant.value.trim() === '' ) {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }

    var regex = /^[0-9]{1,3}$/;
    if (!regex.test(cant.value)) {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //El campo "cantidad" debe contener solamente números.
        lblError.innerHTML = arraySearch("ME_046", deserializeString);
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