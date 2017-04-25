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

$(window).resize(function () {
    if ($(window).width() > tabletWidth) {
        $("#mobileSearchInput").hide();
        $(".show-search-button").removeClass("arrow");
        $('#displayRefineBy').css('display', 'block');
    }
});