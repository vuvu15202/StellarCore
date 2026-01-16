import React from 'react';
import PropTypes from 'prop-types';
import { Icon as Iconify} from '@iconify/react';
import classNames from 'classnames'; 


export const Icon = (props)=>{
    const {width='1em',height='1em',className}=props;
    return(<span className={classNames('anticon',className)}><Iconify width={width} height={height} {...props}/></span>);
};

Icon.propTypes = {
    width:PropTypes.oneOfType([PropTypes.string,PropTypes.number]),
    height:PropTypes.oneOfType([PropTypes.string,PropTypes.number]),
    className:PropTypes.string
};