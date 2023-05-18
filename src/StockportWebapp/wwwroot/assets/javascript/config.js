require.config({
    baseUrl: "/assets/javascript/modules/",
    paths: {
        // STOCKPORT MODULES
        "alerts": "alerts-93475f72.min",
        "carousel": "carousel-719cf06c.min",
        "cludo": "cludo-b9f1b125.min",
        "contactUs": "contactUs-e5b6f8a5.min",
        "events": "events-af550f37.min",
        "expandingLinks": "expandingLinks-2fa18259.min",
        "favourites": "favourites-9929284b.min",
        "filters": "filters-4ba05ae6.min",
        "groups": "groups-e4d2ff31.min",
        "healthystockport": "healthystockport-e14985cb.min",
        "matchbox": "matchbox-1781b65a.min",
        "multiSelect": "multiSelect-6f2004f5.min",
        "multistepForm": "multistepForm-3bf1580e.min",
        "primaryFilter": "primaryFilter-18bf75d0.min",
        "reciteMe": "reciteMe-70c3df4d.min",
        "refineByBar": "refineByBar-c5f96eb6.min",
        "startup": "startUp-d5d2cd16.min",
        "utils": "utils-e9b96aa5.min",
        "viewMoreSlider": "viewMoreSlider-18c0a669.min",

        // VENDOR
        "jquery": "../vendor/jquery-v3.6.0.min",
        "jquery.steps": "../vendor/jquery.steps-e3d787b4.min",
        "jquery.cookie": "../vendor/jquery.cookie-481af348.min",
        "validate": "../vendor/jquery.validate-v1.9.0.min",
        "unobtrusive": "../vendor/jquery.validate.unobtrusive-v1.0.0.min",
        "modernizr": "../vendor/modernizr-v3.3.1.min",
        "slick": "../vendor/slick-3da97841.min",
        "trumbowyg": "../vendor/trumbowyg/trumbowyg-v2.8.1.min",
        "Cludo": "https://customer.cludo.com/scripts/bundles/search-script.min",
        "recaptcha": "https://www.google.com/recaptcha/api",
        "google.places": "https://maps.googleapis.com/maps/api/js?key=AIzaSyCPMO2lE7np9cG_zG63JH_pNVmRfoajZjg&libraries=places"
    },
    shim: {
        'Cludo': {
            deps: ['jquery']
        },
        'jquery.steps': {
            deps: ['jquery', 'jquery.cookie']
        },
        'jquery.cookie': {
            deps: ['jquery']
        },
        'trumbowyg': {
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
