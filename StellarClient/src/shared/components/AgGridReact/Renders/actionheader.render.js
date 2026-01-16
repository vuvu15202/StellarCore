import React from 'react';
import PropTypes from 'prop-types';
import { OverlayTrigger, Tooltip } from 'react-bootstrap';
import {Icon} from '@iconify/react';
class ActionHeader extends React.PureComponent {
    static propTypes = {
        agGridReact: PropTypes.any,
        displayName: PropTypes.string,
        action: PropTypes.any,
        style: PropTypes.any,
    };
    constructor(props) {
        super(props);
    }

    componentDidMount() {
    }

    render() {
        const { displayName, action, style } = this.props;
        return (
            <div className='header'>
                <div style={{flex: 9, textAlign: 'center'}}>
                    <span style={{textOverflow: 'inherit', whiteSpace: 'normal'}}>{displayName}</span>
                </div>
                {
                    action && action.map((x,i) => {
                        return(
                            (x && x?.show) &&
                            <div className='action float-right flex-1' style={style} key={i}>                
                                <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">{x.tooltip}</Tooltip>}>
                                    <div onClick={x.callback}>
                                        <Icon className="iconify icon" icon={x.icon} data-inline="false" style={{ marginRight: 10, fontSize: 20 }}/>
                                    </div>
                                </OverlayTrigger>                                       
                            </div>
                        );
                    })
                }
                
            </div>

        );
    }
}
export { ActionHeader };
