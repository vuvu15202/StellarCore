import React from 'react';
import { Button } from 'antd';
import { Icon } from '../Icon';
export const CloseButton = (props) => {  
    return (       
        <Button {...props}  icon={<Icon icon="ant-design:close-circle-outlined" />} >      
                Đóng
        </Button>
    );
};


