import React from 'react';
import {Tag} from 'antd';
import {HTTP_STATUS_CONFIG} from './httpStatus.config';
import PropTypes from 'prop-types';

export const HttpStatusTag = (props) => {
    const {status, ...rest} = props;

    const config = HTTP_STATUS_CONFIG[status];

    if (!config) {
        return <Tag>Unknown Status</Tag>;
    }

    return (
        <Tag
            style={{
                backgroundColor: config.color,
                color: '#fff',
                border: 'none',
                borderRadius: 999,
                fontWeight: 500,
            }}
        >
            {config.code} {config.label}
        </Tag>
    );
};
HttpStatusTag.propTypes = {
    perm: PropTypes.oneOfType([PropTypes.string, PropTypes.array]),
    title: PropTypes.string
};