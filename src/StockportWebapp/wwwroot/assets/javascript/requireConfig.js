require.config({
    baseUrl: '/assets/javascript',
    paths: {
        "jquery": "vendor/jquery.min",
        "recaptcha": "//www.google.com/recaptcha/api",
        "bootstrap": "/lib/bootstrap/dist/js/bootstrap.min",
        "handlebars": "/lib/handlebars/handlebars.runtime.min",
        "rangy": "/lib/rangy-1.3/rangy-core",
        "validate": "vendor/jquery.validate.min",
        "unobtrusive": "vendor/jquery.validate.unobtrusive.min",
        "slick": "vendor/slick",
        "carousel": "stockportgov/carousel",
        "contactus": "stockportgov/contactUs",
        "iag-validate-chars": "stockportgov/iagValidateChars",
        "text-limiter": "stockportgov/textLimiter",
        "events": "stockportgov/events",
        "expandinglinks": "stockportgov/expandinglinks",
        "filters": "stockportgov/filters",
        "groups": "stockportgov/Groups",
        "matchboxconfig": "stockportgov/matchbox",
        "multiselect": "stockportgov/multiSelect",
        "primaryfilter": "stockportgov/primaryfilter",
        "refinebybar": "stockportgov/refinebybar",
        "startup": "stockportgov/startup",
        "utils": "stockportgov/utils",
        "viewmoreslider": "stockportgov/viewmoreslider",
        "clipboard": "//cdnjs.cloudflare.com/ajax/libs/clipboard.js/1.7.1/clipboard.min",
        "jquery-ui": "../stylesheets/vendor/jquery-ui-1.12.1.custom/jquery-ui.min",
        "favourites": "stockportgov/favourites",
        "tracking": "stockportgov/tracking",
        "alerts": "stockportgov/alerts",
        "matchHeight": "/lib/matchHeight/dist/jquery.matchHeight-min",
        "jquery.steps": "/assets/javascript/vendor/jquery.steps.min",
        "multiStepForm": "stockportgov/multistep-form",
        "jquery.cookie": "/assets/javascript/vendor/jquery.cookie",
        "trumbowyg": "/lib/trumbowyg/trumbowyg.min",
        "trumbowyginit": "stockportgov/trumbowyginit",
        "reciteMe": "stockportgov/reciteme"
    },
    shim: {
        'jquery.steps': {
            deps: ['jquery', 'jquery.cookie']
        },
        'jquery.cookie': {
            deps: ['jquery']
        },
        'jquery-ui': {
            deps: ['jquery']
        },
        bootstrap: {
            deps: ['jquery', 'jquery-ui']
        },
        unobtrusive: {
            deps: ['validate']
        },
        validate: {
            deps: ['jquery']
        },
        trumbowyg: {
            deps: ['jquery']
        }
    }
});
