import React from 'react';
import PropTypes from 'prop-types';
import { Button} from 'antd';
import { Icon } from '../Icon';
import { CheckPerm } from './_CheckPerm';
export const ResetButton = (props) => { 
    const {perm,title,...rest}=  props;
    if (perm == null || CheckPerm(perm))
    {
        return (       
            <Button {...rest} danger icon={<Icon icon="mdi:content-save-off-outline" />} >  
                {title ? title: 'Hủy lưu'}
            </Button>
        );
    }else return null;          
};
ResetButton.propTypes={
    perm: PropTypes.oneOfType([PropTypes.string,PropTypes.array]) ,
    title:PropTypes.string 
};

