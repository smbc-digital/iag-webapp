define(["jquery"], function ($) {
    return {
        Init: function () {
            $('.matchbox-parent').each(function() { 
                $('.matchbox-child', this).matchHeight({ byRow: true }); 
            });
        }
    };
});
