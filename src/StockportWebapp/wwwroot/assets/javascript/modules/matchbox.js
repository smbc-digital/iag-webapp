define(["jquery"], function ($) {
    return {
        Init: function () {
            $('.matchbox-parent').each(function () {
                var maxHeight = 0;

                $(this).find('.matchbox-child').each(function () {
                    maxHeight = $(this).height() > maxHeight ? $(this).height() : maxHeight;
                });

                $(this).find('.matchbox-child').height(maxHeight);
            });
        }
    };
});