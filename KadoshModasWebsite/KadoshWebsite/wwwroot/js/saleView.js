$(window).on("load", function () {
    $('#CustomerId').select2({
        placeholder: "Selecione um cliente"
    });
    $('#comboGetProducts').select2({
        placeholder: "Selecione um produto"
    });
});

$(document).ready(function () {

    // Prevent form submit on enter
    document.getElementById("createSaleForm").onkeypress = function (e) {
        var key = e.charCode || e.keyCode || 0;
        if (key == 13) {
            e.preventDefault();
        }
    }

    document.getElementById("productBarCode").onkeypress = function (e) {
        var key = e.charCode || e.keyCode || 0;
        if (key == 13) {
            AddItemToSaleByBarCode(e.target.value);
            e.target.value = "";
        }
    }

    $('#DownPayment').maskMoney({ prefix: '', allowNegative: false, thousands: '', decimal: ',', affixesStay: false });

    $('#DownPayment').on('change', function () {
        let downPayment = $('#DownPayment').val().replaceAll(',', '.');
        let currentTotal = GetSaleTotal();

        if (downPayment >= currentTotal) {
            alert('O valor de entrada não pode ser maior ou igual o valor total da venda');
            $('#DownPayment').val('0,00');
        }
    });

    $('#btnRegisterSale').on('click', function () {
        let correctPrice = $('#DownPayment').val().replaceAll('.', '').replaceAll(',', '.');
        $('#DownPayment').val(correctPrice);
    });

    $('#PaymentType').on('change', function () {
        const CASH_OPTION = 0;
        const IN_INSTALLMENTS = 1;
        const ON_CREDIT = 2;

        var selectedPaymentType = $(this).val();

        if (selectedPaymentType == CASH_OPTION) {
            $('#DownPaymentDiv').hide();
            $('#NumberOfInstallmentsDiv').hide();
            $('#saleInstallmentsPreview').hide();
        }
        else if (selectedPaymentType == IN_INSTALLMENTS) {
            UpdateInstallments();
            $('#DownPaymentDiv').show();
            $('#NumberOfInstallmentsDiv').show();
            $('#saleInstallmentsPreview').show();
        }
        else if (selectedPaymentType == ON_CREDIT) {
            $('#DownPaymentDiv').show();
            $('#NumberOfInstallmentsDiv').hide();
            $('#saleInstallmentsPreview').hide();
        }
    });

    $('#NumberOfInstallments').on('change', function () {
        UpdateInstallments();
    });
});

function AddItemToSale(productId) {
    $.ajax({
        method: 'GET',
        url: getProductInfoUrl,
        data: { productId: productId },
        //beforeSend: function () {
        //    ShowProductLoadingSpinners(tableRowIndex);
        //}
    }).done(function (productData) {
        var newRowIndex = GetNextItemTableRowIndex();
        AddItemRowToProductsTable(productData, newRowIndex);
        UpdateProductPriceByRowIndex(newRowIndex, productData.price);
        RecalculateSubtotalFromSpecificRowIndex(newRowIndex);
    }).fail(function () {
        swal("Oops!", e.responseText, "error").then(() => {
            $('#productBarCode').focus();
        });
    }).always(function () {
        /*HideProductLoadingSpinners(tableRowIndex);*/
    });
}

function AddItemToSaleByBarCode(productBarCode) {
    $.ajax({
        method: 'GET',
        url: getProductInfoByBarCodeUrl,
        data: { barCode: productBarCode }
    }).done(function (productData) {
        var newRowIndex = GetNextItemTableRowIndex();
        AddItemRowToProductsTable(productData, newRowIndex);
        UpdateProductPriceByRowIndex(newRowIndex, productData.price);
        RecalculateSubtotalFromSpecificRowIndex(newRowIndex);
    }).fail(function (e) {
        swal("Oops!", e.responseText, "error").then(() => {
            $('#productBarCode').focus();
        });
    });
}

function GetNextItemTableRowIndex() {
    return $('#saleItemsTable > tbody > tr').length;
}

function AddItemRowToProductsTable(productData, newRowIndex) {

    var productIndex = CheckIfProductIsAlreadyAdded(productData.id);
    if (productIndex != null) {
        IncrementItemQuantityByRowIndex(productIndex);
        return;
    }

    // Create table row
    var table = $('#saleItemsTable > tbody')[0];
    var newRow = document.createElement('tr');

    // Fill in cells informations
    newRow.innerHTML += CreateProductIndexCell(newRowIndex);
    newRow.innerHTML += CreateProductNameCell(newRowIndex, productData.id, productData.name);
    newRow.innerHTML += CreateProductPriceCell(newRowIndex, productData.price);
    newRow.innerHTML += CreateProductQuantityCell(newRowIndex);
    newRow.innerHTML += CreateProductDiscountCell(newRowIndex);
    newRow.innerHTML += CreateProductSubtotalCell();
    newRow.innerHTML += CreateProductActionsCell(newRowIndex);

    // Append row to table
    table.appendChild(newRow);

}

