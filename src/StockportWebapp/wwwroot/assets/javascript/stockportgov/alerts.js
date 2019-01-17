define(["jquery"], function ($) {

    var addCookie = function (slug, cookieType) {
        var url = "/cookies/add?slug=" + slug + "&cookieType=" + cookieType;
        $.post(url, function (data, status) { });
    };

    var handleClicks = function () {
        $(document).ready(function () {
            $(".close-alert").click(function () {
                var slug = $(this).attr("data-slug");
                var cookieType = "alert";
                addCookie(slug, cookieType);
                var parent = $(this).attr("data-parent");
                $(this).closest("." + parent).hide();
            });
        });

        $(document).ready(function () {
            $(".dismiss a").click(function () {
                var slug = $(this).attr("data-slug");
                var cookieType = "alert";
                addCookie(slug, cookieType);

                var alertBox = $(this).closest("." + $(this).attr("data-parent"));
                var alertBoxContainer = alertBox.parent();

                alertBox.hide();

                var visibleChildren = alertBoxContainer.children(':visible');

                if (visibleChildren.length === 0) {
                    alertBoxContainer.hide();
                }
            });
        });

    };


    var init = function () {
        handleClicks();
    };

    return {
        Init: init
    };
});
