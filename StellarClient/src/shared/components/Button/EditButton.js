import React from 'react';
import PropTypes from 'prop-types';
import {Button, theme} from 'antd';
import {Icon} from '../Icon';
import {CheckPerm} from './_CheckPerm';

export const EditButton = (props) => {
    const {perm, title, ...rest} = props;
    const {token} = theme.useToken();
    if (perm == null || CheckPerm(perm)) {
        return (
            // <ConfigProvider
            //     theme={{
            //         components: {
            //             Button: {
            //                 defaultBorderColor:token.colorWarningBorder,
            //                 defaultHoverBorderColor:token.colorWarningBorderHover,
            //                 defaultHoverColor:token.colorTextBase,
            //                 defaultBg :token.colorWarning,
            //                 defaultHoverBg:token.colorWarningHover,
            //             },
            //         },
            //     }}
            // >
            //     <Button  {...rest} type='default' icon={<Icon  icon="ant-design:edit-filled"  />}  >
            //         {title ? title: 'Chỉnh sửa'}
            //     </Button>
            // </ConfigProvider>
            <Button
                {...rest}
                type='primary'
                icon={<Icon icon="ant-design:edit-filled"/>}
                style={{
                    backgroundColor: '#faad14',
                    color: '#000',
                    borderColor: '#ffe58f'
                }}
            >
                {title ? title : 'Chỉnh sửa'}
            </Button>
        );
    } else return null;
};
EditButton.propTypes = {
    perm: PropTypes.oneOfType([PropTypes.string, PropTypes.array]),
    title: PropTypes.string
};

