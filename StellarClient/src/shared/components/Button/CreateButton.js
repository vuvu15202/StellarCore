import React from 'react';
import PropTypes from 'prop-types';
import { Button} from 'antd';
import { Icon } from '../Icon';
import { CheckPerm } from './_CheckPerm';
export const CreateButton = (props) => {  
    const {perm,title,...rest}=  props;
    if (perm == null || CheckPerm(perm)) 
    {
        return (       
            <Button {...rest} type='primary' icon={<Icon icon="ant-design:plus-outlined" />} > 
                {title ? title: 'Thêm mới'}
            </Button>
        );
    }else return null;  
};
CreateButton.propTypes={
    perm: PropTypes.oneOfType([PropTypes.string,PropTypes.array]) ,
    title: PropTypes.string
};

