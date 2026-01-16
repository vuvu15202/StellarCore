import React from 'react';
import PropTypes from 'prop-types';
import { formatDecimal} from 'shared/utils';
import { formatInt } from 'shared/utils';
class PinnedRowRenderer extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        style: PropTypes.any,
        is_number: PropTypes.any,
    };
    currencyFormat(value) {
        if (value) {
            if(typeof value === 'string')
                return value.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
            else
                return formatDecimal(value);
        }
    }
    render() {
        const {style, value, is_number} = this.props;
        return(
            <div style={style}>
                {
                    is_number ?
                        <span>{value && formatInt(value) || null}</span>
                        :
                        <span>{value && this.currencyFormat(value) || null}</span>
                }
            </div>
        );
    }
}
export {PinnedRowRenderer}; 