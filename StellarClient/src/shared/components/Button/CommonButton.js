import React from 'react';
import PropTypes from 'prop-types';
import { Button, Tooltip,
    //  theme, 
    ConfigProvider } from 'antd';
import { Icon } from '../Icon';
import { CheckPerm } from './_CheckPerm';

export const CommonButton = (props) => {
    const { perm, icon, title
        // , variant
        , type, isTooltip, ...rest } = props;
    
    if (perm == null || CheckPerm(perm)) {
        return (<ConfigProvider
        >    {
                isTooltip ?
                    <Tooltip title={title}>
                        <Button {...rest} type={type} icon={icon && <Icon icon={icon} />}>
                        </Button>
                    </Tooltip> :
                    <Button {...rest} type={type} icon={icon && <Icon icon={icon} />}>
                        {title}
                    </Button>

            }

        </ConfigProvider>
        );
    } else return null;
};
CommonButton.propTypes = {
    perm: PropTypes.oneOfType([PropTypes.string, PropTypes.array]),
    icon: PropTypes.string,
    title:PropTypes.oneOfType([PropTypes.string, PropTypes.object]),
    isTooltip: PropTypes.bool,
    type: PropTypes.any
};

