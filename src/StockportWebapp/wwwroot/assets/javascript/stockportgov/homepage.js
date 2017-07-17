
var $seeMoreServicesButton = $("#see-more-services, #see-more-services-mobile");
var $moreFeaturedTopicsDiv = $("#more-topics");

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
        //if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMore.init(); }
        //if ($(".featured-topics #more-topics").length) { matchboxTopicsHomepageMoreMobile.init(); }
    }
);