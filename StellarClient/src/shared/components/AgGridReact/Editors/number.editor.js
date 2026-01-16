import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {IMaskInput} from 'react-imask';
// import {numeral} from 'shared/utils';
import '../style.scss';


class NumberEditor extends Component {
    static propTypes = {
        value: PropTypes.any,
    };
    constructor(props) {
        super(props);
        this.state = {
            value: props.value?.toString(),
        };

    }
    
    componentDidMount() {
        setTimeout(() => {
            this.ref.focus();
        }, 0);
    }
    
    /* Component Editor Lifecycle methods */
    // the final value to send to the grid, on completion of editing
    getValue() {
        // this simple editor doubles any value entered into the input
        try{
            const v=this.state.value?Number(this.state.value):null;
            return v;
        }catch(e){
            console.log(e);
        }
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
                className="ag-input-field-input ag-text-field-input custom"
                scale={0}
                radix=","
                mapToRadix={ [',']}
                thousandsSeparator="."
                normalizeZeros= {true}
                value={this.state.value}
                unmask={true} // true|false|'typed'
                inputRef={(r) => this.ref = r}  // access to nested input
                // DO NOT USE onChange TO HANDLE CHANGES!
                // USE onAccept INSTEAD
                onAccept={
                    // depending on prop above first argument is
                    (value) => {
                        this.setState({value:value});                                           
                    }
                }

            />            
            
        );
    }
}

export { NumberEditor };
