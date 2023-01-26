"use strict";

const { src, parallel, series, dest, watch } = require("gulp");
const sass = require('gulp-sass')(require('sass'));

const autoprefixer = require('autoprefixer');
const postcss = require('gulp-postcss');
const cssnano = require('cssnano');
const gulpStylelint = require('gulp-stylelint');
const uglify = require("gulp-uglify");
const rename = require("gulp-rename");

// Paths
var paths = {
    stylesToLint: [
        "./wwwroot/assets/sass/stockportgov/**/*.scss",
        "./wwwroot/assets/sass/healthystockport/**/*.scss",
        "./wwwroot/assets/sass/thirdsite/**/*.scss",
        "./wwwroot/assets/sass/StockportStyleGuide/**/*.scss"
    ],
    sassToWatch: "./wwwroot/assets/sass/**/*.scss",
    sassToCompile: "./wwwroot/assets/sass/*.scss",
    stylesheets: "./wwwroot/assets/stylesheets",
    js: [
        "./wwwroot/assets/javascript/**/**/*.js",
        "!./wwwroot/assets/javascript/**/**/*.min.js"
    ]
};

// JS
function js() {
    return src(paths.js)
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(dest(function (file) {
            return file.base;
        }));
}

function lintJS() {
    // Look at ES Lint for Gulp
}

// CSS
function css() {
    return src(paths.sassToCompile)
        .pipe(sass.sync().on('error', sass.logError))
        .pipe(postcss([autoprefixer(), cssnano()]))
        .pipe(rename({ suffix: '.min' }))
        .pipe(dest(paths.stylesheets));
};

function lintScss() {
    return src(paths.stylesToLint)
        .pipe(gulpStylelint({
            failAfterError: false,
            fix: true,
            reporters: [
                { formatter: 'string', console: true }
            ]
        }))
}

exports.css = series(css);
exports.js = series(js);
exports.lint = parallel(lintScss); // and lintJS when it's set up
exports.build = series(lintScss, parallel(css, js));
exports.watch = function () {
    watch(paths.sassToWatch, { events: 'change' }, css);
    watch(paths.js, { events: 'change' }, js);
};
