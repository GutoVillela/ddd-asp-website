
function LoadCustomersPaginated(page) {

    var customerName = $('#queryCustomerByName').val();

    $.ajax({
        method: 'GET',
        url: getCustomersPaginatedUrl,
        data: { page: page, customerName: customerName, includeInactive: false,  },
        beforeSend: function () {
            ShowCustomerSelectLoadingSpinner();
        }
    }).done(function (productsPartial) {
        $('#customersList').html(productsPartial);
        AddSelectionEventToProductTable();
    }).fail(function (error) {
        if (typeof ToastErrorMessage != "function") {
            console.error('The script "toastMessage.js" is missing on the parent View.');
            console.error('Error: ' + error.responseText);
        }
        else
            ToastErrorMessage(error.responseText);
    }).always(function () {
        HideCustomerSelectLoadingSpinner();
    });
}

function AddSelectionEventToProductTable() {
    $('#customerSelectTable tbody > tr').on('click', function (e) {
        $("#customerSelectTable tbody > tr").removeClass("table-active");
        e.currentTarget.classList.add("table-active");
    });
}

function ShowCustomerSelectLoadingSpinner() {
    $('#customerSelectLoadingSpinner').show();
}

function HideCustomerSelectLoadingSpinner() {
    $('#customerSelectLoadingSpinner').hide();
}