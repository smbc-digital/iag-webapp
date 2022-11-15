﻿var allTestFiles = [];
var TEST_REGEXP = /(spec|test)\.js$/i;

// Get a list of all the test files to include
Object.keys(window.__karma__.files).forEach(function (file) {

    if (TEST_REGEXP.test(file)) {
        // Normalize paths to RequireJS module names.
        // If you require sub-dependencies of test files to be loaded as-is (requiring file extension)
        // then do not normalize the paths
        var normalizedTestModule = file.replace(/^\/base\/|\.js$/g, '');
        allTestFiles.push(normalizedTestModule);
    }
});

requirejs.config({
    baseUrl: "/base/",
    paths: {
        "jquery": "src/StockportWebapp/wwwroot/assets/javascript/vendor/jquery.min",
        "modernizr": "src/StockportWebapp/wwwroot/assets/javascript/vendor/modernizr.min",
        "events": "src/StockportWebapp/wwwroot/assets/javascript/modules/events",
        "utils": "src/StockportWebapp/wwwroot/assets/javascript/modules/utils"
    },
    deps: allTestFiles,
    callback: window.__karma__.start
});
