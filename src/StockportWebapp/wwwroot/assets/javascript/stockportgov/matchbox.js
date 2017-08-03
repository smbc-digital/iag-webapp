define(["jquery"], function ($) {

    var self = this;
    var matchboxes = [];

    var populate = function () {

        matchboxes.push(new Matchbox({
            parentSelector: ".matchbox-parent",
            childSelector: ".matchbox-child",
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 },
            { bp: 1024, groupsOf: 3 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".matchbox-parent-4-4-5",
            childSelector: ".matchbox-child",
            groupsOf: 4,
            breakpoints: [
            { bp: 1024, groupsOf: 5 }
            ]
        }));

        matchboxes.push(new Matchbox({
            parentSelector: ".matchbox-parent-1-2-2",
            childSelector: ".matchbox-child",
            groupsOf: 1,
            breakpoints: [
            { bp: 767, groupsOf: 2 }
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
});
