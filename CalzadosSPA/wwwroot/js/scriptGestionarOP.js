function filtroDinamico() {
    var input, filter, tBody, tr, td, i, txtValue;
    input = document.getElementById('myInput');
    filter = input.value.toUpperCase();
    tBody = document.getElementById("myTBody");
    tr = tBody.getElementsByTagName('tr');

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        txtValue = td.textContent || td.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            tr[i].style.display = "";
        } else {
            tr[i].style.display = "none";
        }
    }
}

function obtenerModeloSeleccionado(x) {
    var modeloSeleccionado = document.getElementById("tablaModelosOP").rows[x.rowIndex].cells[0].innerHTML;
    var descripcion = document.getElementById("tablaModelosOP").rows[x.rowIndex].cells[1].innerHTML;
    document.getElementById("modeloSelect").value = modeloSeleccionado;
    document.getElementById("modeloTag").innerHTML = `${descripcion}`;
}

function obtenerColorSeleccionado() {
    var lista = document.getElementById("coloresList");
    var descripcion = lista.options[lista.selectedIndex].text;
    document.getElementById("colorSelect").value = descripcion;
}

function obtenerLineaSeleccionada() {
    var lista = document.getElementById("lineasList");
    var numero = lista.options[lista.selectedIndex].text;
    document.getElementById("lineaSelect").value = numero;
}