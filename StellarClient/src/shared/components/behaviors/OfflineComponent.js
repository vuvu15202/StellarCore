import React from 'react';
import { Offline } from 'react-detect-offline';
import { Icon } from '@iconify/react';
import wifiOff from '@iconify-icons/mdi/wifi-off';
import { API_URL } from '../../app-setting';

class OfflineComponent extends React.Component {
    constructor(props) {
        super(props);
        this.state = {

        };
        this.subscriptions = {};
    }

    componentDidMount() {

    }
    componentWillUnmount() {
        Object.keys(this.subscriptions).forEach((key) => {
            this.subscriptions[key].unsubscribe();
        });
    }


    render() {
        return (
            <Offline
                polling={{
                    url: `${API_URL}api/values/is-online`
                }}
            ><div className="overlay-offline">
                    <div className="offline-message-box">
                        <div className="offline-icon">
                            <Icon icon={wifiOff} />
                        </div>
                        <span className="offline-message">
                            Lỗi kết nối mạng. Vui lòng kiểm tra đường truyền mạng
                        </span>

                    </div>

                </div></Offline>
        );
    }
}

export { OfflineComponent };
