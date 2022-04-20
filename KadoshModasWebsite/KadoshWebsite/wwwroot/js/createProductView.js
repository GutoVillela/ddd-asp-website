$(document).ready(function () {

    ReplaceCharForInputText('txtPrice', '.', ',');
    $('#txtPrice').maskMoney({ prefix: '', allowNegative: false, thousands: '', decimal: ',', affixesStay: false });

    $('#btnCadastrar').on('click', function () {
        let correctPrice = $('#txtPrice').val().replaceAll('.', '').replaceAll(',', '.');
        $('#txtPrice').val(correctPrice);
    });
});

function ReplaceCharForInputText(inputTextId, charToFind, replaceTo) {
    $('#' + inputTextId).val($('#' + inputTextId).val().replace(charToFind, replaceTo));
}