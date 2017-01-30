$(".l-filters .filter-title, .l-filters .filter-inner-title").click(function () {
    $(this).siblings("ul").slideToggle(100, function () {
        $(this).parent().toggleClass("is-collapsed");
    });
});