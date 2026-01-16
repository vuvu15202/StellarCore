
import React from 'react';
import PropTypes from 'prop-types';

class EmptyRender extends React.PureComponent {
    static propTypes = {
        message: PropTypes.string
    };
    
    constructor(props) {
        super(props);
    }

    render() {
        const { message } = this.props;
        return (
            <p className='empty'>
                { message }
            </p>
        );
    }
}

export { EmptyRender };