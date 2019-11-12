function ValidarAgregar() {

    var monto = document.getElementById("txtMonto");

    if (monto.value.trim() == '')
    {
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
    var regex = /^((\d{1,10})((\,\d{0,2})?))$/;
    if (!regex.test(monto.value))
    {
        var lblError = document.getElementById("lblError");
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Por favor, complete el campo "monto" con el formato correcto: "00000.00".
        lblError.innerHTML = arraySearch("ME_100", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    return true;
    }