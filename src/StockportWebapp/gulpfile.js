/// <binding BeforeBuild='css, js' ProjectOpened='watch' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    sass = require("gulp-sass"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    rename = require('gulp-rename'),
    request = require('request'),
    source = require('vinyl-source-stream'),
    fs = require('fs'),
    colors = require('colors'),
    plumber = require('gulp-plumber'),
    print = require('gulp-print'),
    lec = require('gulp-line-ending-corrector'),
    replace = require('gulp-replace');

var styleguideGitUrl = process.env.STYLEGUIDE_GIT_URL;

// paths
var paths = {
    sass: "./wwwroot/assets/sass/**/*.scss",
    cssDest: "./wwwroot/assets/stylesheets",
    jsProject: "./wwwroot/assets/javascript/stockportgov/**/*.js",
    jsConfig: "./wwwroot/assets/javascript/requireConfig.js",
    jsConfigHS: "./wwwroot/assets/javascript/requireConfigHealthyStockport.js",
    concatJsDest: "./wwwroot/assets/javascript/stockportgov.min.js",
    concatFullJsDest: "./wwwroot/assets/javascript/stockportgov.js",
    jsSmart: "./wwwroot/assets/javascript/stockportgov/QuestionComponent/*.js",
    jsProjectHS: "./wwwroot/assets/javascript/healthystockport/*.js",
    concatJsDestHS: "./wwwroot/assets/javascript/healthystockport.min.js",
    concatFullJsDestHS: "./wwwroot/assets/javascript/healthystockport.js",
    jsVendor: "./wwwroot/assets/javascript/vendor/*.js",
    minJs: "./wwwroot/assets/javascript/**/*.min.js",
};

gulp.task("js", ['min:js:sg', 'min:config:sg', 'min:js:hs', 'min:config:hs']);

gulp.task('min:js:sg', function () {
    return gulp.src([paths.jsProject, '!' + paths.minJs,])
        .pipe(uglify())
        .pipe(rename(function (path) {
            path.extname = ".min.js";
        }))
        .pipe(gulp.dest('./wwwroot/assets/javascript/stockportgov'));
});

gulp.task('min:config:sg', function () {
    return gulp.src(paths.jsConfig)
        .pipe(replace(/stockportgov\/(.+)\"/g, function (match) {
            return match.replace('"', '.min"');
        }))
        .pipe(uglify())
        .pipe(rename(function (path) {
            path.extname = ".min.js";
        }))
        .pipe(gulp.dest('./wwwroot/assets/javascript'));
});

gulp.task('min:js:hs', function () {
    return gulp.src([paths.jsProjectHS, '!' + paths.minJs,])
        .pipe(uglify())
        .pipe(rename(function (path) {
            path.extname = ".min.js";
        }))
        .pipe(gulp.dest('./wwwroot/assets/javascript/healthystockport'));
});

gulp.task('min:config:hs', function () {
    return gulp.src(paths.jsConfigHS)
        .pipe(replace(/healthystockport\/(.+)\"/g, function (match) {
            return match.replace('"', '.min"');
        }))
        .pipe(uglify())
        .pipe(rename(function (path) {
            path.extname = ".min.js";
        }))
        .pipe(gulp.dest('./wwwroot/assets/javascript'));
});


////js sg
//gulp.task("min:js:sg", function () {
//    return gulp.src([paths.jsProject, "!" + paths.minJs], { base: "." })
//        .pipe(plumber())
//        .pipe(concat(paths.concatJsDest))
//        .pipe(uglify())
//        .pipe(gulp.dest("."))
//        .pipe(plumber.stop())
//        .pipe(print(function (filepath) {
//            console.log('Processed: '.yellow + filepath.cyan);
//        }));
//});

////js sg no min
//gulp.task("js:sg", function () {
//    return gulp.src([paths.jsProject, "!" + paths.minJs, paths.jsSmart], { base: "." })
//        .pipe(plumber())
//        .pipe(concat(paths.concatFullJsDest))
//        .pipe(gulp.dest("."))
//        .pipe(plumber.stop())
//        .pipe(lec({ verbose: true, eolc: 'CRLF', encoding: 'utf8' }))
//        .pipe(print(function (filepath) {
//            console.log('Processed: '.yellow + filepath.cyan);
//        }));
//});

////js hs
//gulp.task("min:js:hs", function () {
//    return gulp.src([paths.jsProjectHS, "!" + paths.minJs], { base: "." })
//        .pipe(plumber())
//        .pipe(concat(paths.concatJsDestHS))
//        .pipe(uglify())
//        .pipe(gulp.dest("."))
//        .pipe(plumber.stop())
//        .pipe(lec({ verbose: true, eolc: 'CRLF', encoding: 'utf8' }))
//        .pipe(print(function (filepath) {
//            console.log('Processed: '.yellow + filepath.cyan);
//        }));
//});

