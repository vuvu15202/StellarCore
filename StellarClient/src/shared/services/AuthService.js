import { concatMap, map } from 'rxjs/operators';
import { forkJoin, of } from 'rxjs';
import { http, jsonToUrlencoded } from 'shared/utils';
import { AUTHORIZATION_BASE } from '../app-setting';
import { useUserProfileStore } from 'shared/stores/useUserProfileStore';

class AuthService {

    constructor() {
    }

    getToken({ token_endpoint, client_id, code, redirect_uri, grant_type = 'authorization_code', code_verifier }) {
        const body = jsonToUrlencoded({
            client_id,
            grant_type,
            code,
            redirect_uri,
            code_verifier
        });

        return http.post(token_endpoint, body, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).pipe(
            concatMap((token) => {
                console.log(token);

                return forkJoin(
                    {
                        token: of(token)
                    }
                );
            }), map((res) => {
                useUserProfileStore.getState().actions.setToken(res.token);
                return res.token;
            })
        );
    }

    logout({ end_session_endpoint, post_logout_redirect_uri }) {
        const url = `${end_session_endpoint}?post_logout_redirect_uri=${post_logout_redirect_uri}`;
        return http.post(url).pipe(
            map((res) => {
                return res;
            })
        );
    }

    refreshToken(token_endpoint, refresh_token, client_id) {
        const body = jsonToUrlencoded({
            grant_type: 'refresh_token',
            refresh_token: refresh_token,
            client_id: client_id
        });
        return http.post(token_endpoint, body, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': AUTHORIZATION_BASE
            }
        }).pipe(
            concatMap((token) => {
                console.log(token);

                return forkJoin(
                    {
                        token: of(token)
                    }
                );
            }), map((res) => {
                useUserProfileStore.getState().actions.setToken(res.token);
                return res.token;
            })
        );
    }

    getCurrentPermissionByNhomNguoiDung(nhomNguoiDungId = null) {
        const params = nhomNguoiDungId ? `?nhomNguoiDungId=${nhomNguoiDungId}` : '';
        return http.get(`/user-service/nhom-nguoi-dung/current-permission-by-nhom-nguoi-dung${params}`);
    }

    getOauthConfig(server_config_url) {
        return http.get(server_config_url);
    }
}

const authService = new AuthService();
export { authService, AuthService };
