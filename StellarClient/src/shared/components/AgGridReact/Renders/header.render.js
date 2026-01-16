import React from 'react';
import PropTypes from 'prop-types';
import '../style.scss';

class HeaderRender extends React.PureComponent {
    static propTypes = {
        displayName: PropTypes.string,
        style: PropTypes.object,
    };
    constructor(props) {
        super(props);
        this.state = {
        };
    }

    componentDidMount() {
    }

    render() {
        const { displayName, style } = this.props;
        return (
            <div className='header' style={style}>
                {displayName}
            </div>
        );
    }
}
export { HeaderRender };