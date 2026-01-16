import {create} from 'zustand';
import {createJSONStorage, persist} from 'zustand/middleware';

const defaultValue = {
    serverConfig: {
        issuer: '',
        check_session_iframe: '"http://sso.tt1.eps.com/realms/master/protocol/openid-connect/login-status-iframe.html',
        authorization_endpoint: 'http://sso.tt1.eps.com/realms/master/protocol/openid-connect/auth',
        // authorization_endpoint: 'http://localhost:8080/realms/master/protocol/openid-connect/auth',
        token_endpoint: 'http://sso.tt1.eps.com/realms/master/protocol/openid-connect/token',
        // token_endpoint: 'http://localhost:8080/realms/master/protocol/openid-connect/token',
        end_session_endpoint: 'http://sso.tt1.eps.com/realms/master/protocol/openid-connect/logout',
    },
    clientConfig: {
        authority: '',
        client_id: '',
        redirect_uri: '',
        silent_redirect_uri: '',
        post_logout_redirect_uri: '',
        response_type: '',
        scope: '',
        config_uri: ''
    }
};

export const useOAuthConfigurationStore = create(persist(
    (set) => ({
        ...defaultValue,
        actions: {
            setData: (config) => {
                set({...Object.assign(defaultValue, config)});
            }, setClientConfig: (clientConfig) => {
                set({clientConfig: clientConfig});
            }, setServerConfig: (serverConfig) => {
                set({serverConfig});
            }
        }
    }), {
        name: 'oauth-configuration-storage', // name of the item in the storage (must be unique)
        storage: createJSONStorage(() => localStorage), // (optional) by default, 'localStorage' is used
        partialize: (state) => ({
            serverConfig: state.serverConfig,
            clientConfig: state.clientConfig
        })
    }
));

