$(window).on("load", function () {
    // Apply Select2 only when every asset is fully loaded
    $('#StoreId').select2({
        placeholder: 'Escolha uma loja',
        minimumResultsForSearch: 10
    });
});