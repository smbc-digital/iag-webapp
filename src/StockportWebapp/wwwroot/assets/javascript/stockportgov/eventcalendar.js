$(document)
    .ready(function() {
        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-filters#event-filters .collapsible").toggleClass("is-collapsed");
        } else {
            $(".l-filters#event-filters .filter:not(#date-filter).collapsible").toggleClass("is-collapsed");
            $(".l-filters#event-filters .filter:not(#date-filter) .collapsible").toggleClass("is-collapsed");
        }
       

        if ($("#custom-filter-li").hasClass("customdateactive")) {
            $("#custom-filter-li").removeClass().addClass("customdateactive").addClass("collapsible");

            var validationError = $("#inputvalidation").text();
            if (validationError !== null && validationError !== "") {
                $("#date-filter").removeClass().addClass("filter").addClass("collapsible");
            }           
        }
        else {
            $("#custom-filter-li").removeClass().addClass("filter").addClass("is-collapsed");
        }
    });