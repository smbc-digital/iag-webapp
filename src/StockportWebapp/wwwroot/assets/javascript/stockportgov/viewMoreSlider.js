define(["jquery"], function ($) {
    return {
        Init: function () {
            var container = $("#see-more-container");
            var button = $("#see-more-services");
            container.hide();
            button.on("click", function () {
                container.slideToggle(200, function () {
                    container.toggleClass("is-collapsed");
                    var oldButtonText = button.text();
                    if (oldButtonText.includes("fewer")) {
                        button.text(oldButtonText.replace('fewer', 'more'));
                        $('html, body').animate({
                            scrollTop: (button.offset().top - 150) + 'px'
                        });
                    }
                    else {
                        button.text(oldButtonText.replace('more', 'fewer'));
                        if ($(".featured-topic-link")[8] !== undefined) {
                            $(".featured-topic-link")[8].focus();
                        }
                    }
                });
            });
        }
    }
});

