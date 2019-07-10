/*! file: datetime.js | author: Etch | website: ? */
Date.prototype.toDateInputValue = (function () {
    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0, 10);
});

// Set times
$(document).ready(function () {
    $('.today').val(new Date().toDateInputValue());
});