require.config({
    baseUrl: "https://www.stockport.gov.uk/assets/javascript/modules/",
    paths: {
        "startup": "startUp-d5d2cd16.min",
        "utils": "utils-e9b96aa5.min",
        "reciteMe": "reciteMe-70c3df4d.min",
        "cludo": "cludo-b9f1b125.min",

        "jquery": "../vendor/jquery-v3.6.0.min",
        "Cludo": "https://customer.cludo.com/scripts/bundles/search-script.min",
    },
    shim: {
        'Cludo': {
            deps: ['jquery']
        }
    }
});

require(['startup', 'utils', 'reciteMe', 'cludo', 'Cludo', 'jquery'],
    function (startup, utils, reciteMe, cludo) {
        reciteMe.Init();
        startup.Init();
        utils.Init();
        cludo.Init();
    }
);
