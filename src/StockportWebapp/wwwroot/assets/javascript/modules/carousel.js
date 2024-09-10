define(['jquery', 'slick'], function ($) {
    var addSlickCssToHead = function () {
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = '/assets/stylesheets/vendor/slick.min.css'; 
        document.head.appendChild(link);
    };

    return {
        Init: function () {
            $(".carousel a").css("display", "block");
            $(".carousel div").css("display", "block");
            $(document).ready(
                function () {
                    $(".carousel").slick(
                    {
                        dots: true,
                        focusOnChange: true,

                        // Overriden for accessibility
                        customPaging: function (slider, i) {
                            return $('<button type="button" />');
                        },
                    });
                }
            );
            addSlickCssToHead();
        }
    }
});
