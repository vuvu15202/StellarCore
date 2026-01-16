import React from 'react';
import PropTypes from 'prop-types';
import { Button} from 'antd';
import { Icon } from '../Icon';
import { CheckPerm } from './_CheckPerm';

export const SearchButton = (props) => {  
    const {perm,title,...rest}=  props;
    if (perm == null || CheckPerm(perm))
    {
        return (
            <Button {...rest}  type='primary' icon={<Icon  icon="ant-design:search-outlined" />} >
                {title ? title: 'Tìm kiếm'}
            </Button>
        );
    }else return null;  
};
SearchButton.propTypes={
    perm: PropTypes.oneOfType([PropTypes.string,PropTypes.array]),
    title:PropTypes.string 
};

