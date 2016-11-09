var matchboxPrimary = new Matchbox({
    parentSelector: '.featured-topics-primary .featured-topic-list',
    childSelector: '.featured-topic',
    groupsOf: 4,
    breakpoints: [
    { bp: 767, groupsOf: 4 },
    { bp: 1024, groupsOf: 4 }
    ]
});
var matchboxSecondary = new Matchbox({
    parentSelector: '.featured-topics-secondary .featured-topic-list',
    childSelector: '.featured-topic',
    groupsOf: 5,
    breakpoints: [
    { bp: 767, groupsOf: 5 },
    { bp: 1024, groupsOf: 5 }
    ]
});
var matchboxNewsHomepage = new Matchbox({
    parentSelector: '.homepage-news-items',
    childSelector: '.homepage-news-item',
    groupsOf: 2
});

var $seeMoreServicesButton = $("#see-more-services, #see-more-services-mobile");
var $moreFeaturedTopicsDiv = $("#more-topics");

if ($(".featured-topics-primary .featured-topic-list").length) { matchboxPrimary.init(); }
if ($(".featured-topics-secondary .featured-topic-list").length) { matchboxSecondary.init(); }
if ($(".homepage-news-items").length) { matchboxNewsHomepage.init(); }

$(document).ready(
    function () {
        $moreFeaturedTopicsDiv.hide();
        $seeMoreServicesButton.addClass("is-visible");       
    }()
);

$seeMoreServicesButton.on(
    "click", function () {
        $moreFeaturedTopicsDiv.slideToggle(200, function () {
            if (!$seeMoreServicesButton.hasClass("is-collapsed")) {
                $seeMoreServicesButton.text("See fewer services");
                $seeMoreServicesButton.toggleClass("is-collapsed");

            }
            else {
                $seeMoreServicesButton.text("See more services");
                $seeMoreServicesButton.toggleClass("is-collapsed");
            }
        }
        );
    }
);

$('.featured-topics-primary .featured-topic .featured-topic-content a.arrow')
    .on('click', function () {
        if ($(this).hasClass("arrow-hidden")) {
            var openTopic = $('.featured-topics-primary  .featured-topic  .featured-topic-content  a.arrow:not(.arrow-hidden)');
            if (openTopic.length !== 0) {
                toggleFeaturedTopic(openTopic, this, toggleFeaturedTopic);
                return false;
            }
        }
        toggleFeaturedTopic(this);
        return false;
    });

var toggleFeaturedTopic = function (topic, topicToChangeAfter, changeTopicAfter) {
    $(topic).toggleClass("arrow-hidden");
    var child = $(topic).data("child");
    $("#" + child).slideToggle(200, function () {
        $("#" + child).toggleClass("hidden");
        if (changeTopicAfter != null) {
            changeTopicAfter(topicToChangeAfter);
        }
    });
}