'use strict';

const gulp = require('gulp');
const fs = require('fs');
const sass = require('gulp-sass')(require('sass'));

/**
 * Make sure all the destination directories exist.
 */
function copyLibFiles(done) {
    const nodeDir = './node_modules';
    const libDir = './wwwroot/lib';

    // Directories with library files to make available on the client.
    const srcDirs = [
        'bootstrap/dist',
        'jquery/dist',
        'jquery-validation/dist',
        'jquery-validation-unobtrusive/dist',
        'tinymce'
    ];

    for (let srcDir of srcDirs)
    {
        const libName = srcDir.split('/')[0];
        const srcGlob = `${nodeDir}/${srcDir}/**/*`;
        const destDir = `${libDir}/${libName}`;

        // Make sure the destination directory exist.
        if (!fs.existsSync(destDir)) {
            fs.mkdirSync(destDir, { recursive: true });
        }

        // Copy the files over.
        gulp.src(srcGlob).pipe(gulp.dest(destDir));
    }

    // Signal async completion.
    done();
}

/**
 * Compile SCSS code to CSS and copy to wwwroot/css.
 * @param done
 */
function compileCss(done) {
    // Ensure the output directory exists.
    const cssDir = './wwwroot/css';
    if (!fs.existsSync(cssDir)) {
        fs.mkdirSync(cssDir);
    }

    // Compile the SCSS files and return the stream.
    gulp.src('./Sass/site.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(cssDir));

    // Signal async completion.
    done();
}

/**
 * Watch for changes and reprocess JS or SCSS as needed.
 */
function sassWatch() {
    gulp.watch('./Sass/**/*.scss', compileCss);
}

/**
 * Do all setup tasks.
 */
function defaultTask(done) {
    // Execute tasks in parallel and signal when all are complete
    return gulp.parallel(copyLibFiles, compileCss)(done);
}

exports.copyLibFiles = copyLibFiles;
exports.compileCss = compileCss;
exports.watch = sassWatch;
exports.default = defaultTask;
