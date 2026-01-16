import React from 'react';
import PropTypes from 'prop-types';
import { OverlayTrigger, Tooltip } from 'react-bootstrap';
import '../style.scss';
import {Icon} from '@iconify/react';

class ActionRender extends React.PureComponent {
    static propTypes = {
        actions: PropTypes.array,
        style: PropTypes.any,
    };
    constructor(props) {
        super(props);

    }
    render() {
        const { actions, style } = this.props;
        return (
            <div className='action' style={style}>
                {
                    actions &&
                    actions.map((x, i) => {
                        return (
                            x.show &&
                            <OverlayTrigger key={i} overlay={<Tooltip id="tooltip-disabled">{x.tooltip}</Tooltip>}>
                                <div onClick={x.callback} style={x.style}>
                                    <Icon className="iconify icon" icon={x.icon} data-inline="false" style={{ marginRight: 10, fontSize: 20 }}/>
                                </div>
                            </OverlayTrigger>
                        );
                    })
                }
            </div>
        );
    }
}

export { ActionRender };