/**
 * Check if a product is already added to Items Table. If the product is already added returns the table ID, otherwise returns null.
 * @param {any} productId Product ID
 */
function CheckIfProductIsAlreadyAdded(productId) {
    var itemIndex = null;
    $('input[id^=ProductId]').each((index, element) => {
        if (element.value == productId.toString())
            itemIndex = index;
    });
    return itemIndex;
}

function IncrementItemQuantityByRowIndex(rowIndex) {
    var quantityInput = $(`#saleItemsTable > tbody > tr:nth-child(${rowIndex + 1}) > td#productQuantity > input`);
    var newValue = parseInt(quantityInput.val()) + 1;
    quantityInput.val(newValue);
    quantityInput.trigger('change');
}

function CreateProductIndexCell(itemIndex) {
    return `<th scope="row" id="productCount">${itemIndex + 1}</th>`;
}

function CreateProductNameCell(itemIndex, productId, productName) {
    return `<td style="max-width: 50%">
                <input type="hidden" id="ProductId${itemIndex}" name="SaleItems[${itemIndex}][ProductId]" value="${productId}"/>
                <span>${productName}</span>
            </td>`;
}

function CreateProductPriceCell(itemIndex, productPrice) {
    return `<td id="productPrice">
                <div class="spinner-border text-primary visually-hidden" role="status">
                    <span class="visually-hidden">Carregando...</span>
                </div>
                <div class="row-data">
                    R$ <span name="SaleItems[${itemIndex}][Price]">${productPrice}</span>
                </div>
            </td>`;
}

function CreateProductQuantityCell(itemIndex) {
    return `<td id="productQuantity">
                <input type="number" name="SaleItems[${itemIndex}][Quantity]" class="form-control" min="1" value="1" onchange="RecalculateSubtotal(this)" required />
            </td>`;
}

function CreateProductDiscountCell(itemIndex) {
    return `<td id="productDiscount" style="max-width: 5%">
                <span class="badge bg-success" style="display: none">
                0
                <input type="number" id="discountValue" name="SaleItems[${itemIndex}][DiscountInPercentage]" class="bg-transparent border-0 text-white fw-bold w-50 p-0 m-0" value="0" min="0" max="100" readonly />
                %
                </span>
            </td>`;
}

function CreateProductSubtotalCell() {
    return `<td id="productSubtotal" class="fw-bold">
                <div class="spinner-border text-primary visually-hidden" role="status">
                    <span class="visually-hidden">Carregando...</span>
                </div>
                <div class="row-data">
                    R$ 0,00
                </div>
            </td>`;
}

function CreateProductActionsCell(itemIndex) {
    return `<td><button type="button" title="Aplicar desconto no item" onclick="SetUpDiscountModal(this)" class="btn btn-primary btn-sm mb-1" data-bs-toggle="modal" data-bs-target="#discountModal">
                <i class="uil uil-percentage"></i>
            </button>
            <a class="btn btn-danger btn-sm" title="Apagar este produto da venda" onclick="RemoveItemFromTableByRowIndex(${itemIndex})"><i class="uil uil-times"></i></a></td>`;
}

function GetInputIndexAsInteger(inputObject) {
    var matchPattern = /(\d+)/;
    if (inputObject.name.match(matchPattern))
        return parseInt(inputObject.name.match(matchPattern)[0]);
    else
        return 0;
}

function ApplyNewRowIndexes() {
    $('#saleItemsTable > tbody > tr').each(function (index) {
        var rowCountData = $(this).find("#productCount");
        rowCountData.html(index + 1);
    });
}

function DisableSelectedOptionsFromSaleItemSelects(selectIdToIgnore) {
    var selectedValues = GetAllSelectedValuesAndIdsFromSaleItemSelects();
    $('.sale-item-select').each(function () {
        var currentSelectId = this.id;

        $(this).find('option').each(function () {
            var currentSelectedValue = this.value;
            var currentOption = this;
            selectedValues.forEach(function (element) {
                if (element.selectId != currentSelectId && element.selectedValue == currentSelectedValue) {
                    console.log(currentSelectId + ' disabled value: ' + currentSelectedValue);
                    console.log(currentOption);
                    console.log(currentOption.disabled);
                    if (!currentOption.disabled && selectIdToIgnore != currentSelectId)
                        $(currentOption).attr("disabled", 'disabled');
                }
                else
                    $(currentOption).removeAttr('disabled');
            });
        });
    });
}

