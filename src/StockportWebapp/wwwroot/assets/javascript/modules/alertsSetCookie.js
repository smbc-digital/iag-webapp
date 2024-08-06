define(['jquery'], function ($) {
    var addCookie = function (slug, cookieType) {
        var url = `/cookies/add?slug=${slug}&cookieType=${cookieType}`;
        $.post(url);
    };

    let hasInitialized = false;
    
    return {
        Init: function () {
            if (!hasInitialized) {
                hasInitialized = true;

                $(".close-alert").click(function () {
                    var slug = $(this).attr("data-slug");
                    addCookie(slug, "alert");
                });

                $(".dismiss a").click(function () {
                    var slug = $(this).attr("data-slug");
                    addCookie(slug, "alert");
                });

                $(".global-alert-close").click(function () {
                    var slug = $(this).attr("data-slug");
                    addCookie(slug, "alert");
                });
            }
        }
    };
});
