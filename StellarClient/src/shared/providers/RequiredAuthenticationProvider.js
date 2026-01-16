import { Layout, Spin } from 'antd';
import React, { useEffect, useMemo, useState, useRef } from 'react';
import { useOAuth } from 'shared/hooks/useOAuth';
import { useOauthStore } from '../stores/useOauthStore';
import { useOAuthConfigurationStore } from '../stores/useOAuthConfigurationStore';
import { TIME_CHECK } from '../app-setting';

export const RequiredAuthenticationProvider = ({ children }) => {

    const { isAuthenticated, actions: { login } } = useOAuth();
    const { authorizeSession: { state } } = useOauthStore();
    const { clientConfig: { client_id, authority }, serverConfig: { check_session_iframe } } = useOAuthConfigurationStore();
    const [isLoading, setIsLoading] = useState(true);
    const iframeRef = useRef(null);
    const origin = useMemo(() => {
        return new URL(authority).origin;
    }, [authority]);

    useEffect(() => {
        if (!isAuthenticated) {
            login();
        }
    }, [isAuthenticated]);

    useEffect(() => {
        if (!isAuthenticated) {
            return;
        }
        const iframe = document.createElement('iframe');

        iframe.style.display = 'none';
        iframe.src = `${check_session_iframe}?client_id=${client_id}&origin=${'http://localhost:3000'}`;

        document.body.appendChild(iframe);
        iframeRef.current = iframe;
        let sub = null;
        const handleMessage = (event) => {
            console.log(event);
            if (event.origin !== origin) return; // Chỉ nhận từ Keycloak
            if (event.data === null || event.data === undefined) {
                return;
            }
            if (event.data === 'unchanged') {
                if (isLoading) {
                    setIsLoading(false);
                }
            } else {
                // logout();
                console.log('changed: logout');
            }

        };

        if (TIME_CHECK) {
            checkStatus();
            sub = setInterval(checkStatus, 1000 * TIME_CHECK);
            window.addEventListener('message', handleMessage, false);
        } else {
            setIsLoading(false);
        }


        return () => {
            try {
                if (sub) {
                    clearInterval(sub);
                }
                window.removeEventListener('message', handleMessage, false);
            } catch (err) {
                //empty
            }
        };
    }, [state, client_id, isAuthenticated]);

    const checkStatus = () => {
        const text = client_id + ' ' + state;
        if (iframeRef?.current?.contentWindow && state && client_id) {
            iframeRef?.current?.contentWindow.postMessage(text, '*');
        }
    };


    return <>{!isLoading && isAuthenticated ? children : <Spin spinning={isLoading} size="large">
        <Layout style={{ minHeight: '100vh' }}>
        </Layout>
    </Spin>}</>;
};