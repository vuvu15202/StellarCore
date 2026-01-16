import React, { Component } from 'react';
import PropTypes from 'prop-types';
import MaskedInput from 'react-text-mask';
import '../style.scss';
import { parse, format, parseISO } from 'date-fns';
import createAutoCorrectedDatePipe from 'text-mask-addons/dist/createAutoCorrectedDatePipe';

class DateEditor extends Component {
    static propTypes = {
        value: PropTypes.string,
        placeholder: PropTypes.string
    };
    constructor(props) {
        super(props);
        this.state = {
            value: props.value ? format(parseISO(props.value), (props.placeholder || 'dd/MM/yyyy')) : null
        };
    }
    
    componentDidMount() {
        setTimeout(() => {
            this.ref.inputElement.focus();
        }, 0);
        this.getMask();
    }
    getMask() {
        let placeholder = this.props.placeholder || 'dd/MM/yyyy';
        placeholder = placeholder.toLocaleLowerCase();
        let mask = placeholder.split('');
        mask = mask.map(x => {
            if (x === 'd' || x === 'm' || x === 'y') {
                return /\d/;
            } else {
                return x;
            }
        });
        const datePipe = createAutoCorrectedDatePipe(placeholder.toLocaleLowerCase());
        this.setState( {
            mask: mask,
            datePipe: datePipe
        });
    }
    /* Component Editor Lifecycle methods */
    // the final value to send to the grid, on completion of editing
    getValue() {
        let placeholder = this.props.placeholder || 'dd/MM/yyyy';
        if (this.state.value) {
            const date = parse(this.state.value, placeholder, new Date());
            if (!isNaN(Date.parse(date))) {
                return format(date, 'yyyy-MM-dd\'T\'HH:mm:ss.SSS');
            }
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
        const { placeholder } = this.props;

        return (
            this.state.mask ?
                <MaskedInput
                    ref={(r) => this.ref = r}
                    mask={this.state.mask}
                    guide={true}
                    pipe={this.state.datePipe}
                    className="ag-input-field-input ag-text-field-input custom"
                    id="mask-id"
                    placeholder={placeholder || 'dd/MM/yyyy'}
                    value={this.state.value}
                    onChange={(event) => {
                        let val = event.target.value;
                        this.setState({value: val});
                    }}
                /> : null
            
        );
    }
}

export { DateEditor };
