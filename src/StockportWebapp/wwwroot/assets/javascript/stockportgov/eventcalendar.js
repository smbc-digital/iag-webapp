$(document)
    .ready(function() {
        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-events-filter .collapsible").toggleClass("is-collapsed");
        } else {
            $(".l-events-filter .filter:not(#category-filter).collapsible").toggleClass("is-collapsed");
            $(".l-events-filter .filter:not(#category-filter) .collapsible").toggleClass("is-collapsed");
        }
    });

$(".l-events-filter .filter-title, .l-events-filter .filter-inner-title").click(function () {
    $(this).siblings("ul").slideToggle(100, function() {
        $(this).parent().toggleClass("is-collapsed");
    });
});
