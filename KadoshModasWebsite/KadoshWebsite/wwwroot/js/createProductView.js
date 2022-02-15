$(document).ready(function () {

    $('#txtPrice').maskMoney({ prefix: '', allowNegative: false, thousands: '', decimal: ',', affixesStay: false });

    $('#btnCadastrar').on('click', function () {
        let correctPrice = $('#txtPrice').val().replaceAll('.', '').replaceAll(',', '.');
        $('#txtPrice').val(correctPrice);
    });
});