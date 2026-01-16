import React, { Component } from 'react';
import PropTypes from 'prop-types';
import '../style.scss';
import { Form } from 'react-bootstrap';

class RequireEditor extends Component {
    static propTypes = {
        value: PropTypes.any,
    };
    constructor(props) {
        super(props);
    
        this.state = {
            value: props.value,
        };
        this.inputRef = React.createRef();
    }
    
    componentDidMount() {
        setTimeout(() => {
            this.inputRef.current?.focus();
        }, 100);
    }
    
    /* Component Editor Lifecycle methods */
    // the final value to send to the grid, on completion of editing
    getValue() {
        // this simple editor doubles any value entered into the input
  
        if(this.state.value != null && this.state.value != undefined && this.state.value !== '') //ko thêm dòng này thì giá trị ô stt require ở ngoài cha ko thay đổi ko chạy được vào valueSetter
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
            <Form.Control
                style={(this.state.value == '' || this.state.value == null || this.state.value == undefined) ? {borderColor:'red', paddingBottom: '1px'} : {}}
                className="ag-input-field-input ag-text-field-input custom"
                type="text"
                name="stt"
                value={this.state.value}
                onChange={event => this.setState({value: event.target.value})}
                ref={this.inputRef}
            />
        );
    }
}

export { RequireEditor };
