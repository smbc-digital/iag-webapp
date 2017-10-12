define(["jquery"], function ($) {

    var self = this;
    var matchboxes = [];

    var populate = function () {
        $('.matchbox-parent').each(function() {
            $(this).children('.matchbox-child').matchHeight({
                byRow: true
            });

            $(this).children().children('.matchbox-child').matchHeight({
                byRow: true
            });
        });
    };

    return {
        Init: function () {
            populate();
        }
    };
});

