import React from 'react';
import { Layout, theme, ConfigProvider } from 'antd';
import Header from './Header';
import Sider from './Sider';

const { Content } = Layout;

const WebLayout = ({ children }) => {
    return (
        <ConfigProvider
            theme={{
                token: {
                    colorPrimary: '#1D50E7',
                    fontFamily: '"Inter", sans-serif',
                },
            }}
        >
            <Layout style={{ minHeight: '100vh' }}>
                <Header />
                <Layout>
                    <Sider />
                    <Content
                        style={{
                            padding: '24px',
                            margin: 0,
                            minHeight: 280,
                            background: '#f5f5f5',
                        }}
                    >
                        {children}
                    </Content>
                </Layout>
            </Layout>
        </ConfigProvider>
    );
};

export default WebLayout;
