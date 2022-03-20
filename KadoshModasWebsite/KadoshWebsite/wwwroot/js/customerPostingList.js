function RenderPaginatedCustomerPostings(page) {
    $.ajax({
        method: 'GET',
        url: getCustomerPostingsByCustomerPaginatedUrl,
        data: { page: page, filterByCustumerId: filterPostingsByCustumerId },
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
