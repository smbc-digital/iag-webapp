require.config({
    baseUrl: "https://www.stockport.gov.uk/assets/javascript/modules/",
    paths: {
        "jquery": "../vendor/jquery.min",
        "Cludo": "https://customer.cludo.com/scripts/bundles/search-script.min",
    },
    shim: {
        'Cludo': {
            deps: ['jquery']
        }
    }
});

require(['startup.min', 'utils.min', 'reciteMe.min', 'cludo.min', 'Cludo', 'jquery'],
    function (startup, utils, reciteMe, cludo) {
        reciteMe.Init();
        startup.Init();
        utils.Init();
        cludo.Init();
    }
);
