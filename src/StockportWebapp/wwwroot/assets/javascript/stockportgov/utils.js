define(["jquery"], function ($) {

    var mobileWidth = 767;
    var tabletWidth = (1024 - 17);

    var swapLogo = function () {
        var image = $("#header .logo-main-image");
        var logoMobile = image.attr("data-mobile-image");
        var logoDesktop = image.attr("data-desktop-image");

        if ($(window).width() <= mobileWidth) {
            image.attr("src", logoMobile);
        } else {
            image.attr("src", logoDesktop);
        }
    }

    var stripParamFromQueryString = function (url, param) {
        url = url.toLowerCase();
        param = param.toLowerCase();

        var result = '';
        var splitter = '?';
        var urlArray = url.split('?');
        result = urlArray[0];
        if (urlArray.length > 1) {
            var params = urlArray[1].split('&');
            for (var i = 0; i < params.length; i++) {
                var entry = params[i].split('=')
                if (entry[0].toLowerCase() !== param.toLowerCase()) {
                    result = result + splitter + params[i];
                    splitter = '&';
                }
            }
        }

        return result;
    };

    var init = function () {
        $("*").on("focus", function (e) {
            if ($(event.target).parent().hasClass('menu-account-button')) {
                $(".menu-tooltip").show();
            } else if (!$(event.target).parent().hasClass('menu-links')) {
                $('.menu-tooltip').attr('style', '');
            }
        });
    }

    return {
        StripParamFromQueryString: stripParamFromQueryString,
        SwapLogo: swapLogo,
        MobileWidth: mobileWidth,
        TabletWidth: tabletWidth,
        Init: init
    };
});