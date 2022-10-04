function GetCustomerDelinquentAndRenderBadge(customerId) {
    $.ajax({
        method: 'GET',
        url: checkIfCustomerIsDelinquentAUrl,
        data: { customerId: customerId },
        beforeSend: function () {
            ShowDelinquentCustomerLoadingSpinner(customerId);
        }
    }).done(function (result) {
        if (result) {
            $('#delinquentCustomerBadgeResult' + customerId).show();
            $('#notDelinquentCustomerBadgeResult' + customerId).hide();
        }
        else {
            $('#delinquentCustomerBadgeResult' + customerId).hide();
            $('#notDelinquentCustomerBadgeResult' + customerId).show();
        }
    }).fail(function (error) {
        if (typeof ToastErrorMessage != "function") {
            console.error('The script "toastMessage.js" is missing on the parent View.');
            console.error('Error: ' + error.responseText);
        }
        else
            ToastErrorMessage(error.responseText);
    }).always(function () {
        HideDelinquentCustomerLoadingSpinner(customerId);
    });
}

function ShowDelinquentCustomerLoadingSpinner(customerId) {
    $('#delinquentCustomerLoadingSpinner' + customerId).show();
}

function HideDelinquentCustomerLoadingSpinner(customerId) {
    $('#delinquentCustomerLoadingSpinner' + customerId).hide();
}