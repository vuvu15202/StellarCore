import React, { Component } from 'react';
import PropTypes from 'prop-types';
import MaskedInput from 'react-text-mask';
import '../style.scss';
import createNumberMask from 'text-mask-addons/dist/createNumberMask';

const numberMask = createNumberMask({
    prefix: '',
    suffix: '',
});

class NumberNotDotEditor extends Component {
    static propTypes = {
        value: PropTypes.any,
    };
    constructor(props) {
        super(props);
    
        this.state = {
            value: props.value,
        };
    }
    
    componentDidMount() {
        setTimeout(() => {
            this.ref.inputElement.focus();
        }, 0);
    }
    
    /* Component Editor Lifecycle methods */
    // the final value to send to the grid, on completion of editing
    getValue() {
        // this simple editor doubles any value entered into the input
        return this.state.value;
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
            <MaskedInput
                ref={(r) => this.ref = r}
                mask={numberMask}
                className="ag-input-field-input ag-text-field-input custom"
                guide={false}
                id="nummask-id"
                value={this.state.value}
                onChange={(event) => {
                    this.setState({ value: Number(event.target.value.replace(/,/g, '')) });
                    // this.setState({ value: numeral(event.target.value).format('0,0[.]000000')  });
                    // this.setState({ value: event.target.value });
                }}
            />
            
        );
    }
}

export { NumberNotDotEditor };
