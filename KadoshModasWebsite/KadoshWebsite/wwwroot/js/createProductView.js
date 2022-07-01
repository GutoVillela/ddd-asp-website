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

function GenerateBarCode() {
    $.ajax({
        method: 'GET',
        url: generateBarCodeUrl,
        beforeSend: function () {
            ShowBarCodeLoadingSpinner();
        }
    }).done(function (generatedBarCode) {
        $('#BarCode').val(generatedBarCode);
    }).fail(function () {
        swal("Oops!", e.responseText, "error").then(() => {
            $('#BarCode').focus();
        });
    }).always(function () {
        HideBarCodeLoadingSpinner();
    });
}

function ShowBarCodeLoadingSpinner() {
    $('#barCodeLoadingSpinner').show();
}

function HideBarCodeLoadingSpinner() {
    $('#barCodeLoadingSpinner').hide();
}