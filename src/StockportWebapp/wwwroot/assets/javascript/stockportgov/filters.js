$(document)
    .ready(function () {
        $(".l-filters .filter-title, .l-filters .filter-inner-title .expanding-Link-text").click(function () {
            $(this).siblings("ul").slideToggle(100, function () {
                $(this).parent("li").toggleClass("is-collapsed");
            });
        });

        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-filters .collapsible:not(#custom-filter-li)").addClass("is-collapsed");
            $(".filters-list li.active .field-validation-error").parents("li").removeClass("is-collapsed");
        } else {
            $(".l-filters .filter:not(#custom-filter-li):not(#date-filter):not(#category-filter).collapsible").addClass("is-collapsed");
            $(".l-filters .filter:not(#custom-filter-li):not(#date-filter):not(#category-filter) .collapsible").addClass("is-collapsed");
        }

        if ($("#custom-filter-li").hasClass("customdateactive")) {
            $("#custom-filter-li").removeClass("is-collapsed");
        }

        if ($(window).width() > tabletWidth) {
            $(".filters-list li.active").each(function () {
                $(this).parents("li").removeClass("is-collapsed");
            });
        }

        
    });