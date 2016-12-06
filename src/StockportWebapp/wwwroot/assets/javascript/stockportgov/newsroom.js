$(document)
    .ready(function() {
        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-news-filter .collapsable").toggleClass("is-collapsed");
        }
    });

$(".l-news-filter .filter-title, .l-news-filter .filter-inner-title").click(function () {
    $(this).siblings("ul").slideToggle(100, function() {
        $(this).parent().toggleClass("is-collapsed");
    });
});
