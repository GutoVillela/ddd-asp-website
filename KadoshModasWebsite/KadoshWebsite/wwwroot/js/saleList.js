function PayOffSaleUrl(saleId) {
    swal({
        title: "Deseja quitar esta dívida?",
        text: "Será realizado um lançamento do tipo PAGAMENTO na ficha do cliente.",
        icon: "warning",
        buttons: {
            cancel: "Cancelar",
            catch: {
                text: "Quitar dívida!",
                value: "catch",
                closeModal: false
            },
        },
        dangerMode: true
    })
    .then(selectedOption => {
        if (!selectedOption) throw null;

        $.ajax({
            method: 'POST',
            url: payOffSaleUrl,
            data: { saleId: saleId }
        }).done(function () {
            swal("Dívida quitada com sucesso!", "Foi realizado um lançamento com o valor restante da dívida na ficha do cliente. A página será recarregada.", "success").then(() => location.reload());
        }).fail(function (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível quitar a dívida! Mensagem de erro: " + error.responseText, "error");
        });
    })
    .catch(error => {
        if (error) {
            swal("Oops!", "Aconteceu um erro e não foi possível quitar a dívida! Mensagem de erro: " + error, "error");
        } else {
            swal.stopLoading();
            swal.close();
        }
    });
}


function RenderPaginatedSales(page) {
    $.ajax({
        method: 'GET',
        url: getSalesPaginatedUrl,
        data: { page: page, filterByCustumerId: filterByCustumerId },
        beforeSend: function () {
            ShowLoadingSpinner();
        }
    }).done(function (salesPartial) {
        $('#salesList').html(salesPartial);
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

function ShowLoadingSpinner() {
    $('#loadingSpinner').show();
}

function HideLoadingSpinner() {
    $('#loadingSpinner').hide();
}