const spacingPlugin = (editor) => {
    editor.ui.registry.addButton('spacing', {
        text: 'Format',
        onAction: function () {
            const selectedNode = editor.selection.getNode(); // Lấy phần tử cha chứa vùng chọn
            if (!selectedNode) {
                editor.notificationManager.open({
                    text: 'Không thể thiết lập cho phần tử này!',
                    type: 'warning',
                    timeout: 2000,
                });
                return;
            }
            // Lấy giá trị margin hiện tại
            const currentStyle = selectedNode.getAttribute('style') || '';
            const styleMatch = currentStyle.match(/margin:\s*([\dpx\s]+)/);
            const currentMargins = styleMatch ? styleMatch[1].split(' ') : '';
            const styleMatchHeight = currentStyle.match(/line-height:\s*([\d.]+)/);
            const currentLineHeight = styleMatchHeight ? styleMatchHeight[1] : '';
            
            const computedStyle = window.getComputedStyle(selectedNode);
            console.log(12, computedStyle);
            let currentTextIndent = computedStyle.textIndent;
            let currentMarginLeft = computedStyle.marginLeft;
            let indentType = '';
            // Detect current indent type
            if(parseInt(currentTextIndent) > 0){
                indentType ='first-line';
            }else if (parseInt(currentTextIndent) < 0 && parseInt(currentMarginLeft) > 0) {
                indentType = 'hanging';
            }
            const dialog = editor.windowManager.open({
                title: 'Format',
                body: {
                    type: 'panel',               
                    items: [
                        {
                            type: 'grid',
                            columns: 2,
                            items: [
                                {
                                    type: 'selectbox',
                                    name: 'indentType',
                                    label: 'Specical',
                                    items: [
                                        { text: 'None', value: '' },
                                        { text: 'First line', value: 'first-line' },
                                        { text: 'Hanging', value: 'hanging' },
                                    ],
                                    value: '',
                                },
                                {
                                    type: 'input',
                                    name: 'indentSize',
                                    label: 'By',
                                    inputMode: 'decimal',
                                    value: '0in',
                                },
                            ],                   
                        },
                        {
                            type: 'grid',
                            columns: 2,
                            items: [
                                { type: 'input', name: 'marginTop', label: 'Before', inputMode: 'string'},
                                { type: 'input', name: 'marginBottom', label: 'After', inputMode: 'string'},
                                { type: 'input', name: 'marginLeft', label: 'Left', inputMode: 'string'},
                                { type: 'input', name: 'marginRight', label: 'Right', inputMode: 'string'},
                            ]                       
                        },
                        {
                            type: 'input', name: 'lineHeight', label: 'Line spacing', inputMode: 'string'
                        },

                    ],
                },
                buttons: [
                    { type: 'cancel', text: 'Cancel' },
                    { type: 'submit', text: 'Apply', primary: true },
                ],
                onSubmit: (dialog) => {
                    const data = dialog.getData();
                    const newMargin = `${data.marginTop||0} ${data.marginRight||0} ${data.marginBottom||0} ${data.marginLeft||0}`;
                    const newLineHeight = data.lineHeight;
                    const indentType = data.indentType;
                    const indentSize = data.indentSize;

                    // Cập nhật style mới
                    const paragraphs = selectedNode.querySelectorAll('p');
                    paragraphs.forEach(p => {
                        p.style.margin = newMargin;
                        if (newLineHeight) {
                            p.style.lineHeight = newLineHeight; // ✅ Chỉ cập nhật line-height, không ghi đè các style khác
                        }
                        if (indentType === 'first-line') {
                            p.style.textIndent = `${indentSize}`; // Thụt dòng đầu tiên
                            p.style.marginLeft = '0 in'; // Đảm bảo không ảnh hưởng toàn bộ đoạn
                        } 
                        if (indentType === 'hanging') {
                            p.style.textIndent = `-${indentSize}`; // Thụt lùi dòng đầu tiên
                            p.style.marginLeft = `${indentSize}`; // Dịch chuyển toàn bộ đoạn văn
                        }
                    });

                    dialog.close();
                },
            });
            setTimeout(() => {
                dialog.setData({
                    marginTop: currentMargins[0] || '', 
                    marginRight: currentMargins[1]|| '', 
                    marginBottom: currentMargins[2]|| '', 
                    marginLeft: currentMargins[3]|| '', 
                    lineHeight: currentLineHeight,
                    indentType: indentType,
                    indentSize:  Math.abs(parseFloat(currentTextIndent)).toString(),
                });
            }, 50);
        },
    });
};
  
export default spacingPlugin;
  