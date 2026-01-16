import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { IUploadFile } from 'shared/components/Input';

class UploadFileAggrid extends Component {
    static propTypes = {
        value: PropTypes.any
    };
    constructor(props) {
        super(props);
        this.state = {
            value: props.value,
        };

    }
    
    componentDidMount() {
    }
    /* Component Editor Lifecycle methods */
    // the final value to send to the grid, on completion of editing
    getValue() {
        // this simple editor doubles any value entered into the input
        return  this.state.value;
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
            <IUploadFile value={this.state.value} {...this.props}  confirmDelete={false} onChange={(val) => {this.setState({value: val});}}/>
        );
    }   
}

export { UploadFileAggrid };
