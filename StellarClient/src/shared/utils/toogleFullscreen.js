export const toogleFullscreen=(query)=>{
    const el= document.querySelector(query);
    if(document.fullscreenElement){
        document.exitFullscreen();
        
    }else{
        if (el.requestFullscreen) {
            el.requestFullscreen();
        } else if (el.webkitRequestFullscreen) { /* Safari */
            el.webkitRequestFullscreen();
        } else if (document.msRequestFullscreen) { /* IE11 */
            el.msRequestFullscreen();
        }
    }        
};