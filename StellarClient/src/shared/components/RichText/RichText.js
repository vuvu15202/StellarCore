// TinyMCE so the global var exists
import 'tinymce/tinymce';
// DOM model
import 'tinymce/models/dom/model';
// Theme
import 'tinymce/themes/silver';
// Toolbar icons
import 'tinymce/icons/default';
// Editor styles
import 'tinymce/skins/ui/oxide/skin';
// importing the plugin js.
// if you use a plugin that is not listed here the editor will fail to load
import 'tinymce/plugins/autoresize';
import 'tinymce/plugins/advlist';
import 'tinymce/plugins/anchor';
import 'tinymce/plugins/autolink';
import 'tinymce/plugins/charmap';
import 'tinymce/plugins/fullscreen';
import 'tinymce/plugins/help';
import 'tinymce/plugins/help/js/i18n/keynav/en';
import 'tinymce/plugins/image';
import 'tinymce/plugins/link';
import 'tinymce/plugins/lists';
import 'tinymce/plugins/media';
import 'tinymce/plugins/searchreplace';
import 'tinymce/plugins/table';
import 'tinymce/plugins/visualblocks';
import 'tinymce/plugins/visualchars';
import 'tinymce/plugins/preview';
import 'tinymce/plugins/table';
import '@openregion/tinymce-word-paste-plugin';
import spacingPlugin from './spacing';
import paragraphMarkPlugin from './paragraphMark';
import React, { useRef } from 'react';
import { Editor } from '@tinymce/tinymce-react';
import dompurify from 'dompurify';
import { http } from 'shared/utils';
import { FILE_URL } from '../../app-setting';

export const RichText = (props) => {
    const {
        value
        , onChange
        , disabled,
        duong_dan = 'richtext-images',
        cusStyle = 'font-family: Times New Roman; font-size: 14pt'
        , ...restProps
    } = props;

    const editorRef = useRef();
    const handleImageUpload = (blobInfo, progress) => new Promise((resolve, reject) => {
        let formdata = new FormData();
        formdata.append('FormFile', blobInfo.blob());
        http.post(`api/file/tep-tin?duong_dan=${duong_dan}`,
            formdata,
            {
                reportProgress: true,
                observe: 'events',
                onUploadProgress: function (ev) {
                    var percent = Math.round((ev.loaded * 100) / ev.total);
                    progress({ percent });
                }
            }).subscribe(res => {
                if (res && res[0])
                    resolve(`${FILE_URL}${res[0]?.id}`);
            }, err => {
                reject(err);
            });
    });
    if (disabled)
        return (
            <div className='rich-text-view'>
                <div className='rich-text-content' dangerouslySetInnerHTML={{ __html: dompurify.sanitize(value) }}></div>
            </div>
        );
    else
        return (
            <Editor
                license_key='gpl'
                onInit={(_evt, editor) => editorRef.current = editor}
                value={dompurify.sanitize(value)}
                onEditorChange={(val) => { onChange(dompurify.sanitize(val)); }}
                disabled={disabled}
                toolbar={'undo redo | formatselect |Spacing  table image | bold italic fontfamily fontsize align lineheight  bullist numlist outdent indent| blocks| forecolor backcolor |visualchars showHideMarks| fullscreen'}
                {...restProps}
                init={{
                    license_key: 'gpl',
                    menubar: false,
                    statusbar: false,
                    plugins: [
                        'autoresize',
                        'advlist',
                        'autolink',
                        'lists',
                        'link',
                        'image',
                        'charmap',
                        'anchor',
                        'searchreplace',
                        'visualblocks',
                        'visualchars',
                        'fullscreen',
                        'media',
                        'table',
                        'preview',
                        'pasteword',
                        'help'
                    ],
                    content_css: 'default',
                    skin: 'oxide',
                    forced_root_block_attrs: {
                        'style': `${cusStyle}`
                    },
                    content_style: `
                        body { font-family:Times New Roman; font-size:14pt ;}
                        /* ✅ Make non-breaking spaces visible */
                        .mce-nbsp {
                        display: inline-block;
                        width: 0.33em;
                        background-color: rgba(255, 0, 0, 0.2); /* Light red background */
                        border: 1px dashed rgba(255, 0, 0, 0.5);
                        }
                        .mce-visualblocks {
                            outline: 1px dashed red !important;
                            background-color: rgba(255, 0, 0, 0.1) !important;
                        }
                        .show-paragraph-marks p::after,
                        .show-paragraph-marks h1::after,
                        .show-paragraph-marks h2::after,
                        .show-paragraph-marks h3::after,
                        .show-paragraph-marks h4::after,
                        .show-paragraph-marks h5::after,
                        .show-paragraph-marks h6::after,
                        .show-paragraph-marks div::after,
                        .show-paragraph-marks blockquote::after,
                        .show-paragraph-marks li::after,
                        .show-paragraph-marks td::after,
                        .show-paragraph-marks th::after,
                        .show-paragraph-marks pre::after {
                            content: "¶";
                            color: gray;
                            font-size: 14px;
                            margin-left: 5px;
                        }
                        .show-paragraph-marks p:has(br:only-child)::after {
                            content: "¶";
                            color: gray;
                            font-size: 14px;
                            margin-left: 5px;
                        }
                    `,
                    automatic_uploads: true,
                    images_reuse_filename: true,
                    relative_urls: false,
                    remove_script_host: false,
                    images_upload_handler: handleImageUpload,
                    paste_data_images: false,
                    // paste_as_text: true,
                    setup: (editor) => {
                        spacingPlugin(editor); // Setup the plugin inside the editor
                        paragraphMarkPlugin(editor);
                    },
                    extended_valid_elements: 'p[style],div[style],h1[style],h2[style],h3[style],h4[style],h5[style],h6[style]', // Giữ lại style cho các phần tử quan trọng
                    paste_enable_default_filters: false
                }}
            />
        );
};



export default RichText;
