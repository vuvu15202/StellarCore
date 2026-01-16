import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { DatePicker } from 'antd';
import dayjs from 'dayjs';
const rangePresets = [
    {
        label: 'Last 7 Days',
        value: [dayjs().add(-7, 'd'), dayjs()],
    },
    {
        label: 'Last 14 Days',
        value: [dayjs().add(-14, 'd'), dayjs()],
    },
    {
        label: 'Last 30 Days',
        value: [dayjs().add(-30, 'd'), dayjs()],
    },
    {
        label: 'Last 90 Days',
        value: [dayjs().add(-90, 'd'), dayjs()],
    },
];
export const IDateRanger = (props) => {
    const { value, format = 'DD/MM/YYYY', onChange, ...restProps } = props;
    const [innerValue, setInnerValue] = useState(null);
    const [innerStrValue, setInnerStrValue] = useState(null);
    useEffect(() => {
        if (value != innerStrValue) {
            const newValue = value ? value.map(x => dayjs.isDayjs(x) ? x : dayjs(x)) : value;
            setInnerValue(newValue);
            setInnerStrValue(value);
        }

    }, [value]);
    const handleChange = (v) => {
        let newValue = null;
        if (v) {
            newValue = v.map(x => x.format('YYYY-MM-DDTHH:mm:ss'));
        }
        setInnerValue(v);
        setInnerStrValue(newValue);
        if (onChange) {
            onChange(newValue);
        }


    };

    return (
        <DatePicker.RangePicker presets={rangePresets} format={format} value={innerValue} onChange={handleChange} {...restProps} />
    );
};

IDateRanger.propTypes = {
    value: PropTypes.any,
    format: PropTypes.string,
    onChange: PropTypes.func
};



