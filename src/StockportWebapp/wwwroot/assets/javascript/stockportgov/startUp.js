define(["jquery", "multiselect", "utils"], function ($, multiSelector, utils) {

    var documentReady = function () {
        $('.global-alert-close-container a').on('click', function () {
            $(this).closest('.global-alert').hide();
        });

        utils.SwapLogo();

        $(".show-search-button").click(function () {
            $("#mobileSearchInput").slideToggle(220);
            $(".show-search-button").toggleClass("arrow");
        });
    };

    var documentResize = function () {

        utils.SwapLogo();

        if ($(window).width() > utils.TabletWidth) {
            $("#mobileSearchInput").hide();
            $(".show-search-button").removeClass("arrow");
            $('#displayRefineBy').css('display', 'block');
        }
    };

    var init = function () {

        $(document).ready(function () {
            $('.multi-select-control').each(function () {
                multiSelector.Init($(this).val());
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
        Init: init
    }
});

