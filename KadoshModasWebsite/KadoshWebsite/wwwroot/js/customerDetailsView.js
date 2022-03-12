window.addEventListener("load", function () {
    CheckMaskDependencie();
});

function InformCustomerPayment(customerId) {
    
    swal({
        text: 'Informar pagamento na ficha do cliente',
        content: CreateInputForPriceMasked(),
        buttons: {
            cancel: "Cancelar",
            ok: {
                text: "Informar pagamento!",
                closeModal: false,
            }
        },
    })
    .then(buttonClicked => {
        if (!buttonClicked) throw null;
        let value = GetValueFromInputAsFloat($('#payment-inform-input'));
        $.ajax({
            method: 'POST',
            url: informPaymentUrl,
            data: { customerId: customerId, amountToInform: value }
        }).done(function (response) {
            swal("Pagamento informado!", response + ". A página será recarregada.", "success").then(() => location.reload());
        }).fail(function (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível informar o pagamento! Mensagem de erro: " + error.responseText, "error");
        });
    })
    .catch(error => {
        if (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível informar o pagamento! Mensagem de erro: " + error, "error");
        } else {
            swal.stopLoading();
            swal.close();
        }
    });
}

function CreateInputForPriceMasked() {
    var valueInput = document.createElement("input");
    valueInput.type = "text";
    valueInput.id = "payment-inform-input";
    valueInput.className = "form-control";
    valueInput.placeholder = "Valor a ser lançado na ficha";
    $(valueInput).maskMoney({ prefix: '', allowNegative: false, thousands: '', decimal: ',', affixesStay: false });

    return valueInput;
}

function CheckMaskDependencie() {
    if (typeof $().maskMoney != "function")
        console.error('The script "jquery.maskMoney.min.js" is missing on the parent View.');
}

function GetValueFromInputAsFloat(inputText) {
    let value = inputText.val().replaceAll(',', '.');

    return parseFloat(value === '' ? 0 : value);
}