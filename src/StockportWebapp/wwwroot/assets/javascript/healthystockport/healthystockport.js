define(["jquery"], function ($) {

    document.documentElement.className = document.documentElement.className.replace("no-js", "js");

    // Push content on presence of background image
    var mobileWidth = 767;
    var tabletWidth = (1024 - 17);
    var windowHeight = $(window).height();
    var pushHeight = (windowHeight / 4) + "px";
    var groupsOf = 3;
    var bp = 1024;


    var matchboxPrimary = new Matchbox({
        parentSelector: ".topic-block-list",
        childSelector: ".topic-block",
        groupsOf: 1,
        breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1025, groupsOf: 3 }
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

    if ($(".article-list-container").length > 0) {
        matchboxTopic.init();
    }

    $(".show-search-button").click(
        function () {
            $("#mobileSearchInput").slideToggle(220);
            $(".show-search-button").toggleClass("arrow");
        }
    );


    $(window).resize(function () {

        var matchboxPrimary = new Matchbox({
            parentSelector: ".topic-block-list",
            childSelector: ".topic-block",
            groupsOf: 1,
            breakpoints: [
                { bp: 767, groupsOf: 2 },
                { bp: 1025, groupsOf: 3 }
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
});