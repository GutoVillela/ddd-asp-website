$(document).ready(function () {

    ReplaceCharForInputText('txtPrice', '.', ',');
    $('#txtPrice').maskMoney({ prefix: '', allowNegative: false, thousands: '', decimal: ',', affixesStay: false });
});

function ReplaceCharForInputText(inputTextId, charToFind, replaceTo) {
    $('#' + inputTextId).val($('#' + inputTextId).val().replace(charToFind, replaceTo));
}