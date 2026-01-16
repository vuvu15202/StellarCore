import React from 'react';
import PropTypes from 'prop-types';
// import '../../style.scss';
import classNames from 'classnames';

class ContentRender extends React.PureComponent {
    static propTypes = {
        item: PropTypes.any,
    };
    constructor(props) {
        super(props);

    }

    render() {
        const { item } = this.props;
        return (
            <div className={classNames(
                {
                    'ell': true,
                    ['cap_' + item?.cap]: true
                })}
            style={{ paddingLeft: (item?.cap - 1 || 0) * 15 }}>
                {
                    item?.has_child ?
                        <span style={{ fontWeight: 'bold' }}>
                            {item?.ten}
                        </span>
                        :
                        <span >
                            {item?.ten}
                        </span>
                }
            </div>
        );
    }
}
export { ContentRender };
