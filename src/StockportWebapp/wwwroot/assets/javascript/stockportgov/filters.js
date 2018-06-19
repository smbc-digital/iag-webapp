define(["jquery", "utils"], function ($, utils) {

    var init = function () {
        $(document).ready(function () {
            $(".l-filters .filter-title, .l-filters .filter-inner-title, .expanding-Link-text").on("keypress click", function (e) {
                e.preventDefault();

                if (e.type === "click" || (e.type === "keypress" && e.which === 32)) {
                    $(this).siblings("ul").slideToggle(100, function () {
                        $(this).parent("li").toggleClass("is-collapsed");
                    });    
                }
            });

            if ($(window).width() <= utils.TabletWidth) {
                $(".l-filters .collapsible:not(#custom-filter-li)").addClass("is-collapsed");
                $(".filters-list li.active .field-validation-error").parents("li").removeClass("is-collapsed");
            } else {
                $(".l-filters .filter:not(#custom-filter-li):not(#date-filter):not(#category-filter).collapsible").addClass("is-collapsed");
                $(".l-filters .filter:not(#custom-filter-li):not(#date-filter):not(#category-filter) .collapsible").addClass("is-collapsed");
            }

            if ($("#custom-filter-li").hasClass("customdateactive")) {
                $("#custom-filter-li").removeClass("is-collapsed");
            }

            if ($(window).width() > utils.TabletWidth) {
                $(".filters-list li.active").each(function () {
                    $(this).parents("li").removeClass("is-collapsed");
                });
            }
        });
    };

    return {
        Init: init
    }
});
