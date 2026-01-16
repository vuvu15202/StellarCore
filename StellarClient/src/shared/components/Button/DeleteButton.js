import React from 'react';
import PropTypes from 'prop-types';
import { Button} from 'antd';
import { Icon } from '../Icon';
import { CheckPerm } from './_CheckPerm';
export const DeleteButton = (props) => {  
    const {perm,...rest}=  props;
    if (perm == null || CheckPerm(perm))
    {
        return ( 
            <Button {...rest} danger icon={<Icon icon="ant-design:delete-filled" />} >                
                Xóa
            </Button>
        );
    }else return null;  
};
DeleteButton.propTypes={
    perm: PropTypes.oneOfType([PropTypes.string,PropTypes.array]) ,
};

