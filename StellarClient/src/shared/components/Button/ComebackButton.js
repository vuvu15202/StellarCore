import React from 'react';
import {Button} from 'antd';
import {LeftOutlined} from '@ant-design/icons';

export const ComebackButton = (props) => {
    return (
        <Button {...props} type="text" icon={<LeftOutlined/>}></Button>
    );
};


