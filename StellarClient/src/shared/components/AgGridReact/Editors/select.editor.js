import React, { Component } from 'react';
import PropTypes from 'prop-types';
import '../style.scss';
import Select from 'react-select';
class SelectEditor extends Component {
    static propTypes = {
        value: PropTypes.any,
        list_data: PropTypes.array,
        eGridCell: PropTypes.object,
        mode: PropTypes.string,
        api: PropTypes.any,
        placeholder: PropTypes.any,
        onChange:PropTypes.func,
        node:PropTypes.any,
        isTree: PropTypes.bool
    };
    constructor(props) {
        super(props);
        this.state = {
            value: props.value,
        };
    }

    componentDidMount() {
        const that =this;
        setTimeout(() => {
            that.ref.querySelector('.select__input input').focus();
        }, 0);
    }

    /* Component Editor Lifecycle methods */
    // the final value to send to the grid, on completion of editing
    getValue() {
        // this simple editor doubles any value entered into the input
        if(this.state.value != null && this.state.value != undefined && this.state.value !== '') //ko thêm dòng này thì giá trị ô chuc_danh_id require ở ngoài cha ko thay đổi ko chạy được vào valueSetter
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
    isPopup() {
        return true;
    }

    onchange(event){
        this.setState({ value: event.value }, () => {
            if(this.props.onChange){
                this.props.onChange(event.value,this.props.node);
            }
            this.props.api.stopEditing();
        });
    }

    render() {
        const { placeholder, isTree } = this.props;
        let data = this.props.list_data || [];
        const width= this.props.eGridCell.clientWidth;
        return (
            <div  ref={(r) => this.ref = r} style={{width: width}} 
            >
                <Select
                    placeholder={placeholder ? placeholder: 'Chọn giá trị ...'}
                    className="basic-single"
                    classNamePrefix="select"
                    name="color"
                    autoFocus
                    options={data}
                    menuPlacement="auto"
                    defaultMenuIsOpen={true}
                    onChange={(event) => {
                        this.onchange(event);
                    }}
                    isOptionDisabled={(rr) => {
                        return rr.disable ? rr.disable : false;
                    }}
                    styles={isTree&& customStyles}
                />
            </div>
        );
    }
}

export { SelectEditor };
const customStyles= {
    option: (provided, state) => {
        const cap = (state.data.cap && state.data.cap)|| 1;
        return {
            ...provided,
            marginLeft: cap === 1? 5 : (15 *cap),
        };
    }
};
