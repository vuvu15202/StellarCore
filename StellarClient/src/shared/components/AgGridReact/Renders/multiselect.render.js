import React from 'react';
import PropTypes from 'prop-types';

class MultiSelectRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        list_data: PropTypes.array,
        styleItem: PropTypes.object
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
        const { value, list_data, styleItem } = this.props;
        return (
            <>
                {
                    value && value.length >0 && list_data && list_data.length >0 && value.map(x => {
                        return(
                            <div key={x} style={styleItem}>
                                <span >{x && this.lookupValue(list_data, x)} <br/></span>
                            </div>
                        );
                    })
                }

            </>
        );
    }
}

export { MultiSelectRender };