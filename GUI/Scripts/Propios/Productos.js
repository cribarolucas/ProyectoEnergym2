function ValidarAgregar() {

    var nombre = document.getElementById("txtNombre");
    var detalle = document.getElementById("txtDetalle");
    var precio = document.getElementById("txtPrecio");
    var path = document.getElementById("fuProducto");
    var alto = document.getElementById("txtAlto");
    var ancho = document.getElementById("txtAncho");
    var largo = document.getElementById("txtLargo");
    var cant = document.getElementById("txtCant");

    if (nombre.value.trim() === '' || detalle.value.trim() === '' ||
        precio.value.trim() == '' || path.value.trim() === '' ||
        alto.value.trim() === '' || ancho.value.trim() === '' ||
        largo.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    //Validar que los valores ingresados en los
    //campos numéricos tengan el formato correcto
    var regex = /^((\d{1,5})((\,\d{0,2})?))$/;
    if (!regex.test(precio.value) ||
        !regex.test(ancho.value) ||
        !regex.test(alto.value) ||
        !regex.test(largo.value)) {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Por favor, complete los campos "precio", "alto", "largo" y "ancho" con el formato correcto: "00000.00".
        lblError.innerHTML = arraySearch("ME_036", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var regex = /^\.|\.jpe?g$/i;
    if (!regex.test(path.value)) {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Por favor, elija una imagen cuya extensión sea ".jpg" o ".jpeg".
        lblError.innerHTML = arraySearch("ME_037", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    //var regex = /^[0-9]{1,3}$/;
    //if (!regex.test(cant.value)) {
    //    var lblError = document.getElementById("lblError");
    //    var leyendas = document.getElementById("hfLeyendasIdiomaActual");
    //    var deserializeString = JSON.parse(leyendas.value);
    //    //El campo "cantidad" debe contener solamente números y como máximo 3 caracteres.
    //    lblError.innerHTML = arraySearch("ME_046", deserializeString);
    //    lblError.style.color = "red";
    //    return false;
    //}
    return true;
}

function ValidarModificar() {

    var id = document.getElementById("txtID");
    var nombre = document.getElementById("txtNombre");
    var detalle = document.getElementById("txtDetalle");
    var precio = document.getElementById("txtPrecio");
    var path = document.getElementById("fuProducto");
    var alto = document.getElementById("txtAlto");
    var ancho = document.getElementById("txtAncho");
    var largo = document.getElementById("txtLargo");
    var cant = document.getElementById("txtCant");

    if (id.value.trim() === '') {
        //Debe seleccionar el producto que desea modificar.
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_035", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    if (nombre.value.trim() === '' || detalle.value.trim() === '' ||
        precio.value.trim() == '' || path.value.trim() === '' ||
        alto.value.trim() === '' || ancho.value.trim() === '' ||
        largo.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe completar todos los campos.
        lblError.innerHTML = arraySearch("ME_018", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    //Validar que los valores ingresados en los
    //campos numéricos tengan el formato correcto
    var regex = /^((\d{1,5})((\,\d{0,2})?))$/;
    if (!regex.test(precio.value) ||
        !regex.test(ancho.value) ||
        !regex.test(alto.value) ||
        !regex.test(largo.value)) {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Por favor, complete los campos "precio", "alto", "largo" y "ancho" con el formato correcto: "00000.00".
        lblError.innerHTML = arraySearch("ME_036", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    var regex = /^\.|\.jpe?g$/i;
    if (!regex.test(path.value)) {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Por favor, elija una imagen cuya extensión sea ".jpg" o ".jpeg".
        lblError.innerHTML = arraySearch("ME_037", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    //var regex = /^[0-9]{1,3}$/;
    //if (!regex.test(cant.value)) {
    //    var lblError = document.getElementById("lblError");
    //    var leyendas = document.getElementById("hfLeyendasIdiomaActual");
    //    var deserializeString = JSON.parse(leyendas.value);
    //    //El campo "cantidad" debe contener solamente números y como máximo 3 caracteres.
    //    lblError.innerHTML = arraySearch("ME_046", deserializeString);
    //    lblError.style.color = "red";
    //    return false;
    //}
    return true;
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