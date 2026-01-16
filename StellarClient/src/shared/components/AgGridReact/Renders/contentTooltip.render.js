import React from 'react';
import PropTypes from 'prop-types';
import { OverlayTrigger, Tooltip } from 'react-bootstrap';
import '../style.scss';
import { formatDecimal } from 'shared/utils';
class ContentTooltipRender extends React.PureComponent {
    static propTypes = {
        actions: PropTypes.array,
        style: PropTypes.any
    };
    constructor(props) {
        super(props);
    }
    currencyFormat(value) {
        if (value) {
            if(typeof value === 'string'){
                return value.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
            }else{
                return formatDecimal(value);

            }
        }
        return value;
    }
    render() {
        const { actions, style } = this.props;
        return (
            <div style={style}>
                {
                    actions &&
                    actions.map((x, i) => {
                        return (
                            x.show &&
                            <OverlayTrigger key={i} overlay={<Tooltip id="tooltip-disabled">{x.tooltip}</Tooltip>}>
                                <div onClick={x.callback} >
                                    {this.currencyFormat(x?.value)}
                                </div>
                            </OverlayTrigger>
                        );
                    })
                }
            </div>
        );
    }
}

export { ContentTooltipRender };