import React from 'react';
import { Button,Tooltip } from 'antd';
import { Icon } from '../Icon';
export const FullscreenButton = ({query,...props}) => {  
    const toogleFullscreen=()=>{
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
    return (  
        <Tooltip title="Toàn màn hình">
            <Button {...props} icon={<Icon icon="ant-design:fullscreen-outlined"/>} onClick={toogleFullscreen} />
        </Tooltip>             
    );
};


