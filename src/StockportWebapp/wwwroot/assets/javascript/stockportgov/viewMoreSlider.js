define(["jquery", "matchboxconfig"], function ($, matchboxes) {

    var init = function () {
        $(".generic-list-see-more-container").hide();
        $(".generic-list-see-more").addClass("is-visible").on("click", function () {
            self = this;
            $genericMoreDiv = $(this).parent().parent().prev();
            $genericMoreDiv.slideToggle(200, function () {
                if (!$(self).hasClass("is-collapsed")) {
                    $(self).text($(self).text().replace('more', 'fewer'));
                    $(self).toggleClass("is-collapsed");
                    if ($(".featured-topic-link")[8] !== undefined) {
                        $(".featured-topic-link")[8].focus();
                    }
                }
                else {
                    $(self).text($(self).text().replace('fewer', 'more'));
                    $(self).toggleClass("is-collapsed");
                    $([document.documentElement, document.body]).animate({
                        scrollTop: $("#" + $(self)[0].id).offset().top - 250
                    }, 300);
                }
            });

            matchboxes.Init();
        });
    };

    return {
        Init: init
    }
});

