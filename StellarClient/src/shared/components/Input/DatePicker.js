import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { DatePicker } from 'antd';
import dayjs from 'dayjs';
export const IDatePicker = (props) => {
    // dayjs.tz?.setDefault('Asia/Ho_Chi_Minh');
    const { value, format = 'DD/MM/YYYY', onChange, ...restProps } = props;
    const [innerValue, setInnerValue] = useState(null);
    const [innerStrValue, setInnerStrValue] = useState(null);
    useEffect(() => {
        if (value != innerStrValue) {
            const newValue = value ? (dayjs.isDayjs(value) ? value : dayjs(value)) : value;
            setInnerValue(newValue);
            setInnerStrValue(value);
        }

    }, [value, innerStrValue]);
    const handleChange = (v) => {
        let newValue = null;
        if (v) {
            newValue = v.format('YYYY-MM-DDTHH:mm:ss');
        }
        setInnerValue(v);
        setInnerStrValue(newValue);
        if (onChange) {
            onChange(newValue);
        }


    };
    return (
        <DatePicker format={format} value={innerValue} onChange={handleChange} {...restProps} />
    );
};

IDatePicker.propTypes = {
    value: PropTypes.any,
    format: PropTypes.string,
    onChange: PropTypes.func
};