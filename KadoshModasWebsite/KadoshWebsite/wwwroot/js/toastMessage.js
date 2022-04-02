var errorToast;
var toastInstance;

$(document).ready(function () {
    errorToast = document.getElementById('errorToast');
    toastInstance = bootstrap.Toast.getOrCreateInstance(errorToast);
});

function ToastErrorMessage(errorMessage) {
    if (toastInstance === 'undefined') {
        console.error('It was not possible to get your Toast instance');
        return;
    }

    $('#errorToast #errorText').text(errorMessage);
    toastInstance.show();
}