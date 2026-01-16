import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { TimePicker } from 'antd';
import dayjs from 'dayjs';
export const ITimePicker = (props) => { 
    const {value,format='HH:mm',onChange,...restProps}=props;
    const [innerValue, setInnerValue] = useState(null);
    const [innerStrValue, setInnerStrValue] = useState(null);
    useEffect(() => {
        if (value != innerStrValue) {
            const newValue = value ? (dayjs.isDayjs(value) ? value : dayjs(value)) : value;
            setInnerValue(newValue);
            setInnerStrValue(value);
        }

    }, [value]);
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
        <TimePicker format={format} value={innerValue} onChange={handleChange} {...restProps} />            
    );
};

ITimePicker.propTypes = {
    value:PropTypes.any,
    format:PropTypes.string,
    onChange:PropTypes.func
};



