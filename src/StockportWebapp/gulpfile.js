/// <binding BeforeBuild='css, min:js' Clean='clean' ProjectOpened='watch' />
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
    colors = require('colors');

var businessId = process.env.BUSINESS_ID;
var styleguideGitUrl = process.env.STYLEGUIDE_GIT_URL;

// paths
var paths = {
     webroot: "./wwwroot/",
     sass: "./wwwroot/assets/sass/**/*.scss",
     js: "./wwwroot/assets/javascript/**/*.js",
     cssDest: "./wwwroot/assets/stylesheets",
     cleanArtifacts: "./wwwroot/assets/stylesheets/*.min.css",
     jsSite: "./wwwroot/assets/javascript/site.js",
     jsProject: "./wwwroot/assets/javascript/stockportgov/*.js",
     minJs: "./wwwroot/assets/javascript/*.min.js",
     concatJsDest: "./wwwroot/assets/javascript/stockportgov.min.js",
     jsProjectHS: "./wwwroot/assets/javascript/healthystockport/*.js",
     concatJsDestHS: "./wwwroot/assets/javascript/healthystockport.min.js"
};

//clean 
gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    // TODO: Clean only styleguide css
    rimraf(paths.cleanArtifacts, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

//js sg
gulp.task("min:js:sg", function () {
    return gulp.src([paths.jsSite, paths.jsProject, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."))
        .on('end', function() {
            console.log('Successfully minified js files to: '.yellow);
            console.log(paths.concatJsDest.cyan);
        });
});

//js hs
gulp.task("min:js:hs", function () {
    return gulp.src([paths.jsSite, paths.jsProjectHS, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDestHS))
        .pipe(uglify())
        .pipe(gulp.dest("."))
        .on('end', function () {
            console.log('Successfully minified js files to: '.yellow);
            console.log(paths.concatJsDestHS.cyan);
        });
});

//css
gulp.task('css', function () {
    return gulp.src(paths.sass)
        .pipe(sass())
        .on('error', function(err) {
            console.log('There was an error processing the sass files'.underline.red);
            console.log(colors.cyan(err.toString()));
        })
        .pipe(cssmin())
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest(paths.cssDest))
        .on('end', function() { console.log('Successfully processed the sass files to css'.bold.yellow); });
});

gulp.task('pull-styleguide', function () {
    pullArtifacts('master');
});

gulp.task('pull-styleguide-from-tag',
    function () {
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
    }
);

var pullFile = function (file, outputDir, outputFile, version) {
    var url = styleguideGitUrl + version + "/src/StockportStyleGuide/wwwroot/styleguide-artifacts/" + file;

    var options = {
        url: url,
        'rejectUnauthorized': false,
        headers: {
            'User-Agent': 'request'
        }
    };

    request
        .get(options,
            function(error, response, body) {
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
        pullFile("styleguide-hs.min.css", "wwwroot/assets/stylesheets", "styleguide-hs.min.css", version);
        pullFile("styleguide-sg.min.css", "wwwroot/assets/stylesheets", "styleguide-sg.min.css", version);
        pullFile("_color-palette-sg.scss", "wwwroot/assets/sass/styleguide", "_colors-sg.scss", version);
        pullFile("_devices.scss", "wwwroot/assets/sass/styleguide", "_devices.scss", version);
    }
};

//watch
gulp.task('watch', function () {
    gulp.watch("wwwroot/assets/javascript/**/*.js", ['min:js:hs', 'min:js:sg']);
    gulp.watch("wwwroot/assets/sass/**/*.scss", ['css']);
});
