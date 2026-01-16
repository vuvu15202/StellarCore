import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {IMaskInput} from 'react-imask';
import {formatNumber} from 'shared/utils';
import '../style.scss';


class DecimalNumberEditor extends Component {
    static propTypes = {
        value: PropTypes.any,
        maxValue: PropTypes.number,
        minValue: PropTypes.number,
        signed: PropTypes.bool,
        scale: PropTypes.number,
        formatNumberProp: PropTypes.string
    };
    static defaultProps = {
        signed:true,
        scale: 7,
        formatNumberProp: '0,0[.]0000'
    };
    constructor(props) {
        super(props);
        this.state = {
            value: formatNumber(props?.value, props?.formatNumberProp) === 0 ? '0': formatNumber(props?.value, props?.formatNumberProp),
        };
    }
    
    componentDidMount() {
        setTimeout(() => {
            this.ref?.focus();
        }, 0);
    }
    
    /* Component Editor Lifecycle methods */
    // the final value to send to the grid, on completion of editing
    getValue() {
        // this simple editor doubles any value entered into the input
        //const v=this.state.value?numeral(this.state.value).value():this.state.value;
        try{
            const v=this.state.value?Number(this.state.value):null;
            return v;
        }catch(e){
            console.log(e);
        }
        // const v=this.state.value?Number(this.state.value).value():this.state.value;
        // console.log( v);
        //   return v;
    }
    
    // Gets called once before editing starts, to give editor a chance to
    // cancel the editing before it even starts.
    isCancelBeforeStart() {
        return false;
    }
    
    // Gets called once when editing is finished (eg if enter is pressed).
    // If you return true, then the result of the edit will be ignored.
    isCancelAfterEnd() {
        return false;
    }
  
    render() {
        return (
            <IMaskInput
                mask={Number}
                className="ag-input-field-input ag-text-field-input ag-input-field-input-number"
                scale={this.props.scale}
                radix=","
                signed={this.props.signed}
                mapToRadix={ [',']}
                thousandsSeparator=" "
                normalizeZeros= {true}
                value={this.state.value}   
                min={this.props.minValue} 
                max={this.props.maxValue}
                unmask={true} // true|false|'typed'
                inputRef={(r) => this.ref = r}  // access to nested input
                // DO NOT USE onChange TO HANDLE CHANGES!
                // USE onAccept INSTEAD
                onAccept={
                    // depending on prop above first argument is
                    (value) => {
                        this.setState({value:value});                    
                        //const v=value?numeral(value).value():value;
                        //this.props.onChange(v);
                    }
                }               

            />            
            
        );
    }
}

export { DecimalNumberEditor };
