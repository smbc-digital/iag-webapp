/// <binding BeforeBuild='css, js' ProjectOpened='watch' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    sass = require("gulp-sass"),
    cleancss = require("gulp-clean-css"),
    uglify = require("gulp-uglify"),
    rename = require('gulp-rename'),
    request = require('request'),
    fs = require('fs'),
    colors = require('colors'),
    plumber = require('gulp-plumber'),
    print = require('gulp-print').default,
    lec = require('gulp-line-ending-corrector'),
    replace = require('gulp-replace'),
    cached = require('gulp-cached');

var postcss = require('gulp-postcss');
var reporter = require('postcss-reporter');
var syntax_scss = require('postcss-scss');

var styleguideGitUrl = process.env.STYLEGUIDE_GIT_URL;

// paths
var paths = {
    sass: "./wwwroot/assets/sass/**/*.scss",
    cssDest: "./wwwroot/assets/stylesheets",
    jsProject: "./wwwroot/assets/javascript/**/**/*.js",
    minJs: "./wwwroot/assets/javascript/**/**/*.min.js",
    jsConfig: "./wwwroot/assets/javascript/requireConfig.js",
    jsConfigHS: "./wwwroot/assets/javascript/requireConfigHealthyStockport.js"
};


gulp.task('min:js', function () {
    return gulp.src([paths.jsProject, '!' + paths.minJs, '!' + paths.jsConfig, '!' + paths.jsConfigHS])
        .pipe(cached('js'))
        .pipe(uglify())
        .pipe(rename(function (path) {
            path.extname = ".min.js";
        }))
        .pipe(gulp.dest(function (file) {
            return file.base;
        }));
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


gulp.task('min:config:all', gulp.series('min:config:sg', 'min:config:hs'));

//css
gulp.task('css', function () {
    return gulp.src(paths.sass)
        .pipe(plumber())
        .pipe(sass())
        .pipe(cleancss())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(paths.cssDest))
        .pipe(plumber.stop()).pipe(lec({ verbose: true, eolc: 'CRLF', encoding: 'utf8' }))
        .pipe(print(function (filepath) {
            console.log('Processed: '.yellow + filepath.cyan);
        }));
});

// style guide tasks
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

function lintCss(file) {

    console.log("FILE CHANGED: " + file.path);

    var processors = [
        reporter({
            clearReportedMessages: true
        })
    ];

    return gulp.src(
        [file.path]
    )
        .pipe(postcss(processors, { syntax: syntax_scss }));
}

gulp.task('watch:js',
    function () {
        gulp.watch([paths.jsProject, '!' + paths.minJs], gulp.series('min:js'));
    });

gulp.task('watch:config:sg',
    function () {
        gulp.watch(paths.jsConfig, gulp.series('min:config:sg'));
    });

gulp.task('watch:config:hs',
    function () {
        gulp.watch(paths.jsConfigHS, gulp.series('min:config:hs'));
    });

gulp.task('watch:css',
    function () {
        gulp.watch(paths.sass, gulp.parallel('css'));
    });

gulp.task("scss-lint-all", function () {
    var processors = [
        reporter({
            clearReportedMessages: true
        })
    ];

    return gulp.src(
        ['wwwroot/assets/sass/**/*.scss',
            // Ignore linting vendor assets
            // Useful if you have bower components
            '!wwwroot/assets/sass/site-hs.scss',
            '!wwwroot/assets/sass/site-sg.scss',
            '!wwwroot/assets/sass/site-ts.scss',
            '!wwwroot/assets/sass/export-to-pdf.scss',
            '!wwwroot/assets/sass/thirdsite/**/*.scss']
    )
        .pipe(postcss(processors, { syntax: syntax_scss }));
});

gulp.task('watch', gulp.parallel('watch:js', 'watch:config:sg', 'watch:config:hs', 'watch:css', 'scss-lint-all'));