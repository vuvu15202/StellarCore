import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {IMaskInput} from 'react-imask';


class DecimalNumberCommon extends Component {
    static propTypes = {
        value: PropTypes.any,
        id: PropTypes.string,
        onChange: PropTypes.func,
        disabled: PropTypes.bool,
        max: PropTypes.any,
        scale: PropTypes.number
    };
    static defaultProps = {
        scale:7
    };
    constructor(props) {
        super(props);
        this.state = {
            value: props.value?.toString(),
        };

    }
    
    componentDidMount() {
    }
    componentDidUpdate(prevProps){
        if(prevProps.value!== this.props.value){        
            this.setState({value:this.props.value?.toString()}); 
        }
    }
        
    render() {
        const { disabled, max, scale } = this.props;
        return (
            <IMaskInput
                mask={Number}
                className="form-control-sm form-control"
                scale={scale}
                radix=","
                max={max}
                mapToRadix={ ['.']}
                thousandsSeparator=" "
                normalizeZeros= {true}
                value={this.state.value}
                unmask={true} // true|false|'typed'
                inputRef={el => this.input = el}  // access to nested input
                // DO NOT USE onChange TO HANDLE CHANGES!
                // USE onAccept INSTEAD
                onAccept={
                    // depending on prop above first argument is
                    (value) => {
                        this.setState({value:value});                                        
                        const v=value?Number(value):value;
                        this.props.onChange(v);
                    }
                }
                disabled={disabled || false}
               

            />           
            
        );
    }
}

export { DecimalNumberCommon };
