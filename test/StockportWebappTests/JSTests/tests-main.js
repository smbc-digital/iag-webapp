var tests = [];
for (var file in window.__karma__.files) {
    if (window.__karma__.files.hasOwnProperty(file)) {
        if (/Test\.js$/.test(file)) {
            tests.push(file);
        }
    }
}

requirejs.config({
    baseUrl: '../../../src/StockportWebapp/wwwroot/assets/javascript',
    paths: {
        "jquery": "https://ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min",
        "events": "stockportgov/events",
        "utils": "stockportgov/utils",
    },
    deps: tests,
    callback: window.__karma__.start
});
