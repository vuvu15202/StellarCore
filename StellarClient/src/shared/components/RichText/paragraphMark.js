const paragraphMarkPlugin = (editor) => {
    editor.ui.registry.addButton('showHideMarks', {
        text: '¶',
        tooltip: 'Show/Hide Paragraph',
        onAction: () => {
            const body = editor.getBody();
            if (body.classList.contains('show-paragraph-marks')) {
                body.classList.remove('show-paragraph-marks'); // Remove class
            } else {
                body.classList.add('show-paragraph-marks'); // Add class
            }
            editor.dom.setStyles(body, {}); // 🔥 Force TinyMCE to refresh styles
        },
    });
};
export default paragraphMarkPlugin;