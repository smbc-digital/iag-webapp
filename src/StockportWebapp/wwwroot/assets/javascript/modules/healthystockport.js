define(["jquery"], function ($) {
    var mobileWidth = 767;
    var tabletWidth = (1024 - 17);

    var documentReady = function () {

        $(".show-search-button").click(function () {
            $("#mobileSearchInput").slideToggle(220);
            $(".show-search-button").toggleClass("arrow");
        });

        if (isIE()) {
            $("#browser-check").removeClass("hidden");
            var element = document.getElementById("browser-check");
            element.className = element.className.replace(/\bhidden\b/g, "");
        }
    };

    var documentResize = function () {

        if ($(window).width() > mobileWidth) {
            if ($(".topic-block-content").length > 0) {
            } else {
                ($(".topic-block-content").css("height", "56px"));
            }
        }

        if ($(window).width() > tabletWidth) {
            $("#mobileSearchInput").hide();
            $(".show-search-button").removeClass("arrow");
            $('#displayRefineBy').css('display', 'block');
        }
    };

    var isIE = function (userAgent) {
        userAgent = userAgent || navigator.userAgent;
        return userAgent.indexOf("MSIE ") > -1 || userAgent.indexOf("Trident/") > -1 || userAgent.indexOf("Edge/") > -1;
    }

    return {
        Init: function () {
            $(document).ready(function () {
                documentReady();
            });

            $(window).resize(function () {
                documentResize();
            });
        }
    }
});