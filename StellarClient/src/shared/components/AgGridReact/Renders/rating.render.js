import React from 'react';
import PropTypes from 'prop-types';
import Rate from 'rc-rate';
class RatingRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        disabled:PropTypes.bool,
        onChange:PropTypes.func
    };
    static defaultProps ={
        disabled: false
    };
    constructor(props) {
        super(props);

    }


    render() {
        const { value ,disabled,onChange} = this.props;
        return (
            <Rate value={value} disabled={disabled} count={5} onChange={(v)=>{
                onChange&&onChange(v);            
            }} /> 
        );
    }
}

export { RatingRender };