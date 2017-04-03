var matchboxFeaturedItemsShowcase = new Matchbox({
    parentSelector: ".featured-category-items-wrapper",
    childSelector: '.featured-group-category',
    groupsOf: 3,
    breakpoints: [
    { bp: 767, groupsOf: 5 },
    { bp: 1024, groupsOf: 5 }
    ]
});

$(document).ready(
    function () {
        if ($(".featured-category-items-wrapper").length) { matchboxFeaturedItemsShowcase.init(); }
    }()
);