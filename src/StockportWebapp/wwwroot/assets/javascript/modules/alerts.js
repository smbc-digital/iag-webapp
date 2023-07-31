define(['jquery'], function ($) {
    var addCookie = function (slug, cookieType) {
        var url = "/cookies/add?slug=" + slug + "&cookieType=" + cookieType;
        $.post(url, function (data, status) { });
    };

    return {
        Init: function () { 

                // This gets called for every alert - will generate too many click events
                console.trace("adding click event")
                $(".close-alert").click(function () {
                    var slug = $(this).attr("data-slug");
                    var cookieType = "alert";
                    addCookie(slug, cookieType);
                    var parent = $(this).attr("data-parent");
                    $(this).closest("." + parent).hide();
                });

                // this will need changing to accomodate Semantic alerts
                $(".global-alert-close-container a").click(function () {

                    console.log("adding click event for global alert")

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
        }
    };
});
