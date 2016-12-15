var matchboxTopicsHomepagePrimary = new Matchbox({
    parentSelector: '.featured-topics .featured-topics-primary .featured-topic-list',
    childSelector: '.featured-topic',
    groupsOf: 3,
    breakpoints: [
    { bp: 767, groupsOf: 3 },
    { bp: 1024, groupsOf: 3 }
    ]
});

var matchboxTopicsHomepageSecond = new Matchbox({
    parentSelector: '.featured-topics .primary-topics .featured-topic-list',
    childSelector: '.featured-topic',
    groupsOf: 5,
    breakpoints: [
    { bp: 767, groupsOf: 5 },
    { bp: 1024, groupsOf: 5 }
    ]
});

var matchboxTopicsHomepageMore = new Matchbox({
    parentSelector: '.featured-topics #more-topics .featured-topic-list.hide-on-mobile',
    childSelector: '.featured-topic',
    groupsOf: 5,
    breakpoints: [
    { bp: 767, groupsOf: 5 },
    { bp: 1024, groupsOf: 5 }
    ]
});

var matchboxTopicsHomepageMoreMobile = new Matchbox({
    parentSelector: '.featured-topics #more-topics .featured-topic-list.hide-on-desktop',
    childSelector: '.featured-topic',
    groupsOf: 5,
    breakpoints: [
    { bp: 767, groupsOf: 4 },
    { bp: 1024, groupsOf: 4 }
    ]
});

var matchboxNewsHomepage = new Matchbox({
    parentSelector: '.homepage-news-items',
    childSelector: '.homepage-news-item',
    groupsOf: 2
});

var $seeMoreServicesButton = $("#see-more-services, #see-more-services-mobile");
var $moreFeaturedTopicsDiv = $("#more-topics");

$(document).ready(
    function () {
        if ($(".featured-topics .featured-topics-primary").length) { matchboxTopicsHomepagePrimary.init(); }
        if ($(".featured-topics .primary-topics").length) { matchboxTopicsHomepageSecond.init(); }
        if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMore.init(); }
        if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMoreMobile.init(); }
        if ($(".homepage-news-items").length) { matchboxNewsHomepage.init(); }

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
        if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMore.init(); }
        if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMoreMobile.init(); }
    }
);