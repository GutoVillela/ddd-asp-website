$(document).ready(function () {
    $('#btnCadastrar').on('click', function () {
        let correctPrice = $('#txtPrice').val().replaceAll(',', '.');
        $('#txtPrice').val(correctPrice);
    });
});