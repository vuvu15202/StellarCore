import React from 'react';
import {Button, Tooltip} from 'antd';
import {Icon} from '@iconify/react';

export const RemoveButton = (props) => {
    return (
        <Tooltip
            title="Xóa"
            placement="right"
        >
            <Button
                {...props}
                type={'text'}
                icon={<Icon icon="ant-design:delete-filled" width="1.7em" height="1.7em" color="#F5222D"/>}>
            </Button>
        </Tooltip>
    );
};


