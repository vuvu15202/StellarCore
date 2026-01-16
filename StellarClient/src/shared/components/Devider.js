import React from 'react';
import PropTypes from 'prop-types';
import {Divider} from 'antd';

export const Devider = (props) => {
    const {title, ...rest} = props;
    return (
        <Divider
            style={{
                marginTop: 24,
                marginBottom: 24,
                borderColor: '#999',
                borderStyle: 'dashed',
                borderWidth: '2px 0 0 0',
            }}
        />
    );
};
Devider.propTypes = {
    perm: PropTypes.oneOfType([PropTypes.string, PropTypes.array]),
    title: PropTypes.string
};

