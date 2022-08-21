let addedCustomersToMerge = [];

window.addEventListener("load", function () {
    CheckMaskDependencie();
    addedCustomersToMerge = [];
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

function AddSelectedCustomer() {
    // Check if any customer was selected
    var isAnyProductSelected = $('#customerSelectTable tbody > tr.table-active').length > 0;
    if (!isAnyProductSelected) {
        swal("Oops!", 'Você precisa selecionar um cliente antes de clicar em "Selecionar cliente"', "error");
        return;
    }

    // Get selected customer ID
    var selectedCustomerId = $('#customerSelectTable tbody > tr.table-active > th').first().html();

    // Can`t add current user to merge table 
    if (selectedCustomerId == currentCustomerId) {
        swal("Oops!", 'Você não pode mesclar um cliente com ele mesmo. O ID do cliente atual é ' + currentCustomerId, "info");
        return;
    }

    // Add selected customer to table if it`s not added yet
    if (!addedCustomersToMerge.includes(selectedCustomerId)) {
        AddSelectedCustomerToMergeTable(selectedCustomerId);
        DismissCustomerSelectModal();
    }
    else {
        swal("Oops!", 'Este cliente já foi adicionado na lista de mesclagem', "info");
    }
}

function DismissCustomerSelectModal() {
    $('#btnDismissMergeCustomerModal').click();
}

function AddSelectedCustomerToMergeTable(customerId) {
    // Get customer row
    $("#customerSelectTable tbody > tr").each(function (index, row) {
        if (index < 0) return;

        var idCell = row.getElementsByTagName("th");
        if (idCell.length == 0) return;

        // Costumer was found
        if (idCell[0].innerText == customerId) {
            // Add found costumer to Merge Customer Table
            AddCostumerToMergeTable(row.innerHTML, customerId);
            addedCustomersToMerge.push(customerId);
        }
    });
}

function AddCostumerToMergeTable(costumerTableRowInnerHTML, customerId) {
    // Clear table if the only content is the Empty Table Label row
    if ($("#customerMergeTable tbody > tr > th").length == 0) {
        $("#customerMergeTable tbody").empty();
    }

    var costumerTableRow = document.createElement("tr");
    costumerTableRow.innerHTML = costumerTableRowInnerHTML;

    // Create action cell
    const actionCell = document.createElement("td");
    actionCell.innerHTML = `<a onClick="RemoveCostumerFromMergeTable(${customerId})" href="#" class="btn btn-danger text-white" title="Remover cliente">
                                <i class="uil uil-times"></i>
                            </a>`;
    costumerTableRow.append(actionCell);


    $("#customerMergeTable tbody").append(costumerTableRow);
    $("#customerMergeTable tbody > tr").removeClass("table-active");// Remove active class from all rows
}

function RemoveCostumerFromMergeTable(customerId) {
    
    // Get customer row
    $("#customerMergeTable tbody > tr").each(function (index, row) {
        if (index < 0) return;

        var idCell = row.getElementsByTagName("th");
        if (idCell.length == 0) return;

        // Costumer was found
        if (idCell[0].innerText == customerId) {
            // Remove found costumer from Merge Customer Table 
            $("#customerMergeTable tbody > tr:nth-child(" + (index + 1) + ")").remove();
            addedCustomersToMerge.pop(customerId);
        }
    });

    // Add Empty Table Label row if the table is now empty  
    if ($("#customerMergeTable tbody > tr").length == 0) {
        const emptyLabelRow = document.createElement("tr");
        emptyLabelRow.innerHTML = `<td colspan="4" class="text-center">Nenhum cliente selecionado</td>`;
        $("#customerMergeTable tbody").append(emptyLabelRow);
    }
}

function MergeCustomer() {
    if (addedCustomersToMerge.length == 0) {
        swal("Oops!", 'Você não adicionou nenhum cliente para mesclar com esta ficha', "info");
        return;
    }

    swal({
        text: 'Tem certeza que deseja mesclar os clientes?',
        buttons: {
            cancel: "Cancelar",
            ok: {
                text: "Mesclar clientes",
                closeModal: false,
            }
        },
    })
    .then(buttonClicked => {
        if (!buttonClicked) throw null;
        
        $.ajax({
            method: 'POST',
            url: mergeCustomersUrl,
            data: { currentCustomerId: currentCustomerId, customersToMerge: addedCustomersToMerge },
            beforeSend: function () {
                $('#mergeCustomerButton').addClass(`disabled`);
            }
        }).done(function (response) {
            swal("Clientes mesclados com sucesso!", response + ". A página será recarregada.", "success").then(() => location.reload());
        }).fail(function (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível mesclar os clientes! Mensagem de erro: " + error.responseText, "error");
        }).always(function () {
            $('#mergeCustomerButton').removeClass(`disabled`);
        });
    })
    .catch(error => {
        if (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível mesclar os clientes! Mensagem de erro: " + error, "error");
        } else {
            swal.stopLoading();
            swal.close();
        }
    });
}