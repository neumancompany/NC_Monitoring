"use strict";

var NCValidation =
{
    DX: {
        integer: function (options) {
            return options.value == null || options.value.toString().match(/^\d+$/);
        },
    },
}
