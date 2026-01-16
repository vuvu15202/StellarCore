import React from 'react';
import PropTypes from 'prop-types';
class HtmlRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        show: PropTypes.bool
    };
    static defaultProps ={
        show: true
    };
    constructor(props) {
        super(props);
    }
    render() {
        const { value, show } = this.props;
        return (
            show &&
            <div dangerouslySetInnerHTML={{ __html: value }}>
            </div>
        );
    }
}

export { HtmlRender };