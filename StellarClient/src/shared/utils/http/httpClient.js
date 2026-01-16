import axios from 'axios';
import {HttpErrorResponse, HttpHandle, HttpHandleExport} from './htppUtils';
import {BehaviorSubject, Observable, throwError} from 'rxjs';
import {catchError, filter, finalize, switchMap, take,} from 'rxjs/operators';
import {authService,} from 'shared/services';
import {useUserProfileStore} from 'shared/stores/useUserProfileStore';
import {useOAuthConfigurationStore} from '../../stores/useOAuthConfigurationStore';
import {useOauthStore} from '../../stores/useOauthStore'; // Đường dẫn điều chỉnh tùy dự án
import {SelectedFunctionStore} from 'shared/services/SelectedFunctionStore';

export default class HttpClient {
    constructor(options = {}) {
        this.apiUrl = options.apiUrl;
        this.authorizationBase = options.authorizationBase;
        this.tokenSubject = new BehaviorSubject(null);
        this.isRefreshingToken = false;
        axios.defaults.baseURL = this.apiUrl;

        this.oauth = useUserProfileStore?.getState();
        useUserProfileStore.subscribe((state) => {
            this.oauth = state;
        });

        this.oauthConfig = useOAuthConfigurationStore?.getState();
        useOAuthConfigurationStore.subscribe((state) => {
            this.oauthConfig = state;
        });
    }

    request(config) {
        return HttpHandle(this.updateConfig(config, this.oauth)).pipe(catchError(error => {
            if (error instanceof HttpErrorResponse) {
                switch (error.status) {
                    case 400:
                        return this.handle400Error(error);
                    case 401:
                        return this.handle401Error(config, error);
                    default:
                        return throwError(error);

                }
            } else {
                return throwError(error);
            }
        }));
    }

    requestExport(config) {
        return HttpHandleExport(this.updateConfig(config, this.oauth)).pipe(catchError(error => {
            if (error instanceof HttpErrorResponse) {
                switch (error.status) {
                    case 400:
                        return this.handle400Error(error);
                    case 401:
                        return this.handle401Error(config, error);
                    default:
                        return throwError(error);

                }
            } else {
                return throwError(error);
            }
        }));
    }

    get(url, config) {
        return this.request(Object.assign({}, {
            method: 'get', url: url
        }, config));

    }

    delete(url, config) {
        return this.request(Object.assign({}, {
            method: 'delete', url: url
        }, config));

    }

    head(url, config) {
        return this.request(Object.assign({}, {
            method: 'head', url: url
        }, config));

    }

    options(url, config) {
        return this.request(Object.assign({}, {
            method: 'options', url: url
        }, config));
    }

    post(url, data, config) {
        return this.request(Object.assign({}, {
            method: 'post', url: url, data: data
        }, config));
    }

    put(url, data, config) {
        return this.request(Object.assign({}, {
            method: 'put', url: url, data: data
        }, config));

    }

    patch(url, data, config) {
        return this.request(Object.assign({}, {
            method: 'patch', url: url, data: data
        }, config));
    }

    getUri(config) {
        return axios.getUri(config);
    }

    export(url, config) {
        return this.requestExport(Object.assign({}, {
            method: 'get', url: url
        }, config));
    }

    updateConfig(config, token, isOverwrite) {
        let access_token = '';
        if (token) {
            access_token = `Bearer ${token.access_token}`;
        }
        // chỉnh sửa lại request để add thêm token vào header, chỉnh sửa lại url của request
        config.headers = config.headers || {};
        const headers = config.headers;
        if (headers.Authorization && !isOverwrite) {
            access_token = headers.Authorization;
        }
        config.headers.Authorization = access_token;

        if (config.logAction) {
            // ===== ACTIVITY LOG HEADERS =====
            headers['X-Function-Id'] = SelectedFunctionStore.get();
            // ====== Action ===============
            headers['X-Log-Action'] = config.logAction;
        }

        return config;
    }

    handle400Error(error) {
        return throwError(error);
    }

    handle401Error(config, error) {
        if (!this.isRefreshingToken) {
            this.isRefreshingToken = true;
            // Reset here so that the following requests wait until the token
            // comes back from the refreshToken call.
            this.tokenSubject.next(null);
            return this.refreshToken().pipe(catchError(err => {
                console.log(err);
                // If there is an exception calling 'refreshToken', bad news so logout.
                if (err) {
                    this.logoutUser();
                }
                return throwError(error);
            }), switchMap((newToken) => {
                if (newToken) {
                    this.tokenSubject.next(newToken);
                    return HttpHandle(this.updateConfig(config, newToken, true));

                }
                // If we don't get a new token, we are in trouble so logout.
                this.logoutUser();
                return throwError(error);
            }), finalize(() => {
                this.isRefreshingToken = false;
            }));
        } else {
            return this.tokenSubject.pipe(filter(token => token != null), take(1), switchMap(token => {
                return HttpHandle(this.updateConfig(config, token, true));
            }));
        }
    }

    refreshToken() {
        if (this.oauth && this.oauth.refresh_token) {
            return authService.refreshToken(this.oauthConfig?.serverConfig?.token_endpoint, this.oauth.refresh_token, this.oauthConfig?.clientConfig?.client_id);
        } else {
            return new Observable(observable => {
                observable.next(false);
            });
        }
    }

    logoutUser() {
        useOauthStore?.getState().actions.setAuthenticated(false);
    }
}

