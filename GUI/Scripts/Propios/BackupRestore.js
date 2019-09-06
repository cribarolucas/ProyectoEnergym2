function Validar() {
    var rbRestore = document.getElementById("RB_BACKUP")
    if (!rbRestore.checked) {
        var listbox = document.getElementById("lbBackups")
        var listLength = listbox.options.length;
        var cont = 0;
        for (var i = 0; i < listLength; i++) {
            if (listbox.options[i].selected) {
                cont += 1;
                if (cont > 1) {
                    return false;
                    var lblError = document.getElementById("lblError");
                    lblError.style.display = 'block';
                    var leyendas = document.getElementById("hfLeyendasIdiomaActual");
                    var deserializeString = JSON.parse(leyendas.value);
                    //Debe seleccionar solo un archivo de backup.
                    lblError.innerHTML = arraySearch("ME_026", deserializeString);
                    lblError.style.color = "red";
                    return false;
                    break;
                }

            }
        }
        if (cont == 0) {
            var lblError = document.getElementById("lblError");
            lblError.style.display = 'block';
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //Debe seleccionar un archivo de backup.
            lblError.innerHTML = arraySearch("ME_025", deserializeString);
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