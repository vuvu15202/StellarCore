import React from 'react';
import PropTypes from 'prop-types';
import { formatInt } from 'shared/utils';
class NumberRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        styleNumber: PropTypes.any,
        show: PropTypes.bool
    };
    static defaultProps ={
        show: true
    };
    constructor(props) {
        super(props);

    }


    render() {
        const { value, styleNumber, show } = this.props;
        return (
            show &&
            <div style={{textAlign: 'right'}}>
                <span style={styleNumber}>{value && formatInt(value) || null}</span>
            </div>

        );
    }
}

export { NumberRender };