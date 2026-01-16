import React from 'react';
import PropTypes from 'prop-types';
import {numeral} from 'shared/utils';

class DecimalNumberRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        styleNumber: PropTypes.any,
        format:PropTypes.any,
        unit: PropTypes.number
    };
    static defaultProps ={
        show: true,
        unit: 1
    };
    constructor(props) {
        super(props);

    }

    render() {
        const { value, styleNumber ,format, unit = 1} = this.props;
        let formatStr = unit == 1000 ? '0,0' : (format||'0,0.00');
        return (
            <div style={{textAlign: 'right'}}>
                <span style={styleNumber}>{value && numeral(value/unit).format(formatStr)}</span>                                   
            </div>
        );
    }
}

export { DecimalNumberRender };