import React from 'react';
import PropTypes from 'prop-types';
import { Button} from 'antd';
import { Icon } from '../Icon';
import { CheckPerm } from './_CheckPerm';

export const SendButton = (props) => {  
    const {perm,title,...rest}=  props;
    if (perm == null || CheckPerm(perm))
    {
        return (
            <Button {...rest}  icon={<Icon  icon="mdi:send-variant-outline" />} >
                {title ? title: 'Gửi'}
            </Button>
        );
    }else return null;  
};
SendButton.propTypes={
    perm: PropTypes.oneOfType([PropTypes.string,PropTypes.array]),
    title:PropTypes.string 
};



