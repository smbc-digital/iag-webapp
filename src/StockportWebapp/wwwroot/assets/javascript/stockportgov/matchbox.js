define(["jquery"], function ($) {
    return {
        Init: function () {
            $('.matchbox-parent').each(function () {
                var maxHeight = 0;
                $('.matchbox-child').each(function () {
                    if ($(this).height() > maxHeight) {
                        maxHeight = $(this).height();
                    }
                });
                $('.matchbox-child').height(maxHeight);
            });
        }
    };
});