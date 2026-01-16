import React from 'react';
import PropTypes from 'prop-types';
import { Button } from 'antd';
import { Icon } from '../Icon';

export const StatusToggleButton = (props) => {
    const {
        status = null, 
        onToggle,
        loading = false,
        ...rest
    } = props;

    return (
        <Button
            {...rest}
            onClick={onToggle}
            type={status ? 'default' : 'primary'}
            style={{
                backgroundColor: status ? '#ff4d4f' : '#52c41a', 
                color: '#fff',
            }}
            icon={
                <Icon icon={status ? 'ant-design:eye-invisible-outlined' : 'ant-design:eye-outlined'} />
            }
            loading={loading}
        >
            {status ? 'Tắt' : 'Bật'}
        </Button>
    );
};

StatusToggleButton.propTypes = {
    status: PropTypes.bool, // trạng thái hiện tại: true = bật, false = tắt
    onToggle: PropTypes.func, 
    loading: PropTypes.bool, 
};