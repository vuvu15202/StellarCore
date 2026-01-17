import React from 'react';
import { Layout, Typography, Avatar, Space, theme } from 'antd';
import { UserOutlined, BellOutlined } from '@ant-design/icons';

const { Header: AntHeader } = Layout;
const { Text } = Typography;

const Header = () => {
    const {
        token: { colorBgContainer, colorBorderSecondary },
    } = theme.useToken();

    return (
        <AntHeader
            style={{
                padding: '0 24px',
                background: colorBgContainer,
                borderBottom: `1px solid ${colorBorderSecondary}`,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                height: 64,
            }}
        >
            <div className="logo" style={{ fontWeight: 'bold', fontSize: '18px' }}>
                Stellar Education
            </div>
            <Space size="large">
                <BellOutlined style={{ fontSize: '18px', cursor: 'pointer' }} />
                <Space>
                    <Avatar icon={<UserOutlined />} />
                    <Text strong>Admin User</Text>
                </Space>
            </Space>
        </AntHeader>
    );
};

export default Header;
