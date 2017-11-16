requirejs.config({
    baseUrl: '/assets/javascript',
    paths: {
        "jquery": "//ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min",
        "cludo": "//customer.cludo.com/scripts/bundles/search-script.min",
        "jquery-ui": "/assets/stylesheets/vendor/jquery-ui-1.12.1.custom/jquery-ui.min",
        "validate": "vendor/jquery.validate.min",
        "unobtrusive": "vendor/jquery.validate.unobtrusive.min",
        "slick": "vendor/slick",
        "carousel": "healthystockport/carousel",
        "cludoconfig": "healthystockport/cludo",
        "healthystockport": "healthystockport/healthystockport",
        "matchboxconfig": "healthystockport/matchbox",
        "startup": "healthystockport/startup",
        "matchHeight": "/lib/matchHeight/dist/jquery.matchHeight-min"
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

require(['carousel', 'cludoconfig', 'healthystockport', 'matchboxconfig', 'validate', 'unobtrusive', 'jquery', 'startup', 'matchHeight'],
    function (carousel, cludoconfig, healthystockport, matchboxconfig, validate, unobtrusive, $, startup, matchHeight) {
        startup.Init();
        matchboxconfig.Init();
    }
);

if (typeof (globalButoIds) !== "undefined") {
    require(globalButoIds,
        function () {
        }
    );
}



