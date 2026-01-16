import React from 'react';
import PropTypes from 'prop-types';
import { Image } from 'antd';
import { FILE_URL } from '../../app-setting';
import avatar from 'assets/images/avatar.png';
export const IImage = (props) => {
    const { fileId, fallback = avatar, ...rest } = props;
    return (
        <Image  {...rest} src={`${FILE_URL}${fileId}`} fallback={fallback} />
    );
};
IImage.propTypes = {
    fileId: PropTypes.string,
    fallback: PropTypes.string,
};


