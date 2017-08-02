var STK = STK || {};

STK.StartUp = (function () {

    var mobileWidth = 767;
    var tabletWidth = (1024 - 17);

    var documentReady = function () {
        $('.global-alert-close-container a').on('click', function () {
            $(this).closest('.global-alert').hide();
        });

        STK.Utils.SwapLogo();

        $(".show-search-button").click(function () {
            $("#mobileSearchInput").slideToggle(220);
            $(".show-search-button").toggleClass("arrow");
        });
    };

    var documentResize = function () {

        STK.Utils.SwapLogo();

        if ($(window).width() > tabletWidth) {
            $("#mobileSearchInput").hide();
            $(".show-search-button").removeClass("arrow");
            $('#displayRefineBy').css('display', 'block');
        }
    };

    var init = function () {

        $(document).ready(function () {
            $('.multi-select-control').each(function () {
                STK.MultiSelector.Init($(this).val());
            });
        });


        document.documentElement.className = document.documentElement.className.replace("no-js", "js");

        $(document).ready(function () {
            documentReady();
        });

        $(window).resize(function () {
            documentResize();
        });
    };

    return {
        Init: init,
        MobileWidth: mobileWidth,
        TabletWidth: tabletWidth
    }
})();

STK.StartUp.Init();


