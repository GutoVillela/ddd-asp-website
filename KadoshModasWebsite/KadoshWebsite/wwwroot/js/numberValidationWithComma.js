/**
 * This method overloads the decimal validation from MVC and allows us to use "," as a valid decimal separator.
 * @param {any} value
 * @param {any} element
 */
$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:[,]\d+)?$/.test(value) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:[.]\d+)?$/.test(value);
};