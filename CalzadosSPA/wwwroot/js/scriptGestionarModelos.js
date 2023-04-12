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
    var table = document.getElementById("tablaModelos");
    var skuSeleccionado = table.rows[x.rowIndex].cells[0].innerHTML;
    document.getElementById("skuBorrar").value = skuSeleccionado;
    document.getElementById("btnBorrar").innerHTML =`${skuSeleccionado} <i class="fa fa-trash"></i>`;
}

function bindingDinamico() {
    var input, tBody, tr, td, i, j, flag;
    input = document.getElementById('bindingSKU').value;
    tBody = document.getElementById("myTBody");
    tr = tBody.getElementsByTagName('tr');
    flag = false;

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (input == td.textContent) { j = i; flag = true; }
    }

    if (flag == true) {
        document.getElementsByName('SKUBuscado').forEach(function (ele, id) { ele.value = tr[j].getElementsByTagName("td")[0].textContent; })
        document.getElementsByName('DenominacionBuscado').forEach(function (ele, id) { ele.value = tr[j].getElementsByTagName("td")[1].textContent; })
        document.getElementsByName('InferiorReproceso').forEach(function (ele, id) { ele.value = tr[j].getElementsByTagName("td")[2].textContent; })
        document.getElementsByName('SuperiorReproceso').forEach(function (ele, id) { ele.value = tr[j].getElementsByTagName("td")[3].textContent; })
        document.getElementsByName('InferiorObservado').forEach(function (ele, id) { ele.value = tr[j].getElementsByTagName("td")[4].textContent; })
        document.getElementsByName('SuperiorObservado').forEach(function (ele, id) { ele.value = tr[j].getElementsByTagName("td")[5].textContent; })
    } else {
        document.getElementsByName('SKUBuscado').forEach(function (ele, id) { ele.value = 0; })
        document.getElementsByName('DenominacionBuscado').forEach(function (ele, id) { ele.value = ""; })
        document.getElementsByName('InferiorReproceso').forEach(function (ele, id) { ele.value = 0; })
        document.getElementsByName('SuperiorReproceso').forEach(function (ele, id) { ele.value = 0; })
        document.getElementsByName('InferiorObservado').forEach(function (ele, id) { ele.value = 0; })
        document.getElementsByName('SuperiorObservado').forEach(function (ele, id) { ele.value = 0; })
    }
}