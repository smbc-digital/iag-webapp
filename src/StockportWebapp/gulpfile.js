"use strict";

const { src, parallel, series, dest, watch } = require("gulp");
const sass = require('gulp-sass')(require('sass'));

const autoprefixer = require('autoprefixer');
const postcss = require('gulp-postcss');
const cssnano = require('cssnano');
const uglify = require("gulp-uglify");
const rename = require("gulp-rename");

const hash = require('gulp-hash');

// Paths
var paths = {
    stylesToLint: [
        "./wwwroot/assets/sass/stockportgov/**/*.scss",
        "./wwwroot/assets/sass/healthystockport/**/*.scss",
        "./wwwroot/assets/sass/StockportStyleGuide/**/*.scss"
    ],
    sassToWatch: "./wwwroot/assets/sass/**/*.scss",
    sassToCompile: "./wwwroot/assets/sass/*.scss",
    stylesheets: "./wwwroot/assets/stylesheets",
    js: [
        "./wwwroot/assets/javascript/**/*.js",
        "!./wwwroot/assets/javascript/**/*.min.js"
    ]
};

// JS
function js() {
    return src(paths.js)
        .pipe(uglify())
        .pipe(hash())
        .pipe(rename({ suffix: '.min' }))
        .pipe(dest(function (file) {
            return file.base;
        }));
}

// CSS
function css() {
    return src(paths.sassToCompile)
        .pipe(sass.sync().on('error', sass.logError))
        .pipe(postcss([autoprefixer(), cssnano()]))
        .pipe(rename({ suffix: '.min' }))
        .pipe(dest(paths.stylesheets));
};

exports.css = series(css);
exports.js = series(js);
exports.build = parallel(css, js);
exports.watch = function () {
    watch(paths.sassToWatch, { events: 'change' }, css);
    watch(paths.js, { events: 'change' }, js);
};
