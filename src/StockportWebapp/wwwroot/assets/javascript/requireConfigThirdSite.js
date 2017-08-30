requirejs.config({
    baseUrl: '/assets/javascript',
    paths: {
        "jquery": "//ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min",
        "cludo": "//customer.cludo.com/scripts/bundles/search-script.min",
        "jquery-ui": "/assets/stylesheets/vendor/jquery-ui-1.12.1.custom/jquery-ui.min",
        "validate": "vendor/jquery.validate.min",
        "unobtrusive": "vendor/jquery.validate.unobtrusive.min",
        "slick": "vendor/slick",
        "carousel": "thirdsite/carousel",
        "cludoconfig": "thirdsite/cludo",
        "thirdsite": "thirdsite/thirdsite",
        "matchboxconfig": "thirdsite/matchbox",
        "startup": "thirdsite/startup",
    },
    shim: {
        unobtrusive: {
            deps: ['validate']
        },
        validate: {
            deps: ['jquery']
        },
        carousel: {
            deps: ['slick']
        }
    }
});

require(['carousel', 'cludoconfig', 'thirdsite', 'matchboxconfig', 'validate', 'unobtrusive', 'jquery', 'startup'],
    function (carousel, cludoconfig, thirdsite, matchboxconfig, validate, unobtrusive, $, startup) {
        matchboxconfig.Init();
        startup.Init();
    }
);



