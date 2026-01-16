import React from 'react';
import PropTypes from 'prop-types';
import { format, parseISO } from 'date-fns';

class DateMonthYearRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.string,
        placeholder: PropTypes.string
    };
    constructor(props) {
        super(props);

    }

    render() {
        const { value } = this.props;
        let placeholder = this.props.placeholder || 'MM/yyyy';
        return (
            <span>{value && format(parseISO(value), placeholder)}</span>
        );
    }
}

export { DateMonthYearRender };