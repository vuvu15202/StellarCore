import React from 'react';
import PropTypes from 'prop-types';

class SelectRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        list_data: PropTypes.array,
        show: PropTypes.bool
    };
    static defaultProps ={
        show: true
    };
    constructor(props) {
        super(props);
    }
    lookupValue(arr, value) {
        if (value) {
            return arr?.find(x => x.value === value)?.label;
        }
    }
    
    render() {
        const { value, list_data, show } = this.props;
        return (
            show &&
            <span>{value && this.lookupValue(list_data, value)}</span>
        );
    }
}

export { SelectRender };