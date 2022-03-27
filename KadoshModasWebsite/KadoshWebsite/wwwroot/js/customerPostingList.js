function RenderPaginatedCustomerPostings(page) {
    var url;
    var requestData;

    if (filterPostingsByCustumerId && filterPostingsByCustumerId != '') {
        url = getCustomerPostingsByCustomerPaginatedUrl;
        requestData = { page: page, filterByCustumerId: filterPostingsByCustumerId };
    }
    else {
        url = getCustomerPostingsBySalePaginatedUrl;
        requestData = { page: page, filterBySaleId: filterPostingsBySaleId };
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
    }).fail(function () {
        alert("error");// TODO Toast nicely error message
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
