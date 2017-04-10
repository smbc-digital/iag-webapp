var matchboxFeaturedItemsShowcase = new Matchbox({
    parentSelector: ".group-listing-container",
    childSelector: ".group-li .group-card",
    groupsOf: 1,
    breakpoints: [
    { bp: 767, groupsOf: 2 },
    { bp: 1024, groupsOf: 3 }
    ]
});

$(document).ready(
    function () {
        if ($(".group-listing-container").length) { matchboxFeaturedItemsShowcase.init(); }
    }()
);