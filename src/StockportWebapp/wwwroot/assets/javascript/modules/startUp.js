define(["jquery", "utils"], function ($, utils) {

    var documentReady = function () {

        utils.SwapLogo();

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

        utils.SwapLogo();

        if ($(window).width() > utils.TabletWidth) {
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

            if (isIE()) {
                $("html").addClass("ie");
            }

            $(document).ready(function () {
                documentReady();
            });

            $(window).resize(function () {
                documentResize();
            });
        }
    }
});

