requirejs.config({
    baseUrl: '/assets/javascript',
    paths: {
        "jquery": "vendor/jquery.min",
        "recaptcha": "//www.google.com/recaptcha/api",
        "cludo": "//customer.cludo.com/scripts/bundles/search-script.min",
        "validate": "vendor/jquery.validate.min",
        "unobtrusive": "vendor/jquery.validate.unobtrusive.min",
        "slick": "vendor/slick",
        "carousel": "stockportgov/carousel",
        "cludoconfig": "stockportgov/cludo",
        "contactus": "stockportgov/contactus",
        "events": "stockportgov/events",
        "expandinglinks": "stockportgov/expandinglinks",
        "filters": "stockportgov/filters",
        "groups": "stockportgov/groups",
        "matchboxconfig": "stockportgov/matchbox",
        "multiselect": "stockportgov/multiselect",
        "primaryfilter": "stockportgov/primaryfilter",
        "refinebybar": "stockportgov/refinebybar",
        "startup": "stockportgov/startup",
        "utils": "stockportgov/utils",
        "viewmoreslider": "stockportgov/viewmoreslider",
        "jquery-ui": "../stylesheets/vendor/jquery-ui-1.13.0.custom/jquery-ui.min",
        "favourites": "stockportgov/favourites",
        "tracking": "stockportgov/tracking",
        "matchHeight": "/lib/matchHeight/dist/jquery.matchHeight-min"
    },
    shim: {
        'jquery-ui': {
            deps: ['jquery']
        },
        unobtrusive: {
            deps: ['validate']
        },
        validate: {
            deps: ['jquery']
        },
    }
});

require(['carousel', 'cludoconfig', 'contactus', 'events', 'expandinglinks', 'filters', 'groups', 'matchboxconfig', 'primaryfilter', 'refinebybar', 'startup', 'viewmoreslider', 'validate', 'recaptcha', 'unobtrusive', 'jquery', 'clipboard', 'jquery-ui', 'favourites', 'tracking', 'matchHeight'],
    function (carousel, cludoconfig, contactus, events, expandinglinks, filters, groups, matchboxconfig, primaryfilter, refinebybar, startup, viewmoreslider, validate, recaptcha, unobtrusive, $, clipboard, jqueryui, favourites, tracking, matchHeight) {
        carousel.Init();
        cludoconfig.Init();
        contactus.Init();
        events.Init();
        expandinglinks.Init();
        filters.Init();
        groups.Init();
        matchboxconfig.Init();
        primaryfilter.Init();
        refinebybar.Init();
        startup.Init();
        viewmoreslider.Init();
        favourites.Init();
        tracking.Init();
    }
);

if (typeof (globalButoIds) !== "undefined") {
    require(globalButoIds,
        function () {
        }
    );
}
