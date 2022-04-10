$(document).ready(function () {
    $('.repeater').repeater({
        //// (Optional)
        //// start with an empty list of repeaters. Set your first (and only)
        //// "data-repeater-item" with style="display:none;" and pass the
        //// following configuration flag
        //initEmpty: true,
        // (Optional)
        // "show" is called just after an item is added.  The item is hidden
        // at this point.  If a show callback is not given the item will
        // have $(this).show() called on it.
        show: function () {
            $(this).find("input[type='text'][name*='PhoneNumber']").mask('(00) 00000-0000');
            $(this).slideDown();
        },
        // (Optional)
        // "hide" is called when a user clicks on a data-repeater-delete
        // element.  The item is still visible.  "hide" is passed a function
        // as its first argument which will properly remove the item.
        // "hide" allows for a confirmation step, to send a delete request
        // to the server, etc.  If a hide callback is not given the item
        // will be deleted.
        hide: function (deleteElement) {
            if (confirm('Tem certeza que deseja apagar este telefone?')) {
                $(this).slideUp(deleteElement);
            }
        },
        // (Optional)
        // Removes the delete button from the first list item,
        // defaults to false.
        //isFirstItemUndeletable: true
    });

    // Clear repeater
    $('*[data-repeater-list="Phones"]').empty();
});