import React, { Component } from 'react';
import PropTypes from 'prop-types';

export class CheckBoxNewRender extends Component {
    static propTypes = {
        value: PropTypes.any,
        onChange: PropTypes.func,
        editMode:PropTypes.any,
        style: PropTypes.any,
        hide: PropTypes.bool
    };
    constructor(props) {
        super(props);
    }
    checkedHandler(event) {
        let checked = event.target.checked;
        if(this.props.onChange){
            this.props.onChange(checked);
        }
    }
    render() {
        const { value, editMode, style, hide} = this.props;
        return (
            <div style={style? style: {textAlign: 'center'}}>
                <input
                    type="checkbox"
                    onClick={this.checkedHandler.bind(this)}
                    checked={value}
                    disabled={!editMode}               
                    hidden={hide} 
                />
            </div>

        );
    }
}