function GetAllSelectedValuesAndIdsFromSaleItemSelects() {
    var selectedValues = [];
    $('.sale-item-select').each(function () {
        var selectId = this.id;
        var selectedValue = $(this).find('option:selected').val();

        if (selectedValue)
            selectedValues.push({
                selectId,
                selectedValue
            });
    });
    return selectedValues;
}

function RecalculateSubtotal(element) {
    var rowIndex = GetInputIndexAsInteger(element);

    var price = GetProductPriceByRowIndex(rowIndex);
    var quantity = element.value;
    var discountInPercentage = GetProductDiscountByRowIndex(rowIndex);

    UpdateSubtotalByRowIndex(rowIndex, price, quantity, discountInPercentage);

}

function RecalculateSubtotalFromSpecificRowIndex(rowIndex) {

    var price = GetProductPriceByRowIndex(rowIndex);
    var quantity = GetProductQuantityByRowIndex(rowIndex);
    var discountInPercentage = GetProductDiscountByRowIndex(rowIndex);

    UpdateSubtotalByRowIndex(rowIndex, price, quantity, discountInPercentage);

}

function GetProductPriceByRowIndex(rowIndex) {
    var priceSearch = GetPriceElementByRowIndex(rowIndex);

    if (priceSearch.length > 0) {
        var price = parseFloat(priceSearch.text().replace(',', '.'));
        return price;
    }
    else
        return 0;
}

function GetPriceElementByRowIndex(rowIndex) {
    var nameToSearch = 'SaleItems\[' + rowIndex + '\]\[Price\]';
    return $('[name="' + nameToSearch + '"]');
}

function GetProductQuantityByRowIndex(rowIndex) {
    var quantitySearch = GetQuantityElementByRowIndex(rowIndex);

    if (quantitySearch.length > 0) {
        var quantity = parseInt(quantitySearch.val());
        return quantity;
    }
    else
        return 0;
}

function GetQuantityElementByRowIndex(rowIndex) {
    var nameToSearch = 'SaleItems\[' + rowIndex + '\]\[Quantity\]';
    return $('[name="' + nameToSearch + '"]');
}

function GetProductDiscountByRowIndex(rowIndex) {
    var discountSearch = GetDiscountElementByRowIndex(rowIndex);

    if (discountSearch.length > 0) {
        var discount = parseFloat($(discountSearch).val());
        return discount || 0;
    }
    else
        return 0;
}

function GetDiscountElementByRowIndex(rowIndex) {
    var nameToSearch = 'SaleItems\[' + rowIndex + '\]\[DiscountInPercentage\]';
    return $('[name="' + nameToSearch + '"]');
}

function UpdateSubtotalByRowIndex(rowIndex, price, quantity, discountInPercentage) {
    var tableRow = $('#saleItemsTable > tbody').find('tr').eq(rowIndex);
    var subtotalElement = tableRow.find('#productSubtotal');

    var subtotal = (price * quantity || 0) - ((price * quantity || 0) * (discountInPercentage / 100));
    var subtotalFormated = 'R$ ' + subtotal.toFixed(2).replace('.', ',');

    subtotalElement.text(subtotalFormated);
    UpdateSaleSubtotal();
}

function SetNewSelectedProductInfo(selectedProductId, tableRowIndex) {
    if (selectedProductId == '') {
        UpdateProductPriceByRowIndex(tableRowIndex, 0);
        RecalculateSubtotalFromSpecificRowIndex(tableRowIndex);
        return;
    }

    $.ajax({
        method: 'GET',
        url: getProductInfoUrl,
        data: { productId: selectedProductId },
        beforeSend: function () {
            ShowProductLoadingSpinners(tableRowIndex);
        }
    }).done(function (productData) {
        UpdateProductPriceByRowIndex(tableRowIndex, productData.price);
        RecalculateSubtotalFromSpecificRowIndex(tableRowIndex);
    }).fail(function () {
        alert("error");
    }).always(function () {
        HideProductLoadingSpinners(tableRowIndex);
    });

}

function UpdateProductPriceByRowIndex(rowIndex, price) {
    var priceElement = GetPriceElementByRowIndex(rowIndex);
    if (priceElement.length > 0) {
        var priceFormated = price.toString().replace('.', ',');
        priceElement.text(priceFormated);
    }
}

