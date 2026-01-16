import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';

class LinkRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        show: PropTypes.bool,
        link: PropTypes.string
    };
    static defaultProps ={
        show: true
    };
    constructor(props) {
        super(props);
    }
    render() {
        const { value, show, link } = this.props;
        return (
            show &&
            <Link to={link}>
                {value}
            </Link>   
        );
    }
}

export { LinkRender };