$(document)
    .ready(function() {
        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-filters#news-filters .collapsible").toggleClass("is-collapsed");
        } else {
            $(".l-filters#news-filters .filter:not(#category-filter).collapsible").toggleClass("is-collapsed");
            $(".l-filters#news-filters .filter:not(#category-filter) .collapsible").toggleClass("is-collapsed");
        }
    });
