$(document)
    .ready(function() {
        var tabletWidth = (1024 - 17);

        if ($(window).width() <= tabletWidth) {
            $(".l-news-filter .collapsable").toggleClass("is-collapsed");
        }
    });

var matchboxNews = new Matchbox({
    parentSelector: '.nav-card-news-list',
    childSelector: '.nav-card-news',
    groupsOf: 1,
    breakpoints: [
    { bp: 767, groupsOf: 2 },
    { bp: 1024, groupsOf: 3 }
    ]
});

if ($(".nav-card-news-list").length) { matchboxNews.init(); }

$(".l-news-filter .filter-title").click(function () {
    $(this).siblings("ul").slideToggle(100, function() {
        $(this).parent().toggleClass("is-collapsed");
    });
});
