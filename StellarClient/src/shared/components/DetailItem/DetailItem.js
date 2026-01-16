import React from 'react';
import { Descriptions} from 'antd';
import {formatDate, formatTime} from 'shared/utils';

export const DetailItem = (item) => {
    switch (item.type) {
    case 'textarea':
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{item.gia_tri}</Descriptions.Item>;
    case 'number':
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{item.gia_tri}</Descriptions.Item>;
    case 'date':
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{formatDate(item.gia_tri)}</Descriptions.Item>;
    case 'time':
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{formatTime(item.gia_tri)}</Descriptions.Item>;
    case 'dateranger':
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{item.gia_tri}</Descriptions.Item>;
    case 'radio':
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{item.gia_tri}</Descriptions.Item>;
    case 'checkbox':
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{item.gia_tri}</Descriptions.Item>;
    case 'select':
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{item.gia_tri}</Descriptions.Item>;
    default:
        return <Descriptions.Item key={item.ten_truong} label={item.ten} span={1}>{item.gia_tri}</Descriptions.Item>;
    }
};
