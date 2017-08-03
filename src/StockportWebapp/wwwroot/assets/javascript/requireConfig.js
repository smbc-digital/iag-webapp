requirejs.config({
    baseUrl: '/assets/javascript',
    paths: {
        "jquery": "https://ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min",
        "recaptcha": "https://www.google.com/recaptcha/api",
        "cludo": "http://customer.cludo.com/scripts/bundles/search-script.min",
        "jquery-ui": "/assets/stylesheets/vendor/jquery-ui-1.12.1.custom/jquery-ui.min",
        "bootstrap": "/lib/bootstrap/dist/js/bootstrap.min",
        "handlebars": "/lib/handlebars/handlebars.runtime.min",
        "rangy": "/lib/rangy-1.3/rangy-core",
        "wysiwygtoolbar": "/lib/wysihtml5x/dist/wysihtml5x-toolbar.min",
        "jquery-validate": "vendor/jquery.validate.min",
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
    },
    shim: {
        bootstrap: {
            deps: ['jquery']
        },
        wysiwygtoolbar: {
            deps: ['rangy']
        },
        customwysiwyg: {
            deps: ['wysiwygtoolbar']
        }
    }
});

require(['carousel', 'cludoconfig', 'contactus', 'customwysiwyg', 'events', 'expandinglinks', 'filters', 'groups', 'livechat', 'matchboxconfig', 'primaryfilter', 'refinebybar', 'startup', 'viewmoreslider'],
        function (carousel, cludoconfig, contactus, customwysiwyg, events, expandinglinks, filters, groups, livechat, matchboxconfig, primaryfilter, refinebybar, startup, viewmoreslider) {
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
        }
);



