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