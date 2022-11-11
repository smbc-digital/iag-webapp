define(['jquery', 'slick'], function ($) {

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
        }
    }
});
