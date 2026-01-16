import {useLayoutEffect} from 'react';
import {useOAuthConfigurationStore} from '../stores/useOAuthConfigurationStore';
import {authService} from '../services';

export const AuthorizationProvider = ({config, children}) => {

    const {actions: {setClientConfig, setServerConfig}} = useOAuthConfigurationStore();

    useLayoutEffect(() => {
        setClientConfig(config);
        const serverConfig = `${config?.authority}/.well-known/openid-configuration`;
        authService.getOauthConfig(serverConfig).subscribe(res => {
            setServerConfig(res);
        }, err => {
            console.log(err);
        });
    }, [config]);

    // eslint-disable-next-line react/react-in-jsx-scope
    return (<>{children}</>);
};

AuthorizationProvider.prototype = {
    config: {
        authority: '',
        client_id: '',
        redirect_uri: '',
        silent_redirect_uri: '',
        post_logout_redirect_uri: '',
        response_type: '',
        scope: '',
        config_uri: ''
        // automaticSilentRenew: false,
        // loadUserInfo: false,
    }
};

