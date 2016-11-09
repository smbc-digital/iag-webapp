// Push content on presence of background image

var windowHeight = $(window).height();
var pushHeight = (windowHeight / 2) + "px";

$(document).ready(function () {
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