// Push content on presence of background image
var mobileWidth = 767;
var tabletWidth = (1024 - 17);
var windowHeight = $(window).height();
var pushHeight = (windowHeight / 4) + "px";
var groupsOf = 3;
var bp = 1024;
var childselector = ".topic-block-content";


$(document).ready(function () {
   
    var matchboxPrimary = new Matchbox({
        parentSelector: ".topic-block-container",
        childSelector: childselector,
        groupsOf: 1,
        breakpoints: [
        { bp: 767, groupsOf: 2 },
        { bp: 1024, groupsOf: 3 }
        ]
    });

    if ($(".topic-block-content").length > 0) {
        matchboxPrimary.init();
    }

    var matchboxTopic = new Matchbox({
        parentSelector: ".article-list-item",
        childSelector: ".article-list-container",
        groupsOf: 1,
        breakpoints: [
        { bp: 767, groupsOf: 2 },
        { bp: 1024, groupsOf: 2 }
        ]
    });

    matchboxTopic.init();
   
    $(".show-search-button").click(
            function () {
                $("#mobileSearchInput").slideToggle(220);
                $(".show-search-button").toggleClass("arrow");
                if ($(".l-body-container-mobile").css("margin-top") !== "0px") {
                    $(".l-body-container-mobile").css("cssText", "margin-top: 0 !important;");
                } else {

                    $(".l-body-container-mobile").css("cssText", "margin-top: 20px !important;");
                }
            }
    );

    if (($('.background-image').css("background-image") !== "none"
        &&
        $('.background-image').css("background-image") !== undefined)
        &&
        ($('body').hasClass('type-topic') || $('body').hasClass('type-article'))) {
        $('.l-body-container-pushed').css("margin-top", pushHeight);
    }
});


$(window)
    .resize(function() {
       
        var matchboxPrimary = new Matchbox({
            parentSelector: ".topic-block-container",
            childSelector: childselector,
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1024, groupsOf: 3 }
            ]
        });

        if ($(window).width() > mobileWidth) {
            if ($(".topic-block-content").length > 0) {
                matchboxPrimary.init();
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