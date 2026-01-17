import React from 'react';
import { Card, Typography, Row, Col } from 'antd';

const { Title, Paragraph } = Typography;

const WebLayoutTest = () => {
    return (
        <div>
            <Title level={2}>Web Layout Test</Title>
            <Paragraph>
                This page uses the ported layout from the <code>web</code> project.
                It has a simplified Header and Sider without OIDC dependencies.
            </Paragraph>

            <Row gutter={[16, 16]}>
                <Col span={8}>
                    <Card title="Card 1" bordered={false}>
                        Card content
                    </Card>
                </Col>
                <Col span={8}>
                    <Card title="Card 2" bordered={false}>
                        Card content
                    </Card>
                </Col>
                <Col span={8}>
                    <Card title="Card 3" bordered={false}>
                        Card content
                    </Card>
                </Col>
            </Row>
        </div>
    );
};

export default WebLayoutTest;
