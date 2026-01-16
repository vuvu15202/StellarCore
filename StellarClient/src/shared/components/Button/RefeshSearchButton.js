import React from 'react';
import PropTypes from 'prop-types';
import { Button} from 'antd';
import { Icon } from '../Icon';
import { CheckPerm } from './_CheckPerm';

export const RefeshSearchButton = (props) => {  
    const {perm,title,...rest}=  props;
    if (perm == null || CheckPerm(perm))
    {
        return (
            <Button {...rest} type='default' icon={<Icon  icon="ant-design:reload-outlined"  />} >
                {title ? title: 'Làm mới'}
            </Button>
        );
    }else return null;  
};
RefeshSearchButton.propTypes={
    perm: PropTypes.oneOfType([PropTypes.string,PropTypes.array]),
    title:PropTypes.string 
};

