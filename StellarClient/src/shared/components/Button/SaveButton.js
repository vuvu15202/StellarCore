import React from 'react';
import PropTypes from 'prop-types';
import { Button} from 'antd';
import { Icon } from '../Icon';
import { CheckPerm } from './_CheckPerm';

export const SaveButton = (props) => {  
    const {perm,title,...rest}=  props;
    if (perm == null || CheckPerm(perm))
    {
        return (       
            <Button {...rest} type='primary' icon={<Icon  icon="ant-design:save-filled"  />} >
                {title ? title: 'Lưu lại'}
            </Button>
        );
    }else return null;  
};
SaveButton.propTypes={
    perm: PropTypes.oneOfType([PropTypes.string,PropTypes.array]),
    title:PropTypes.string 
};

