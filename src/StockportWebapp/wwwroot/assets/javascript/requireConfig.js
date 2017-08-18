requirejs.config({
    baseUrl: '/assets/javascript',
    paths: {
        "jquery": "vendor/jquery.min",
        "recaptcha": "//www.google.com/recaptcha/api",
        "cludo": "//customer.cludo.com/scripts/bundles/search-script.min",
        "bootstrap": "/lib/bootstrap/dist/js/bootstrap.min",
        "handlebars": "/lib/handlebars/handlebars.runtime.min",
        "rangy": "/lib/rangy-1.3/rangy-core",
        "wysiwygtoolbar": "/lib/wysihtml5x/dist/wysihtml5x-toolbar.min",
        "validate": "vendor/jquery.validate.min",
        "unobtrusive": "vendor/jquery.validate.unobtrusive.min",
        "slick": "vendor/slick",
        "carousel": "stockportgov/carousel",
        "cludoconfig": "stockportgov/cludo",
        "contactus": "stockportgov/contactus",
        "customwysiwyg": "stockportgov/customwysiwyg",
        "events": "stockportgov/events",
        "expandinglinks": "stockportgov/expandinglinks",
        "filters": "stockportgov/filters",
        "groups": "stockportgov/groups",
        "livechat": "stockportgov/livechat",
        "matchboxconfig": "stockportgov/matchbox",
        "multiselect": "stockportgov/multiselect",
        "primaryfilter": "stockportgov/primaryfilter",
        "refinebybar": "stockportgov/refinebybar",
        "startup": "stockportgov/startup",
        "utils": "stockportgov/utils",
        "viewmoreslider": "stockportgov/viewmoreslider",
        "questioncontroller": "stockportgov/questioncomponent/questioncontroller",
        "questionmodule": "stockportgov/questioncomponent/questionmodule",
        "questionvalidator": "stockportgov/questioncomponent/questionvalidator",
        "questionview": "stockportgov/questioncomponent/questionview",
        "clipboard": "//cdnjs.cloudflare.com/ajax/libs/clipboard.js/1.7.1/clipboard.min",
        "jquery-ui": "../stylesheets/vendor/jquery-ui-1.12.1.custom/jquery-ui.min",
        "favourite": "stockportgov/favourite",
        "tracking": "stockportgov/tracking",
    },
    shim: {
        'jquery-ui': {
            deps: ['jquery']
        },
        bootstrap: {
            deps: ['jquery', 'jquery-ui']
        },
        wysiwygtoolbar: {
            deps: ['jquery', 'jquery-ui', 'bootstrap', 'rangy']
        },
        customwysiwyg: {
            deps: ['wysiwygtoolbar']
        },
        unobtrusive: {
            deps: ['validate']
        },
        validate: {
            deps: ['jquery']
        },
    }
});

require(['carousel', 'cludoconfig', 'contactus', 'customwysiwyg', 'events', 'expandinglinks', 'filters', 'groups', 'livechat', 'matchboxconfig', 'primaryfilter', 'refinebybar', 'startup', 'viewmoreslider', 'validate', 'recaptcha', 'unobtrusive', 'jquery', 'clipboard', 'jquery-ui', 'favourite', 'tracking'],
    function (carousel, cludoconfig, contactus, customwysiwyg, events, expandinglinks, filters, groups, livechat, matchboxconfig, primaryfilter, refinebybar, startup, viewmoreslider, validate, recaptcha, unobtrusive, $, clipboard, jqueryui, favourite, tracking) {
        carousel.Init();
        cludoconfig.Init();
        contactus.Init();
        customwysiwyg.Init();
        events.Init();
        expandinglinks.Init();
        filters.Init();
        groups.Init();
        livechat.Init();
        matchboxconfig.Init();
        primaryfilter.Init();
        refinebybar.Init();
        startup.Init();
        viewmoreslider.Init();
        favourite.Init();
        tracking.Init();
    }
);

if (typeof (globalButoIds) !== "undefined") {
    require(globalButoIds,
        function () {
        }
    );
}
