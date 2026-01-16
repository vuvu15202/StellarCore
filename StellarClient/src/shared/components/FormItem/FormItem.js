import React from 'react';
import PropTypes from 'prop-types';
import { Form, Input,Radio ,Checkbox,Select} from 'antd';
import {IDatePicker,ITimePicker,IDateRanger} from '../Input';
const InputItem = (props) => {
    const { type, ...rest } = props;
    switch (type) {
    case 'textarea':
        return <Input.TextArea rows={4} maxLength={1024} showCount {...rest} />;
    case 'number':
        return <Input type='number' {...rest} />;
    case 'date':
        return <IDatePicker {...rest} />;
    case 'time':
        return <ITimePicker {...rest} />;
    case 'dateranger':
        return <IDateRanger {...rest} />;
    case 'radio':
        return <Radio.Group {...rest} />;
    case 'checkbox':
        return <Checkbox.Group {...rest} />;
    case 'select':
        return <Select showSearch optionFilterProp="label" {...rest} />;
    default:
        return <Input {...rest} />;
    }
};
InputItem.propTypes = {
    type: PropTypes.oneOf(
        [
            'text',
            'textarea',
            'number',
            'date',
            'time',
            'dateranger',
            'radio',
            'checkbox',
            'select'
        ]),
    autoFocus: PropTypes.bool,
};
export const FormItem = (props) => {
    const { field, label, span, required, autoFocus, type, ...rest } = props;
    return (
        <Form.Item
            key={field}
            className={`col-md-${span}`}
            label={label}
            name={field}
            {...rest}
            rules={[
                {
                    required: required,
                    message: `${label} không được để trống!`,
                },
            ]}
        >
            <InputItem type={type} autoFocus={autoFocus}  {...rest} />
        </Form.Item>
    );
};
FormItem.propTypes = {
    ...InputItem.propTypes, ...{
        field: PropTypes.string,
        label: PropTypes.string,
        span: PropTypes.number,
        required: PropTypes.bool,
    }
};

