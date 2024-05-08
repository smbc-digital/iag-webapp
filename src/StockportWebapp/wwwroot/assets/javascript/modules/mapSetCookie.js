define(['jquery'], function ($) {
    var addMapCookie = function (slug, cookieType) {
        var url = `/cookies/add?slug=${slug}&cookieType=${cookieType}`;
        $.post(url);
    };

    var removeMapCookie = function (slug, cookieType) {
        var url = `/cookies/remove?slug=${slug}&cookieType=${cookieType}`;
        $.post(url);
    };

    let hasInitialized = false;

    return {
        Init: function () {
            if (!hasInitialized) {
                hasInitialized = true;

                $("#showMap").click(function () {
                    var slug = $(this).attr("data-slug");
                    addMapCookie(slug, "map");
                });

                $("#hideMap").click(function () {
                    var slug = $(this).attr("data-slug");
                    removeMapCookie(slug, "map");
                });
            }
        }
    };
});
