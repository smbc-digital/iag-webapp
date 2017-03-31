var matchboxFeaturedItemsShowcase = new Matchbox({
    parentSelector: ".featured-items-wrapper",
    childSelector: '.featured-topic',
    groupsOf: 3,
    breakpoints: [
    { bp: 767, groupsOf: 4 },
    { bp: 1024, groupsOf: 5 }
    ]
});

$(document).ready(
    function () {
        if ($(".featured-items-wrapper").length) { matchboxFeaturedItemsShowcase.init(); }
    }()
);