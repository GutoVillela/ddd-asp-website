function LoadInfoToSingleInfoCard(urlToFetchData, elementToDisplayInfoId, loadingSpinnerId) {
    $.ajax({
        method: 'GET',
        url: urlToFetchData,
        beforeSend: function () {
            ShowLoadingSpinner(loadingSpinnerId);
        }
    }).done(function (data) {
        $('#' + elementToDisplayInfoId.id).html(data);
    }).fail(function (error) {
        if (typeof ToastErrorMessage != "function") {
            console.error('The script "toastMessage.js" is missing on the parent View.');
            console.error('Error: ' + error.responseText);
        }
        else
            ToastErrorMessage(error.responseText);
    }).always(function () {
        HideLoadingSpinner(loadingSpinnerId);
    });
}

function ShowLoadingSpinner(loadingSpinnerId) {
    $('#' + loadingSpinnerId.id).show();
}

function HideLoadingSpinner(loadingSpinnerId) {
    $('#' + loadingSpinnerId.id).hide();
}