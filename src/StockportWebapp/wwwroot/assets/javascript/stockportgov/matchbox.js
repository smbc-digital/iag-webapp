var STK = {};

STK.Matchboxes = (function () {

    var self = this;
    var matchboxes = [];

    var populate = function () {

        matchboxes.push(new Matchbox({
            parentSelector: ".group-listing-container",
            childSelector: ".group-li .group-card",
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1024, groupsOf: 3 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".featured-category-items-wrapper",
            childSelector: '.featured-group-category',
            groupsOf: 3,
            breakpoints: [
            { bp: 767, groupsOf: 5 },
            { bp: 1024, groupsOf: 5 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: '.primary-feature-box',
            childSelector: '.primary-feature-match',
            groupsOf: 3
        }));

        matchboxes.push(new Matchbox({
            parentSelector: '.featured-topics .primary-topics .featured-topic-list',
            childSelector: '.featured-topic',
            groupsOf: 5,
            breakpoints: [
            { bp: 767, groupsOf: 5 },
            { bp: 1024, groupsOf: 5 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: '.featured-topics #more-topics .featured-topic-list.hide-on-mobile',
            childSelector: '.featured-topic',
            groupsOf: 5,
            breakpoints: [
            { bp: 767, groupsOf: 5 },
            { bp: 1024, groupsOf: 5 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: '.featured-topics #more-topics .featured-topic-list.hide-on-desktop',
            childSelector: '.featured-topic',
            groupsOf: 5,
            breakpoints: [
            { bp: 767, groupsOf: 4 },
            { bp: 1024, groupsOf: 4 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: '.homepage-news-items',
            childSelector: '.homepage-news-item',
            groupsOf: 2
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".latest-container",
            childSelector: ".latest-nav-card-item",
            groupsOf: 1,
            breakpoints: [
            { bp: 1024, groupsOf: 3 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".featured-items-wrapper",
            childSelector: '.featured-topic',
            groupsOf: 3,
            breakpoints: [
            { bp: 767, groupsOf: 4 },
            { bp: 1024, groupsOf: 5 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".sk-table-body.primary-items",
            childSelector: ".subitem",
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1024, groupsOf: 2 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".l-page-content .nav-card-list",
            childSelector: ".nav-card .nav-card-item",
            groupsOf: 1,
            breakpoints: [
                { bp: 1024, groupsOf: 3 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".event-listing-container",
            childSelector: ".event-card-information",
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1024, groupsOf: 3 }
            ]
        }));

    };

    return {
        Init: function () {
            populate();
            for (var i = 0; i < matchboxes.length; i++) {
                if ($(matchboxes[i].settings.parentSelector).length) { matchboxes[i].init(); }
            }
        }
    };
})();

STK.Matchboxes.Init();
