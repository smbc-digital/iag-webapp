requirejs.config({
    baseUrl: '/assets/javascript',
    paths: {
        "jquery": "//ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min",
        "cludo": "//customer.cludo.com/scripts/bundles/search-script.min",
        "validate": "vendor/jquery.validate.min",
        "unobtrusive": "vendor/jquery.validate.unobtrusive.min",
        "slick": "vendor/slick",
        "carousel": "healthystockport/carousel",
        "healthystockport": "healthystockport/healthystockport",
        "matchboxconfig": "healthystockport/matchbox",
        "startup": "healthystockport/startup",
        "matchHeight": "/lib/matchHeight/dist/jquery.matchHeight-min",
        "reciteMe": "healthystockport/reciteme"
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

require(['carousel', 'healthystockport', 'matchboxconfig', 'validate', 'unobtrusive', 'jquery', 'startup', 'matchHeight', 'reciteMe'],
    function (carousel, healthystockport, matchboxconfig, validate, unobtrusive, $, startup, matchHeight, reciteMe) {
        startup.Init();
        matchboxconfig.Init();
        reciteMe.Init();
    }
);

if (typeof (globalButoIds) !== "undefined") {
    require(globalButoIds,
        function () {
        }
    );
}



