tinymce.init({
    selector: 'textarea',
    plugins: 'preview searchreplace autolink code visualblocks visualchars fullscreen image link template codesample table charmap pagebreak nonbreaking anchor insertdatetime advlist lists wordcount help charmap quickbars emoticons',
    menubar: false,
    toolbar: 'undo redo | bold italic underline strikethrough subscript superscript | blocks blockquote | forecolor backcolor removeformat | image link | alignleft aligncenter alignright outdent indent |  numlist bullist table | anchor pagebreak hr charmap emoticons | fullscreen preview print code',
    toolbar_mode: 'sliding',
    toolbar_sticky: true,
    toolbar_sticky_offset: 0,
    image_advtab: true,
    file_picker_callback: (callback, value, meta) => {
        /* Provide file and text for the link dialog */
        if (meta.filetype === 'file') {
            callback('https://www.google.com/logos/google.jpg', { text: 'My text' });
        }
        /* Provide image and alt text for the image dialog */
        if (meta.filetype === 'image') {
            callback('https://www.google.com/logos/google.jpg', { alt: 'My alt text' });
        }
    },
    template_cdate_format: '[Date Created (CDATE): %d/%m/%Y %H:%M:%S]',
    template_mdate_format: '[Date Modified (MDATE): %d/%m/%Y %H:%M:%S]',
    height: 600,
    image_caption: true,
    quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
    noneditable_class: 'mceNonEditable',
    contextmenu: 'link image table',
    skin: 'oxide',
    content_css: 'default',
});

function onIsFolderClick() {
    if ($('#IsFolder').is(':checked')) {
        $('#content-field').hide();
    }
    else {
        $('#content-field').show();
    }
}

function initIsFolderCheckbox() {
    // Update hide/show state of content field when IsFolder is clicked..
    $('#IsFolder').click(onIsFolderClick);

    // Set initial hide/show state of content field.
    onIsFolderClick();
}

function initIconButtons() {
    $('#icon-control').hide();

    $('#iconAction-keep').click(event => {
        $('#current-icon').css('opacity', 1);
        $('#icon-control').hide();
    });

    $('#iconAction-delete').click(event => {
        $('#current-icon').css('opacity', 0.25);
        $('#icon-control').hide();
    });

    $('#iconAction-update').click(event => {
        $('#current-icon').css('opacity', 0.25);
        $('#icon-control').show();
    });
}

$(() => {
    initIsFolderCheckbox();
    initIconButtons();
});
