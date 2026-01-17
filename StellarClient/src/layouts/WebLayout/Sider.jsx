import React, { useState } from 'react';
import { Layout, Menu, theme } from 'antd';
import {
    DashboardOutlined,
    UserOutlined,
    BookOutlined,
    QuestionCircleOutlined,
} from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

const { Sider: AntSider } = Layout;

const Sider = () => {
    const [collapsed, setCollapsed] = useState(false);
    const {
        token: { colorBgContainer, colorBorderSecondary },
    } = theme.useToken();
    const navigate = useNavigate();

    const items = [
        {
            key: 'dashboard',
            icon: <DashboardOutlined />,
            label: 'Dashboard',
            onClick: () => navigate('/'),
        },
        {
            key: 'courses',
            icon: <BookOutlined />,
            label: 'Courses',
            children: [
                { key: 'course-list', label: 'All Courses' },
                { key: 'add-course', label: 'Add Course' },
            ],
        },
        {
            key: 'users',
            icon: <UserOutlined />,
            label: 'Users',
        },
        {
            key: 'exams',
            icon: <QuestionCircleOutlined />,
            label: 'Exams',
        },
    ];

    return (
        <AntSider
            collapsible
            collapsed={collapsed}
            onCollapse={(value) => setCollapsed(value)}
            width={260}
            style={{
                background: colorBgContainer,
                borderRight: `1px solid ${colorBorderSecondary}`,
            }}
            theme="light"
        >
            <Menu
                mode="inline"
                defaultSelectedKeys={['dashboard']}
                style={{ borderRight: 0 }}
                items={items}
            />
        </AntSider>
    );
};

export default Sider;
