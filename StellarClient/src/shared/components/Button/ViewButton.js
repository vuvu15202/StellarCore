import React from 'react';
import {Button, Tooltip} from 'antd';
import {Icon} from '@iconify/react';

export const ViewButton = (props) => {
    return (
        <Tooltip
            title="Xem"
            placement="right"
        >
            <Button
                {...props}
                type={'text'}
                icon={<Icon icon="lets-icons:view" width="1.7em" height="1.7em" color="#008bdc"/>}>
            </Button>
        </Tooltip>
    );
};


