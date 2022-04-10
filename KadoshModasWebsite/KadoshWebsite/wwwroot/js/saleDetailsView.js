window.addEventListener("load", function () {
    CheckMaskDependencie();
});

function InformCustomerPayment(saleId) {

    swal({
        text: 'Informar pagamento na venda',
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
                data: { saleId: saleId, amountToInform: value }
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

function PayOffSaleInstallment(saleId, installmentId, installmentNumber) {
    swal({
        text: 'Deseja quitar a parcela de número ' + installmentNumber + '?',
        buttons: {
            cancel: "Cancelar",
            ok: {
                text: "Quitar parcela",
                closeModal: false,
            }
        },
    })
        .then(buttonClicked => {
            if (!buttonClicked) throw null;
            $.ajax({
                method: 'POST',
                url: payOffInstallmentUrl,
                data: { saleId: saleId, installmentId: installmentId }
            }).done(function (response) {
                swal("Parcela quitada!", response + ". A página será recarregada.", "success").then(() => location.reload());
            }).fail(function (error) {
                swal("Oops!", "Aconteceu um erro e não foi possível quitar a parcela! Mensagem de erro: " + error.responseText, "error");
            });
        })
        .catch(error => {
            if (error) {
                swal("Oops!", "Aconteceu um erro e não foi possível quitar a parcela! Mensagem de erro: " + error, "error");
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