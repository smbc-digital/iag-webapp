// Push content on presence of background image
var mobileWidth = 767;
var tabletWidth = (1024 - 17);
var windowHeight = $(window).height();
var pushHeight = (windowHeight / 2) + "px";

$(document).ready(function () {
    $(".show-search-button").click(
            function () {
                $("#mobileSearchInput").slideToggle(220);
                $(".show-search-button").toggleClass("arrow");
               
                if ($(".l-body-container-mobile").css("margin-top") != "0px") {
                    $(".l-body-container-mobile").css("cssText", "margin-top: 0 !important;");
                } else {
                    
                    $(".l-body-container-mobile").css("cssText", "margin-top: 60px !important;");
                }
            }
    );

    if (($('.background-image').css("background-image") !== "none"
        &&
        $('.background-image').css("background-image") !== undefined)
        &&
        ($('body').hasClass('type-topic') || $('body').hasClass('type-article')))
        {
            $('.l-body-container-pushed').css("margin-top", pushHeight);
        }
});

// Fade background image on scroll

var target = $('.background-image');
var targetHeight = $(window).height();

$(document).scroll(function (e) {
    

    if (!$('body').hasClass('type-home')) {
        var y_scroll_pos = $(this).scrollTop();
        var scrollPercent = (targetHeight - (y_scroll_pos * 2.0)) / targetHeight;
        if (scrollPercent >= 0) {
            target.css('opacity', scrollPercent);
        }
        if (scrollPercent <= 0.02) {
            target.css('opacity', 0);
        }
    }
});

$(window).resize(function () {
    if ($(window).width() > tabletWidth) {
        $("#mobileSearchInput").hide();
        $(".show-search-button").removeClass("arrow");
        $('#displayRefineBy').css('display', 'block');
    }
});