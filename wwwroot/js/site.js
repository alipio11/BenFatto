// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#FilterValue').keypress(function (e) {
        if (e.keyCode == 13)
            $('#basic-text1').click();
    });
    $('#FilterType').change(function () {
        var v = $(this).children("option:selected").html();
        if (v === 'Hora') {
            $('#FilterValueHora').show();
            $('#FilterValue').hide();
        } else {
            $('#FilterValueHora').hide();
            $('#FilterValue').show();
        }
    });
});

function Filter() {
    var select = $('#FilterType').children("option:selected").html();;
    if (select === 'Hora') {
        $('#FilterValue').val($("#FilterValueHora option:selected").html());
    }
    var type = $('#FilterType').val();
    var value = $('#FilterValue').val();
    var url = '/Home/Index';
    var F = '?' + type + '=' + value;
    F = url + F;
    F = encodeURI(F);
   window.location.href = F;
}

function Upload() {
    if (!$('#FileUpload').val()) {
        $('#FileUpload').click();
        return;
    }
    var formdata = new FormData();
    formdata.append('file', $('#FileUpload')[0].files[0]);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Home/Upload');
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            location.reload();
        }
    }
}

function Salvar() {
    var formdata = new FormData();
    if ($('#Id').val() !== '')
        formdata.append('Id', $('#Id').val());
    else
        formdata.append('Id', '-1');
    formdata.append('Ip', $('#IP').val());
    formdata.append('User', $('#User').val());
    formdata.append('Datetime', $('#D').val());
    formdata.append('Detail', $('#Detail').val());

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Home/Salvar');
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            location.reload();
        }
    }
}

function Delete() {
    if ($('#Id').val() !== '') {
        var formdata = new FormData();
        formdata.append('Id', $('#Id').val());
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Home/Delete');
        xhr.send(formdata);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                location.reload();
            }
        }
    }
}
