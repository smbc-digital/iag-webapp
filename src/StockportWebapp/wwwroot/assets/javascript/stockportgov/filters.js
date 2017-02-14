$(document)
    .ready(function () {
        $(".l-filters .filter-title, .l-filters .filter-inner-title").click(function () {
            $(this).siblings("ul").slideToggle(100, function () {
                $(this).parent("li").addClass("is-collapsed");
            });
        });

        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-filters .collapsible").addClass("is-collapsed");
        } else {
            $(".l-filters .filter:not(#date-filter):not(#category-filter).collapsible").addClass("is-collapsed");
            $(".l-filters .filter:not(#date-filter):not(#category-filter) .collapsible").addClass("is-collapsed");
        }

        if ($("#custom-filter-li").hasClass("customdateactive")) {
            $("#custom-filter-li").removeClass("is-collapsed");
            $("#news-archive").removeClass("is-collapsed");
        }
});