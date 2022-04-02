function RenderPaginatedCustomerPostings(page) {
    var url;
    var requestData;

    if (filterPostingsByCustumerId && filterPostingsByCustumerId != '') {
        url = getCustomerPostingsByCustomerPaginatedUrl;
        requestData = { page: page, filterByCustumerId: filterPostingsByCustumerId, getTotal: getTotal };
    }
    else if (filterPostingsBySaleId && filterPostingsBySaleId != '') {
        url = getCustomerPostingsBySalePaginatedUrl;
        requestData = { page: page, filterBySaleId: filterPostingsBySaleId, getTotal: getTotal };
    }
    else {
        url = getCustomerPostingsByStoreAndDatePaginatedUrl;
        requestData = { page: page, date: filterPostingsByDate, storeId: filterPostingsByStore, getTotal: getTotal };
    }

    $.ajax({
        method: 'GET',
        url: url,
        data: requestData,
        beforeSend: function () {
            ShowPostingsLoadingSpinner();
        }
    }).done(function (salesPartial) {
        $('#customerPostingsList').html(salesPartial);
    }).fail(function (error) {
        ToastErrorMessage(error.responseText)
    }).always(function () {
        HidePostingsSpinner();
    });
}

function ShowPostingsLoadingSpinner() {
    $('#loadingPostingSpinner').show();
}

function HidePostingsSpinner() {
    $('#loadingPostingSpinner').hide();
}
