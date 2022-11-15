define(["jquery"], function ($) {

    var init = function () {
        $(document).ready(function () {
            $(".expanding-Link-items").addClass("is-collapsed");
            
            $(".expanding-Link-text").click(function () {
                if (!$('.expanding-Link-items:visible', this).length) {
                    $('.expanding-Link-items').hide();
                    $('.expanding-link-box-button').removeClass("is-collapsed-toggle");
                }

                $(this).children('.expanding-Link-items').slideToggle("fast");
                $(this).children('.expanding-link-box-button').toggleClass("is-collapsed-toggle");
            });

            $(".expanding-Link-text").keyup(function (event) {
                if (event.which === 32) {
                    event.target.click();
                }
            });
        });
    };

    return {
        Init: init
    }
});



