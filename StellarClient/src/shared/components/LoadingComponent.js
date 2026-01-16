import React from 'react';
import PropTypes from 'prop-types';
import { Spinner } from 'react-bootstrap';
class LoadingComponent extends React.Component {
    static propTypes = {
        loading: PropTypes.bool,
    };
    render() {
        const { loading } = this.props;
        return (
            <React.Fragment>
                {
                    loading &&
                    <div className="loading-warp">
                        <Spinner animation="border" role="status">
                            <span className="sr-only">Loading...</span>
                        </Spinner>
                    </div>
                }
            </React.Fragment>
        );
    }
}

export { LoadingComponent };
