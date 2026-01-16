// convert định dạng tệp về chuẩn only office
const convertDocTypeOnlyOffice = (dinh_dang) => {
    let word = ['DOCM', 'DOCX', 'DOTM', 'DOTX'];
    let cell = ['XLSM', 'XLSX', 'XLTM', 'XLTX'];
    let slide = ['POTM', 'POTX', 'PPSM', 'PPSX', 'PPTM', 'PPTX'];
    let pdf = ['PDF'];
    let result = '';
    if(dinh_dang){
        if(word.indexOf(dinh_dang.toUpperCase()) !== -1){
            result = 'word';
        }else if(cell.indexOf(dinh_dang.toUpperCase()) !== -1){ 
            result = 'cell';
        }else if(slide.indexOf(dinh_dang.toUpperCase()) !== -1){
            result = 'slide';
        }else if(pdf.indexOf(dinh_dang.toUpperCase()) !== -1){
            result = 'pdf';
        }
    }

    return result;
};
export { convertDocTypeOnlyOffice };