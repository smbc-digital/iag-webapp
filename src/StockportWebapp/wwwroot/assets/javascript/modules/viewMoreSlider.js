﻿define(["jquery"], function ($) {
    return {
        Init: function () {
            var container = $("#see-more-container").hide();
            var button = $("#see-more-services");

            button.on("click", function () {
                container.slideToggle(200, function () {
                    container.toggleClass("is-collapsed");
                    var oldButtonText = button.text();
                    var index = oldButtonText.search("fewer"); // -1 means no start index found
                    if (index === -1) {
                        button.text(oldButtonText.replace('more', 'fewer'));
                        container.find(".featured-topic-link:first").focus();
                    }
                    else {
                        button.text(oldButtonText.replace('fewer', 'more'));
                        $('html, body').animate({ scrollTop: (button.offset().top - 250) + 'px' });
                    }
                    button.toggleClass("is-collapsed");
                });
            });
        }
    }
});

