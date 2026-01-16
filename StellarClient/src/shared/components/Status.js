import React from 'react';
import PropTypes from 'prop-types';
import {Icon} from '@iconify/react';

export const Status = (props) => {
    const {result, ...rest} = props;
    return (
        result !== null && result !== undefined
            ? result
                ? <Icon {...rest} icon="ep:success-filled" width={20} height={20} color="#00d60d"/>
                : <Icon {...rest} icon="ix:namur-failure-filled" width={20} height={20} color="#dc0000"/>
            : null
    );
};
Status.propTypes = {
    perm: PropTypes.oneOfType([PropTypes.string, PropTypes.array]),
    title: PropTypes.string
};

