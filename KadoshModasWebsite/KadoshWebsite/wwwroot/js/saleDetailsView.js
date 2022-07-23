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

function CancelSale(saleId) {

    swal({
        icon: 'warning',
        text: 'Tem certeza que deseja estornar a venda?',
        buttons: {
            cancel: "Cancelar",
            ok: {
                text: "Estornar venda",
                closeModal: false,
            }
        },
    })
    .then(buttonClicked => {
        if (!buttonClicked) throw null;
        $.ajax({
            method: 'POST',
            url: cancelSaleUrl,
            data: { saleId: saleId }
        }).done(function (response) {
            swal("Venda estornada!", response + ". A página será recarregada.", "success").then(() => location.reload());
        }).fail(function (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível estornar a venda! Mensagem de erro: " + error.responseText, "error");
        });
    })
    .catch(error => {
        if (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível estornar a venda! Mensagem de erro: " + error, "error");
        } else {
            swal.stopLoading();
            swal.close();
        }
    });
}

function CancelSaleItem(saleId, productId, maxAmountToCancel) {
    var inputId = "sale-item-cancelation-input";
    swal({
        icon: 'warning',
        content: CreateInputForAmountMasked(1, maxAmountToCancel, inputId),
        text: 'Quantas unidades do item você deseja cancelar?',
        buttons: {
            cancel: "Cancelar",
            ok: {
                text: "Cancelar item da venda",
                closeModal: false,
            }
        },
    })
    .then(buttonClicked => {
        if (!buttonClicked) throw null;
        let amountToCancel = $('#' + inputId).val();
        $.ajax({
            method: 'POST',
            url: cancelSaleItemUrl,
            data: { saleId: saleId, productId: productId, amountToCancel: amountToCancel }
        }).done(function (response) {
            swal("Item cancelado!", response + ". A página será recarregada.", "success").then(() => location.reload());
        }).fail(function (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível cancelar o item! Mensagem de erro: " + error.responseText, "error");
        });
    })
    .catch(error => {
        if (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível cancelar o item! Mensagem de erro: " + error, "error");
        } else {
            swal.stopLoading();
            swal.close();
        }
    });
}

function CreateInputForAmountMasked(minValue, maxValue, inputId) {
    var valueInput = document.createElement("input");
    valueInput.type = "number";
    valueInput.id = inputId;
    valueInput.className = "form-control";
    valueInput.min = minValue;
    valueInput.max = maxValue;
    valueInput.value = minValue;

    return valueInput;
}