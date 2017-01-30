$(document)
    .ready(function() {
        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-filters#event-filters .collapsible").toggleClass("is-collapsed");
        } else {
            $(".l-filters#event-filters .filter:not(#date-filter).collapsible").toggleClass("is-collapsed");
            $(".l-filters#event-filters .filter:not(#date-filter) .collapsible").toggleClass("is-collapsed");
        }
    });