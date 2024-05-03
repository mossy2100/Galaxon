'use strict';

const gulp = require('gulp');
const fs = require('fs');
// const concat = require('gulp-concat');
const gulpSass = require('gulp-sass');
const nodeSass = require('node-sass');
const minify = require('gulp-minify');

function copyLibFiles(cb) {
    const nodeDir = './node_modules';
    const libDir = './wwwroot/lib';
    
    // Make sure all the destination directories exist.
    const subdirs = [
        '',
        '/bootstrap',
        '/jquery',
        '/jquery-validation',
        '/jquery-validation-unobtrusive',
        '/tinymce'
    ];
    subdirs.forEach(dir => {
        dir = `${libDir}${dir}`;
        if (!fs.existsSync(dir)) {
            fs.mkdirSync(dir);
        }
    });

    // Copy the distribution files.
    gulp.src(`${nodeDir}/bootstrap/dist/**/*`)
        .pipe(gulp.dest(`${libDir}/bootstrap`));
    gulp.src(`${nodeDir}/jquery/dist/**/*`)
        .pipe(gulp.dest(`${libDir}/jquery`));
    gulp.src(`${nodeDir}/jquery-validation/dist/**/*`)
        .pipe(gulp.dest(`${libDir}/jquery-validation`));
    gulp.src(`${nodeDir}/jquery-validation-unobtrusive/dist/**/*`)
        .pipe(gulp.dest(`${libDir}/jquery-validation-unobtrusive`));
    gulp.src(`${nodeDir}/tinymce/**/*`)
        .pipe(gulp.dest(`${libDir}/tinymce`));

    cb();
}

function compileCss(cb) {
    // Ensure the output directory exists. 
    const cssDir = './wwwroot/css';
    if (!fs.existsSync(cssDir)) {
        fs.mkdirSync(cssDir);
    }

    // Compile the SCSS files.
    const sass = gulpSass(nodeSass);
    gulp.src('./src/scss/site.scss')
        .pipe(sass.sync().on('error', sass.logError))
        .pipe(gulp.dest(cssDir));

    cb();
}

function minifyJs(cb) {
    // Ensure the output directory exists. 
    const jsDir = './wwwroot/js';
    if (!fs.existsSync(jsDir)) {
        fs.mkdirSync(jsDir);
    }

    // Minify the JS files.
    gulp.src(['src/js/*.js', 'src/js/*.mjs'])
        .pipe(minify({
            ext: {
                min: '.min.js'
            },
            noSource: true,
        }))
        .pipe(gulp.dest(jsDir))

    cb();
}

function scssWatch(cb) {
    gulp.watch('./src/scss/**/*.scss', compileCss);
    gulp.watch('./src/js/**/*.js', minifyJs());
    cb();
}

function defaultTask(cb) {
    // copyLibFiles(cb);
    compileCss(cb);
    minifyJs(cb);
    cb();
}

exports.copyLibFiles = copyLibFiles;
exports.compileCss = compileCss;
exports.minifyJs = minifyJs;
exports.scssWatch = scssWatch;
exports.default = defaultTask; 
