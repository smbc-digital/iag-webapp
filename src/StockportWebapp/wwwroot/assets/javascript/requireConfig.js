require.config({
    baseUrl: '/assets/javascript',
    paths: {
        "jquery": "vendor/jquery.min",
        "recaptcha": "//www.google.com/recaptcha/api",
        "validate": "vendor/jquery.validate.min",
        "unobtrusive": "vendor/jquery.validate.unobtrusive.min",
        "slick": "vendor/slick",
        "carousel": "stockportgov/carousel",
        "contactus": "stockportgov/contactUs",
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
        "jquery-ui": "../stylesheets/vendor/jquery-ui-1.13.0.custom/jquery-ui.min",
        "favourites": "stockportgov/favourites",
        "tracking": "stockportgov/tracking",
        "alerts": "stockportgov/alerts",
        "matchHeight": "/lib/matchHeight/dist/jquery.matchHeight-min",
        "jquery.steps": "/assets/javascript/vendor/jquery.steps.min",
        "multiStepForm": "stockportgov/multistep-form",
        "jquery.cookie": "/assets/javascript/vendor/jquery.cookie",
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
        unobtrusive: {
            deps: ['validate']
        },
        validate: {
            deps: ['jquery']
        }
    }
});
