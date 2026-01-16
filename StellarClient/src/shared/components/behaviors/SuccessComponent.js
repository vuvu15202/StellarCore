import React, { useContext, useState, useEffect } from 'react';
import {
    Modal, theme, Space
} from 'antd';
import { Icon } from '@iconify/react';
import { BehaviorsContext } from 'shared/services';

const SuccessComponent = () => {
    const context = useContext(BehaviorsContext);
    const [show, setShow] = useState(false);
    const [msg, setMsg] = useState('');
    const { token } = theme.useToken();

    useEffect(() => {
        const subscriptions = context.onShowSuccess.subscribe((res) => {
            if (!show) {
                setMsg(res.msg || '');
                setShow(true);
            }
        });
        return () => {
            subscriptions?.unsubscribe();
        };
    }, []);

    const handleClose = () => {
        setShow(false);
        context.onHideSuccess.next(true);
    };

    return (
        <Modal
            zIndex={1040}
            title={<Space>
                <span className='anticon'>
                    <Icon width={18} height={18} style={{ color: token.colorSuccess }} icon="ant-design:check-circle-filled" />
                </span>
                Thành công
            </Space>}
            open={show}
            onOk={handleClose}
            footer={(_, { OkBtn }) => (
                <>
                    <OkBtn />
                </>
            )}

            onCancel={handleClose}
        >

            <p>{msg}</p>
        </Modal>

    );
};

export { SuccessComponent };
