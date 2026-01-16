import React from 'react';
import PropTypes from 'prop-types';
import { Image } from 'antd';
import { FILE_URL } from '../../app-setting';
import avatar from 'assets/images/avatar.png';
export const IAvatar = (props) => {
    const { fileId, width = 200, height = 267, fallback = avatar, ...rest } = props;
    return (
        <Image style={{ objectFit: 'cover' }} {...rest} width={width} height={height} src={`${FILE_URL}${fileId}`} fallback={fallback} />
    );
};
IAvatar.propTypes = {
    fileId: PropTypes.string,
    fallback: PropTypes.string,
    width: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    height: PropTypes.oneOfType([PropTypes.string, PropTypes.number])
};


