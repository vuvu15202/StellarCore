import React from 'react';
import PropTypes from 'prop-types';
import { Form } from 'react-bootstrap';
// import { baoCaoService } from 'modules/kiemtradinhky/baocao/BaoCaoService';

class CheckBoxRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        node: PropTypes.any,
        onChange: PropTypes.func,
        editMode:PropTypes.any,
        style: PropTypes.any,
        hide: PropTypes.bool
    };
    constructor(props) {
        super(props);
        this.state = {
            value: props.value ? props.value : false,
            editMode: props.editMode
        };
    }
    onChange() {
        this.setState({ value: !this.state.value}, () => {
            if (this.props.onChange) {
                this.props.onChange(this.props.node ? this.props.node?.data?.id ? this.props.node?.data?.id : this.props.node?.data?.cong_viec_id ? this.props.node?.data?.cong_viec_id : null : null, this.state.value);
            }
        });
    }

    componentDidMount() {
        // baoCaoService.sendToSubs.subscribe((res) => {
        //     if(this.props.node?.data?.phan_tram_hoan_thanh != 100)
        //     {
        //         this.setState({
        //             editMode: res.editMode
        //         });
        //     }
        //     else{
        //         this.setState({
        //             editMode: false
        //         });
        //     }
        // });
    }
    
    componentDidUpdate(prevProps){
        if(prevProps.editMode !== this.props.editMode){
            this.setState({
                editMode: this.props.editMode
            });
        }
    }

    render() {
        const {
            style,
            hide} = this.props;
        return (
            !hide &&
            <Form.Check
                style={style? style: {textAlign: 'center'}}               
                type="checkbox"
                onChange={() => {
                    this.onChange();
                }}
                custom
                disabled = {!this.state.editMode}
                checked={this.state.value ? this.state.value : false}
            />
        );
    }
}

export { CheckBoxRender };