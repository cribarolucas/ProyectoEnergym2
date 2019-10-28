function ValidarRealizarPedido() {
    if (!ValidarActualizar()) {
        return false;
    }
    else {
        var gridview = document.getElementById("gvCarrito");
        if (gridview.rows.length < 1) {
            var lblError = document.getElementById("lblError");
            lblError.style.display = 'block';
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //Debe agregar al menos un producto al carrito.
            lblError.innerHTML = arraySearch("ME_049", deserializeString);
            lblError.style.color = "red";
            return false;
        }

        var usuario = document.getElementById("hfUsuarioConectado");
        var deserializeString = JSON.parse(usuario.value);
        if (deserializeString === null) {
            var lblError = document.getElementById("lblError");
            lblError.style.display = 'block';
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //Para realizar pedido debe iniciar sesión.
            lblError.innerHTML = arraySearch("ME_050", deserializeString);
            lblError.style.color = "red";
            return false;
        }
        return true;
    }
}

function ValidarActualizar() {
    var regex = /^[0-9]{1,3}$/;
    var gridview = document.getElementById("gvCarrito");
    var currentRow;

    //Recorro hasta la fila 5 inclusive, ya que esa es la cantidad de registros
    // que se muestran en pantalla.
    if (gridview.rows.length > 0) {
        //Empiezo desde 1 porque no tengo que evaluar el encabezado de la grilla
        for (i = 1; i < gridview.rows.length - 1; i++) {
            currentRow = gridview.rows[i].cells[2];
            if (currentRow.childNodes.length > 1) {
                if (currentRow.childNodes[1].value.trim() === '') {
                    var lblError = document.getElementById("lblError");
                    lblError.style.display = 'block';
                    var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                    var deserializeString = JSON.parse(leyendas.value);
                    //Debe completar el campo "cantidad"
                    lblError.innerHTML = arraySearch("ME_051", deserializeString);
                    lblError.style.color = "red";
                    return false;
                }
                if (currentRow.childNodes[1].value < 1) {
                    var lblError = document.getElementById("lblError");
                    lblError.style.display = 'block';
                    var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                    var deserializeString = JSON.parse(leyendas.value);
                    //El campo "Cantidad" debe contener un número mayor a "0".
                    lblError.innerHTML = arraySearch("ME_066", deserializeString);
                    lblError.style.color = "red";
                    return false;
                }
                if (!regex.test(currentRow.childNodes[1].value)) {
                    var lblError = document.getElementById("lblError");
                    lblError.style.display = 'block';
                    var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                    var deserializeString = JSON.parse(leyendas.value);
                    //El campo "cantidad" debe contener solamente números positivos.
                    lblError.innerHTML = arraySearch("ME_046", deserializeString);
                    lblError.style.color = "red";
                    return false;
                }
            }
        }
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