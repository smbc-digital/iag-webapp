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
                }
                else {
                    $(self).text($(self).text().replace('fewer', 'more'));
                    $(self).toggleClass("is-collapsed");
                }
            });

            matchboxes.Init();
        });
    };

    return {
        Init: init
    }
});

