requirejs.config({
    baseUrl: 'assets/javascript/stockportgov',
    paths: {
        "jquery": "https://ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min.js",
        "jquery-ui": "assets/stylesheets/vendor/jquery-ui-1.12.1.custom/jquery-ui.min.js",
        "jquery-validate": "assets/javascript/stockportgov/jquery.validate.min.js",
        "Modernizr": "assets/javascript/stockportgov/modernizr.min.js",
        "Matchbox": "assets/javascript/stockportgov/matchbox.js",
        "Slick": "assets/javascript/stockportgov/slick.js",
        "Recaptcha": "https://www.google.com/recaptcha/api.js",
        "Rangy": "lib/rangy-1.3/rangy-core.js"
    }
});

requirejs(['startup']);
