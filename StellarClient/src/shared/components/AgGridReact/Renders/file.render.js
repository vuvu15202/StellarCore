import React from 'react';
import PropTypes from 'prop-types';
import { API_URL } from '../../../app-setting';
import { Icon } from '@iconify/react';

class ListFileRender extends React.PureComponent {
    static propTypes = {
        value: PropTypes.any,
        show: PropTypes.bool,
        ds_file: PropTypes.array
    };
    static defaultProps = {
        show: true
    };
    constructor(props) {
        super(props);
    }
    render() {
        const { show, ds_file } = this.props;
        return (
            show &&
            ds_file?.map((x, index) => (<span key={index}><Icon icon="oi:paperclip" width={'1em'} height={'1em'} /> <a target='blank' href={`${API_URL}api/file-view/${x.id}`}>{x.name}</a><br /></span>))
        );
    }
}

export { ListFileRender };