define(["jquery", "multiselect", "utils"], function ($, multiSelector, utils) {

    var documentReady = function () {
        $('.global-alert-close-container a').on('click', function () {
            $(this).closest('.global-alert').hide();
        });

        $('.alert-close a').on('click', function () {
            $(this).closest('.alert').hide();
        });

        utils.SwapLogo();

        $(".show-search-button").click(function () {
            $("#mobileSearchInput").slideToggle(220);
            $(".show-search-button").toggleClass("arrow");
        });
        
        var ie = (!!window.ActiveXObject && +(/msie\s(\d+)/i.exec(navigator.userAgent)[1])) || NaN;
        if (ie === 9 || ie === 10) {
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

    var init = function () {

        $(document).ready(function () {
            $('.multi-select-control').each(function () {
                multiSelector.Init($(this).val());
            });
        });

        if (isIE() === true) {
            $("html").addClass("ie"); 
        }

        document.documentElement.className = document.documentElement.className.replace("no-js", "js");

        $(document).ready(function () {
            documentReady();
        });

        $(window).resize(function () {
            documentResize();
        });
        
    };

    return {
        Init: init
    }
});