function ShowProductLoadingSpinners(rowIndex) {
    var tableRow = $('#saleItemsTable > tbody').find('tr').eq(rowIndex);
    $(tableRow).find('.spinner-border').show();
    $(tableRow).find('.row-data').hide();
}

function HideProductLoadingSpinners(rowIndex) {
    var tableRow = $('#saleItemsTable > tbody').find('tr').eq(rowIndex);
    $(tableRow).find('.spinner-border').hide();
    $(tableRow).find('.row-data').show();
}

function SetUpDiscountModal(buttonClicked) {
    $('#discountTableRowIndex').val(parseInt($(buttonClicked.parentNode.parentNode).find('#productCount').text()) - 1)
}

function ApplyDiscount() {
    let tableIndex = $('#discountTableRowIndex').val();
    let discountElement = GetDiscountElementByRowIndex(tableIndex);
    let discountToApply = $('#discountToApply').val() || 0;

    if (discountElement.length > 0) {

        $(discountElement).val(discountToApply);
        $('#discountTableRowIndex').val(0);
        RecalculateSubtotalFromSpecificRowIndex(tableIndex);
        DismissDiscountModal();
    }

    if (discountToApply > 0)
        discountElement.parent().show();
    else
        discountElement.parent().hide();
}

function DismissDiscountModal() {
    $('#btnDismissModal').click();
    $('#productBarCode').focus();
}

function UpdateSaleSubtotal() {
    var saleSubtotal = 0;
    var saleDiscount = 0;

    $('#saleItemsTable > tbody > tr').each(function (index) {
        var actualSubtotal = GetProductSubtotalWithNoDiscountByRowIndex(index);
        saleSubtotal += actualSubtotal;
        saleDiscount += actualSubtotal * (GetProductDiscountByRowIndex(index) / 100);
    });

    var saleTotal = saleSubtotal - saleDiscount;

    var saleSubtotalFormated = 'R$ ' + saleSubtotal.toFixed(2).replace('.', ',');
    var saleDiscountFormated = 'R$ ' + saleDiscount.toFixed(2).replace('.', ',');
    var saleTotalFormated = 'R$ ' + saleTotal.toFixed(2).replace('.', ',');

    $('#saleSubtotal').text(saleSubtotalFormated);
    $('#saleDiscounts').text(saleDiscountFormated);
    $('#saleTotal').text(saleTotalFormated);

    UpdateInstallments();
}

function GetSaleTotal() {
    let subtotalToCalculate = 0;
    var discountToApply = 0;

    $('#saleItemsTable > tbody > tr').each(function (actualIndex) {
        let indexSubtotalToCalculate = GetProductSubtotalWithNoDiscountByRowIndex(actualIndex);
        subtotalToCalculate += indexSubtotalToCalculate;
        discountToApply += indexSubtotalToCalculate * (GetProductDiscountByRowIndex(actualIndex) / 100);
    });

    let calculatedSaleTotal = subtotalToCalculate - discountToApply;

    return calculatedSaleTotal;
}

function GetProductSubtotalWithNoDiscountByRowIndex(rowIndex) {
    let quantity = GetProductQuantityByRowIndex(rowIndex);
    let price = GetProductPriceByRowIndex(rowIndex);
    var subtotal = (price * quantity || 0);
    return subtotal;
}

function UpdateInstallments() {
    let saleTotal = GetSaleTotal();
    let installmentsNumber = $('#NumberOfInstallments').val();

    let individualInstallmentPrice = saleTotal / installmentsNumber;
    var individualInstallmentPriceFormated = installmentsNumber + 'x de R$ ' + individualInstallmentPrice.toFixed(2).replace('.', ',');
    $('#saleInstallmentsLabel').text(individualInstallmentPriceFormated);
}

function AddSelectedProduct() {
    // Check if any product was selected
    var isAnyProductSelected = $('#productSelectTable tbody > tr.table-active').length > 0;
    if (!isAnyProductSelected) {
        swal("Oops!", 'Você precisa selecionar um produto antes de clicar em "Selecionar produto"', "error");
        return;
    }

    // Get selected product ID
    var selectedProductId = $('#productSelectTable tbody > tr.table-active > th').first().html();

    // Add item to sale
    AddItemToSale(selectedProductId);
    DismissProductSelectModal();
}

function DismissProductSelectModal() {
    $('#btnDismissProductModal').click();
    $('#productBarCode').focus();
}

function RemoveItemFromTableByRowIndex(rowIndex) {
    $(`#saleItemsTable > tbody > tr:nth-child(${rowIndex + 1})`).remove();
}