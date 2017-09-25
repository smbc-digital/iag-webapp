define(["jquery"], function ($) {
    $(".carousel a").css("display", "block");
    $(".carousel div").css("display", "block");
    $(".carousel").slick({
        arrows: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        dots: true,
        autoplay: true,
        autoplaySpeed: 5000
    });
});
define(["jquery"],function(s){s(".carousel a").css("display","block"),s(".carousel div").css("display","block"),s(".carousel").slick({arrows:!0,infinite:!0,slidesToShow:1,slidesToScroll:1,dots:!0,autoplay:!0,autoplaySpeed:5e3})});
define(["jquery", "cludo"], function ($, cludo) {
    var CludoSearch;
    (function () {
        var cludoSettings = {
            customerId: 112,
            engineId: 1757,
            type: 'standardOverlay',
            hideSearchFilters: true,
            initSearchBoxText: '',
            searchInputs: ["cludo-search-form", "cludo-search-mobile-form", "cludo-search-hero-form"],
            theme: { themeColor: '#055c58', themeBannerColor: { textColor: '#333', backgroundColor: '#f2f2f2' }, borderRadius: 10 },
            language: 'en'
        };
        CludoSearch = new Cludo(cludoSettings);
        CludoSearch.init();
    })();
});
define(["jquery","cludo"],function(e,r){var o;!function(){var e={customerId:112,engineId:1757,type:"standardOverlay",hideSearchFilters:!0,initSearchBoxText:"",searchInputs:["cludo-search-form","cludo-search-mobile-form","cludo-search-hero-form"],theme:{themeColor:"#055c58",themeBannerColor:{textColor:"#333",backgroundColor:"#f2f2f2"},borderRadius:10},language:"en"};o=new Cludo(e),o.init()}()});
define(["jquery"], function ($) {

    document.documentElement.className = document.documentElement.className.replace("no-js", "js");

    // Push content on presence of background image
    var mobileWidth = 767;
    var tabletWidth = (1024 - 17);
    var windowHeight = $(window).height();
    var pushHeight = (windowHeight / 4) + "px";
    var groupsOf = 3;
    var bp = 1024;

    $(".show-search-button").click(
        function () {
            $("#mobileSearchInput").slideToggle(220);
            $(".show-search-button").toggleClass("arrow");
        }
    );


    $(window).resize(function () {

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
    });
});
define(["jquery"],function(e){document.documentElement.className=document.documentElement.className.replace("no-js","js");{var o=767,n=1007;e(window).height()}e(".show-search-button").click(function(){e("#mobileSearchInput").slideToggle(220),e(".show-search-button").toggleClass("arrow")}),e(window).resize(function(){e(window).width()>o&&(e(".topic-block-content").length>0||e(".topic-block-content").css("height","56px")),e(window).width()>n&&(e("#mobileSearchInput").hide(),e(".show-search-button").removeClass("arrow"),e("#displayRefineBy").css("display","block"))})});
define(["jquery"], function ($) {

    var self = this;
    var matchboxes = [];

    var populate = function () {

        matchboxes.push(new Matchbox({
            parentSelector: ".matchbox-parent",
            childSelector: ".matchbox-child",
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1024, groupsOf: 3 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".matchbox-parent-4-4-5",
            childSelector: ".matchbox-child",
            groupsOf: 4,
            breakpoints: [
            { bp: 1024, groupsOf: 5 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".matchbox-parent-1-2-2",
            childSelector: ".matchbox-child",
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 }
            ]
        }));
    };

    return {
        Init: function () {
            populate();
            for (var i = 0; i < matchboxes.length; i++) {
                if ($(matchboxes[i].settings.parentSelector).length) { matchboxes[i].init(); }
            }
        }
    };
});


define(["jquery"],function(t){var e=[],o=function(){e.push(new Matchbox({parentSelector:".matchbox-parent",childSelector:".matchbox-child",groupsOf:1,breakpoints:[{bp:767,groupsOf:2},{bp:1024,groupsOf:3}]})),e.push(new Matchbox({parentSelector:".matchbox-parent-4-4-5",childSelector:".matchbox-child",groupsOf:4,breakpoints:[{bp:1024,groupsOf:5}]})),e.push(new Matchbox({parentSelector:".matchbox-parent-1-2-2",childSelector:".matchbox-child",groupsOf:1,breakpoints:[{bp:767,groupsOf:2}]}))};return{Init:function(){o();for(var r=0;r<e.length;r++)t(e[r].settings.parentSelector).length&&e[r].init()}}});
define(["jquery"], function ($) {

    var documentReady = function () {

        $('.alert-close a').on('click', function () {
            $(this).closest('.alert').hide();
        });
    };

    var init = function () {
        $(document).ready(function () {
            documentReady();
        });
    };

    return {
        Init: init
    }
});


define(["jquery"],function(n){var t=function(){n(".alert-close a").on("click",function(){n(this).closest(".alert").hide()})},e=function(){n(document).ready(function(){t()})};return{Init:e}});