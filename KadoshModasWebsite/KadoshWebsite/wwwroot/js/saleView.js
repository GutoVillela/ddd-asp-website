$(window).on("load", function () {
    // Apply Select2 only when every asset is fully loaded
    RenameAndApplySelect2ToSaleItemSelects();
    $('#ProductId0').select2("val", -1);// Reset first select
    $('#CustomerId').select2({
        placeholder: "Selecione um cliente"
    });
});

$(document).ready(function () {

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

    $('.repeater').repeater({
        show: function () {
            RenameAndApplySelect2ToSaleItemSelects();
            $(this).slideDown();
            //DisableSelectedOptionsFromSaleItemSelects(); // TODO implement disable selected options
        },

        hide: function (deleteElement) {
            if (confirm('Tem certeza que deseja apagar este item?')) {
                $(this).slideUp(deleteElement, function () {
                    deleteElement();
                    RenameAndApplySelect2ToSaleItemSelects();
                    UpdateSaleSubtotal();
                    //DisableSelectedOptionsFromSaleItemSelects(); // TODO implement disable selected options
                });
            }
        },

        isFirstItemUndeletable: true
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

function RenameAndApplySelect2ToSaleItemSelects() {

    $('#saleItemsTable > tbody > tr').each(function () {
        var saleItemSelect = $(this).find(".sale-item-select")[0];
        ApplySelect2ToSaleItemSelectByName(saleItemSelect.name);

        var index = GetInputIndexAsInteger(saleItemSelect);
        saleItemSelect.id = "ProductId" + index;

        ApplyNewRowIndexes();
    });
}

function ApplySelect2ToSaleItemSelectByName(selectName) {
    $('[name="' + selectName.replace('[', '\[').replace(']', '\]') + '"]').select2({
        placeholder: 'Escolha um produto',
        minimumResultsForSearch: 10
    }).on('change', function () {

        SetNewSelectedProductInfo(this.value, GetInputIndexAsInteger(this));
        //DisableSelectedOptionsFromSaleItemSelects(this.id); // TODO implement disable selected options
    });
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

        ApplySelect2ToSaleItemSelectByName(this.name);
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