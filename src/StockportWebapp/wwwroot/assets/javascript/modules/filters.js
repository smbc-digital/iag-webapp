define(["jquery", "utils"], function ($, utils) {

    var init = function () {
        $(document).ready(function () {
            document.getElementById("news-archive").classList.add("is-collapsed");

            $(".l-filters .filter-title, .l-filters .filter-inner-title, .expanding-Link-text").on("keypress click", function (e) {
                e.preventDefault();
                console.log("click or keypress event triggered");
                if (e.type === "click" || (e.type === "keypress" && e.which === 32)) {
                    $(this).siblings("ul").slideToggle(100, function () {
                        $(this).parent("li").toggleClass("is-collapsed");
                    });
                }
            });

            $(".filters-list li.active").each(function () {
                $(this).parents("li").addBack().removeClass("is-collapsed");
                $(this).closest(".collapsible").removeClass("is-collapsed");
            });

            if ($("#news-archive .filters-list li.active").length > 0) {
                $("#news-archive").removeClass("is-collapsed");
            }

            if ($("#custom-filter-li").hasClass("customdateactive")) {
                $("#custom-filter-li").removeClass("is-collapsed");
            }
        });
    };

    return {
        Init: init
    }
});
