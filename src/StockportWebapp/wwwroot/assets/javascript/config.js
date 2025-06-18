require.config({
    baseUrl: "/assets/javascript/modules/",
    paths: {
        // STOCKPORT MODULES
        "alerts": "alerts-37b64135.min",
        "alertsSetCookie": "alertsSetCookie-928361e1.min",
        "carousel": "carousel-4885f0f7.min",
        "carousel2024": "carousel2024-1e862dcf.min",
        "cludo": "cludo-ae98e22c.min",
        "contactUs": "contactUs-e5b6f8a5.min",
        "directoriesMapInit": "directoriesMapInit-e9b7ff2c.min",
        "directoryFilters": "directoryFilters-cefe3db0.min",
        "events": "events-af550f37.min",
        "eventCategories": "eventCategories-e45a6772.min",
        "eventFilters": "eventFilters-0efc724b.min",
        "filters": "filters-9a822ad2.min",
        "healthystockport": "healthystockport-e14985cb.min",
        "mapSetCookie": "mapSetCookie-75bedf74.min",
        "matchbox": "matchbox-1781b65a.min",
        "multiSelect": "multiSelect-6f2004f5.min",
        "reciteMe": "reciteMe-70c3df4d.min",
        "searchResults": "searchResults-72f65d65.min",
        "startup": "startUp-4a929759.min",
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
