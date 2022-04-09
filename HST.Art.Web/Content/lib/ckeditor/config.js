/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'zh-cn';
    config.font_names = '宋体;楷体;黑体;Arial;Times New Roman;Verdana';
    config.fontSize_sizes = '12/12px;14/14px;16/16px;18/18px;20/20px;22/22px;24/24px;26/26px;28/28px;36/36px;48/48px;72/72px'
    config.fontSize_defaultLabel = '14px';
    //config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;
    config.pasteFromWordRemoveFontStyles = false;
    config.pasteFromWordRemoveStyles = false;
    config.removeDialogTabs = 'image:advanced;image:Link';
    config.AllowedExtensions='{zip}';
    //config.skin='office2003';
    //config.uiColor = '#fff';
    // config.toolbar = 'Basic';

    config.toolbar_Full =
    [
        ['Source', '-', 'Save', 'NewPage', 'Preview', '-', 'Templates'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print', 'SpellChecker', 'Scayt'],
        ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
        ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],
        '/',
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Link', 'Unlink', 'Anchor'],
        ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
        '/',
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Maximize', 'ShowBlocks', '-', 'About']
    ];

    config.toolbar_Basic =
    [
        ['Bold', 'Italic', 'Underline', '-', 'NumberedList', 'BulletedList', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'Font', 'FontSize', '-', 'TextColor', 'BGColor', '-', 'Image', 'Link', 'Unlink', '-', 'Maximize','Source']
    ];
};