////js hs no min
//gulp.task("js:hs", function () {
//    return gulp.src([paths.jsProjectHS, "!" + paths.minJs], { base: "." })
//        .pipe(plumber())
//        .pipe(concat(paths.concatFullJsDestHS))
//        .pipe(gulp.dest("."))
//        .pipe(plumber.stop())
//        .pipe(print(function (filepath) {
//            console.log('Processed: '.yellow + filepath.cyan);
//        }));
//});

//js vendor
gulp.task("min:js:vendor", function () {
    return gulp.src([paths.jsVendor, "!" + paths.jsVendorMin], { base: "." })
        .pipe(plumber())
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest("."))
        .pipe(plumber.stop())
        .pipe(print(function (filepath) {
            console.log('Processed vendor js: '.yellow + filepath.cyan);
        }));
});

//css
gulp.task('css', function () {
    return gulp.src(paths.sass)
        .pipe(plumber())
        .pipe(sass())
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(paths.cssDest))
        .pipe(plumber.stop())
        .pipe(lec({ verbose: true, eolc: 'CRLF', encoding: 'utf8' }))
        .pipe(print(function (filepath) {
            console.log('Processed: '.yellow + filepath.cyan);
        }));
});

gulp.task('pull-styleguide', function () {
    pullArtifacts('master');
});

gulp.task('pull-styleguide-from-tag', function () {
    fs.readFile("../../StyleGuideVersion.lock", function (err, data) {
        if (err) {
            console.log('There was an error reading the StyleGuideVersion.lock file: ' + err);
            throw err;
        } else {
            var version = data.toString().trim();
            console.log(colors.yellow("Pulling artifacts from tag " + version));

            pullArtifacts(version);
        }
    });
});

var pullAssetFile = function (file, outputDir, outputFile, version) {
    var styleguideFolder = "/src/StockportStyleGuide/wwwroot/styleguide-artifacts/";
    pullFile(file, outputDir, outputFile, version, styleguideFolder);
};

var pullCodeFile = function (file, outputDir, outputFile, version) {
    var codeFolder = "/src/StockportTagHelpers/";
    pullFile(file, outputDir, outputFile, version, codeFolder);
};

var pullFile = function (file, outputDir, outputFile, version, fromFolder) {
    var url = styleguideGitUrl + version + fromFolder + file;

    var options = {
        url: url,
        'rejectUnauthorized': false,
        headers: {
            'User-Agent': 'request'
        }
    };

    request
        .get(options,
        function (error, response, body) {
            if (error || body.startsWith("<!DOCTYPE html>") || response.statusCode !== 200) {
                console.log("There was an error requesting: ".red);
                if (error) console.log(colors.white(error));
            } else {
                console.log("Successfully pulled artifact from: ".yellow);
                fs.writeFile(outputDir + '/' + outputFile, body);
            }
            console.log(url.cyan);
        });
};

var pullArtifacts = function (version) {
    if (styleguideGitUrl === undefined || styleguideGitUrl === "") {
        console.log("STYLEGUIDE_GIT_URL environment variable is not set, this needs to be set to be able to pull artifacts from the styleguide git repository".red);
    } else {
        pullAssetFile("styleguide-hs.min.css", "wwwroot/assets/stylesheets", "styleguide-hs.min.css", version);
        pullAssetFile("styleguide-sg.min.css", "wwwroot/assets/stylesheets", "styleguide-sg.min.css", version);
        pullAssetFile("_color-palette-sg.scss", "wwwroot/assets/sass/styleguide", "_colors-sg.scss", version);
        pullAssetFile("_devices.scss", "wwwroot/assets/sass/styleguide", "_devices.scss", version);
        pullCodeFile("ButtonTagHelpers.cs", "StockportTagHelpers", "ButtonTagHelpers.cs", version);
        pullCodeFile("ProfileTagHelpers.cs", "StockportTagHelpers", "ProfileTagHelpers.cs", version);
        pullCodeFile("HtmlAttributes.cs", "StockportTagHelpers", "HtmlAttributes.cs", version);
    }
};

function swallowError(error) {
    console.log('There was an error'.underline.red);
    console.log(colors.cyan(error.toString()));
    this.emit('end');
}

//watch
gulp.task('watch', function () {
    gulp.watch("wwwroot/assets/javascript/stockportgov/*.js", ['js:sg', 'min:js:sg']);
    gulp.watch("wwwroot/assets/javascript/stockportgov/**/*.js", ['js:sg', 'min:js:sg']);
    gulp.watch("wwwroot/assets/javascript/healthystockport/*.js", ['js:hs', 'min:js:hs']);
    gulp.watch("wwwroot/assets/javascript/site.js", ['js:sg', 'js:hs', 'min:js:hs', 'min:js:sg']);
    gulp.watch("wwwroot/assets/javascript/stockportgov/QuestionComponent/*.js", ['js:sg', 'min:js:sg']);
    gulp.watch("wwwroot/assets/javascript/stockportgov/vendor/*.js", ['min:js:vendor']);
    gulp.watch("wwwroot/assets/sass/**/*.scss", ['css']);
});
