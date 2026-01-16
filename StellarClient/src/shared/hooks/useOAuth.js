import {useLayoutEffect, useState} from 'react';
import {useOAuthConfigurationStore} from '../stores/useOAuthConfigurationStore';

import  {pkceChallenge} from 'shared/utils';
import {useOauthStore} from '../stores/useOauthStore';
import {useNavigate, useSearchParams} from 'react-router-dom';
import {authService} from '../services';
import {useUserProfileStore} from '../stores/useUserProfileStore';

export const useOAuth = ({automaticOAuth} = {}) => {
    const {
        isLoading, isAuthenticated, authorizeSession, actions: {
            setLoading, setSessionState, setCodeChallenge, setAuthenticated, clearStorage: clearOauthStorage
        }
    } = useOauthStore();
    
    const {
        serverConfig: {
            authorization_endpoint, token_endpoint, end_session_endpoint,
        }, clientConfig: {
            post_logout_redirect_uri, redirect_uri, scope, client_id, response_type
        }
    } = useOAuthConfigurationStore();
    const {id_token, actions: {clearStorage: clearUserProfileStorage}} = useUserProfileStore();
    const [errorMessage, setErrorMessage] = useState();

    const navigate = useNavigate();

    if (automaticOAuth) {
        const [searchParams] = useSearchParams();
        useLayoutEffect(() => {
            const code = searchParams.get('code');
            const state = searchParams.get('session_state');
            if (isAuthenticated) {
                navigateDashboard();
                return;
            }
            if (code) {
                setLoading(true);
                setSessionState(state, code);               
                authService.getToken({
                    token_endpoint, redirect_uri, code_verifier: authorizeSession?.code_verifier, code, client_id
                }).subscribe({
                    error: (err) => {
                        setLoading(false);
                        if (err.status === 400) {
                            setErrorMessage('Tài khoản hoặc mật khẩu không chính xác, vui lòng kiểm tra lại');
                        } else {
                            setErrorMessage('Có lỗi xảy ra trong quá trình login');
                        }
                    }, complete: () => {
                        setLoading(false);
                        setAuthenticated(true);
                        navigateDashboard();
                    }
                });
            }
            setLoading(false);

        }, [searchParams]);
    }

    const navigateDashboard = () => {
        const returnUrl = localStorage.getItem('returnUrl') || '/';
        localStorage.setItem('returnUrl', '/');
        navigate(returnUrl, {replace: true});
    };

    const login =  () => {
        let authUrl = `${authorization_endpoint}?client_id=${client_id}&scope=${scope}&response_type=${response_type}&redirect_uri=${redirect_uri}`;
        if (response_type === 'code') {
            let codeChallenge = authorizeSession?.code_challenge;
            // Nếu chưa có code_challenge thì tạo mới
            // và lưu vào authorizeSession
            if (!codeChallenge || codeChallenge === '') {
                // Tạo code_challenge và code_verifier mới
                const challenge =  pkceChallenge(128);
                setCodeChallenge(challenge.code_challenge, challenge.code_verifier);
                codeChallenge = challenge.code_challenge;
            }
            authUrl += `&code_challenge=${codeChallenge}&code_challenge_method=S256`;
        }
        // Lưu returnUrl để sau khi login xong sẽ điều hướng về trang này
        localStorage.setItem('returnUrl', location.pathname);
        window.location.href = authUrl;
    };

    const logout = () => {
        window.location.href = `${end_session_endpoint}?id_token_hint=${id_token}&post_logout_redirect_uri=${post_logout_redirect_uri}`;
    };

    const handlePostLogout = () => {
        clearOauthStorage();
        clearUserProfileStorage();
        setAuthenticated(false);
        navigateDashboard();
    };

    return {
        isLoading: isLoading, isAuthenticated: isAuthenticated, errorMessage: errorMessage, actions: {
            login, logout, handlePostLogout
        }

    };
};