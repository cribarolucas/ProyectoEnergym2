function Validar() {
    var txtDateFrom = document.getElementById("txtFecDesde");
    var txtDateTo = document.getElementById("txtFecHasta");
    var dateFrom = document.getElementById("hfDateFrom");
    var dateTo = document.getElementById("hfDateTo");
    dateFrom.value = txtDateFrom.value;
    dateTo.value = txtDateTo.value;
    if (dateFrom.value.trim() === '' || dateTo.value.trim() === '') {
        var lblError = document.getElementById("lblError");
        lblError.style.display = 'block';
        var leyendas = document.getElementById("hfLeyendasIdiomaActual");
        var deserializeString = JSON.parse(leyendas.value);
        //Debe seleccionar las fechas "desde y "hasta".
        lblError.innerHTML = arraySearch("ME_024", deserializeString);
        lblError.style.color = "red";
        return false;
    }
    else {
        var from = dateFrom.value.split("/");
        var df = new Date(from[2], from[1] - 1, from[0]);
        var to = dateTo.value.split("/");
        var dt = new Date(to[2], to[1] - 1, to[0]);
        if (df > dt) {
            var lblError = document.getElementById("lblError");
            lblError.style.display = 'block';
            var leyendas = document.getElementById("hfLeyendasIdiomaActual");
            var deserializeString = JSON.parse(leyendas.value);
            //"Fecha desde" debe ser menor o igal a "Fecha hasta"
            lblError.innerHTML = arraySearch("ME_023", deserializeString);
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

$(document).ready(function () {
    var txtDateFrom = document.getElementById("txtFecDesde");
    var txtDateTo = document.getElementById("txtFecHasta");
    var dateFrom = document.getElementById("hfDateFrom");
    var dateTo = document.getElementById("hfDateTo");
    txtDateFrom.value = dateFrom.value;
    txtDateTo.value = dateTo.value;
    $('#txtFecDesde').datepicker({
        dateFormat: 'dd/mm/yy',
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo',
            'Junio', 'Julio', 'Agosto', 'Septiembre',
            'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr',
            'May', 'Jun', 'Jul', 'Ago',
            'Sep', 'Oct', 'Nov', 'Dic']
    });
    $('#txtFecHasta').datepicker({
        dateFormat: 'dd/mm/yy',
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo',
            'Junio', 'Julio', 'Agosto', 'Septiembre',
            'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr',
            'May', 'Jun', 'Jul', 'Ago',
            'Sep', 'Oct', 'Nov', 'Dic']
    });
})