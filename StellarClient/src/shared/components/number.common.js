import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {IMaskInput} from 'react-imask';


class NumberCommon extends Component {
    static propTypes = {
        value: PropTypes.any,
        id: PropTypes.string,
        onChange: PropTypes.func,
        autoFocus : PropTypes.any,
        max: PropTypes.number,
        min: PropTypes.number
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
        return (
            <IMaskInput
                mask={Number}
                max={this.props.max ?? null}
                min={this.props.min ?? null}
                className="form-control-sm form-control"
                scale={0}
                radix=","
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

            />           
            
        );
    }
}

export { NumberCommon };
