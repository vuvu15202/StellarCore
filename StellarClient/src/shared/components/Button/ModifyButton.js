import React from 'react';
import {Button, Tooltip} from 'antd';
import {Icon} from '@iconify/react';

export const ModifyButton = (props) => {
    return (
        <Tooltip
            title="Sửa"
            placement="right"
        >
            <Button
                {...props}
                type={'text'}
                icon={<Icon icon="system-uicons:create" width="1.7em" height="1.7em" color="#faad14"/>}>
            </Button>
        </Tooltip>
    );
};


