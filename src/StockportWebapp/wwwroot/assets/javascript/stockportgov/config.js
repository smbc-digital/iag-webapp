require.config({
    baseUrl: "assets/javascript/stockportgov/",
    paths: {
        "jquery": "../vendor/jquery.min",
        "modernizr": "../vendor/modernizr.min",
        "slick": "../vendor/slick.min",
        "Cludo": "https://customer.cludo.com/scripts/bundles/search-script.min",
    },
    shim: {
        'Cludo': {
            deps: ['jquery']
        }
    }
});
