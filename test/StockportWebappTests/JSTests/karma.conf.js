// Karma configuration
// Generated on Mon Mar 20 2017 14:43:48 GMT+0000 (GMT Standard Time)

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '',


    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: [
      'jasmine'
     ],
     
     // list of files / patterns to load in the browser
    files: [ 
      'jquery-2.2.2.min.js',
	  'node_modules/jasmine-jquery/lib/jasmine-jquery.js',
      'http://customer.cludo.com/scripts/bundles/search-script.min.js',
      {pattern: '../../../src/StockportWebapp/wwwroot/assets/javascript/vendor/*.js', included: true},
      { pattern: '../../../src/StockportWebapp/wwwroot/assets/javascript/stockportgov.js', included: true },
      { pattern: '../../../src/StockportWebapp/wwwroot/assets/javascript/healthystockport.js', included: true },
      { pattern: '*.js', included: true },
      { pattern: '../../../src/StockportWebapp/wwwroot/assets/stylesheets/vendor/jquery-ui-1.12.1.custom/jquery-ui.min.js', included: true }
    ],


    // list of files to exclude
    exclude: [
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
    },


   

    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress'],


    // web server port
    port: 9878,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['PhantomJS'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false,

    // Concurrency level
    // how many browser should be started simultaneous
    concurrency: Infinity,

    browserNoActivityTimeout: 20000,

    browserDisconnectTolerance: 5
  })
}
