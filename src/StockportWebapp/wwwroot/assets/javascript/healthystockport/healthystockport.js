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