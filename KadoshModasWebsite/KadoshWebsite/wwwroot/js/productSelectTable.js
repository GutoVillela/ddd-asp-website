function LoadProductsPaginated(page) {

    var productName = $('#queryProductByName').val();

    $.ajax({
        method: 'GET',
        url: getProductsPaginatedUrl,
        data: { page: page, productName: productName },
        beforeSend: function () {
            ShowLoadingSpinner();
        }
    }).done(function (productsPartial) {
        $('#productList').html(productsPartial);
        AddSelectionEventToProductTable();
    }).fail(function (error) {
        if (typeof ToastErrorMessage != "function") {
            console.error('The script "toastMessage.js" is missing on the parent View.');
            console.error('Error: ' + error.responseText);
        }
        else
            ToastErrorMessage(error.responseText);
    }).always(function () {
        HideLoadingSpinner();
    });
}

function AddSelectionEventToProductTable() {
    $('#productSelectTable tbody > tr').on('click', function (e) {
        $("#productSelectTable tbody > tr").removeClass("table-active");
        e.currentTarget.classList.add("table-active");
    });
}

function ShowLoadingSpinner() {
    $('#loadingSpinner').show();
}

function HideLoadingSpinner() {
    $('#loadingSpinner').hide();
}