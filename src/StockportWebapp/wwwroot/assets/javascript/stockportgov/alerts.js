define(["jquery"], function ($) {

    var addCookie = function (slug, cookieType) {
        var url = "/cookies/add?slug=" + slug + "&cookieType=" + cookieType;
        $.get(url,
            function (data, status) {
                
            });
    };

    var handleClicks = function() {
        $(document).ready(function () {
            $(".close-alert").click(function () {
                console.log($(this))
                var slug = $(this).attr("data-slug");
                var cookieType = "alert";
                addCookie(slug, cookieType);
            });
        });
    }

    var init = function () {
        handleClicks();
    };

    return {
        Init: init
    }
});
