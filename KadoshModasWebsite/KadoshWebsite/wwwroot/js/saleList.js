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