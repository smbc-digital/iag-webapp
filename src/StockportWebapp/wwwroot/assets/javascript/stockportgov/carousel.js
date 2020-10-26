define(["jquery", "slick"], function ($, slick) {

    var init = function () {
        $(".carousel a").css("display", "block");
        $(".carousel div").css("display", "block");
        $(document).ready(
            function () {
                $(".carousel").slick(
                    {
                        arrows: true,
                        infinite: true,
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        dots: true,
                        accessibility: true,
                        autoplay: false,
                        autoplaySpeed: 5000,
                        focusOnChange: true,

                        // Overriden for accessibility
                        customPaging: function (slider, i) {
                            return $('<button type="button" />');
                        },
                    });
            }
        );
    };

    return {
        Init: init
    }
});
