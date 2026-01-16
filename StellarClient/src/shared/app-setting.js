export const API_URL = window.appCfg.API_URL;
// export const BASE_FILE_URL = window.appCfg.BASE_FILE_URL;

export const TIME_CHECK = window.appCfg.TIME_CHECK;
export const BASE_AS_URL=window.appCfg.BASE_AS_URL;
export const AS_URL=window.appCfg.AS_URL;
export const AS_CLIENT_SECRET=window.appCfg.AS_CLIENT_SECRET;
export const AS_CLIENT_ID=window.appCfg.AS_CLIENT_ID;
export const DASHBOARD_URL=window.appCfg.DASHBOARD_URL;
export const FILE_URL = `${API_URL}api/file-view/`;
export const FILE_MAX_SIZE=window.appCfg.FILE_MAX_SIZE||200;
export const FILE_ACCEPT_LIST=[
    '.pdf' 
    ,'.doc'
    ,'.docx'
    ,'.pps'
    ,'.ppsx'
    ,'.xls'
    ,'.xlsx'
    ,'image/*'
    ,'video/*'
    ,'audio/*'
    ,'.zip'
    ,'.rar'
    ,'application/msword'
    ,'application/x-compressed'
    ,'application/vnd.ms-excel'
];

export const CLIENT = {
    AS_URL: AS_URL,
    client_id: AS_CLIENT_ID,
    client_secret: AS_CLIENT_SECRET,
    scope:'openid profile email',
    response_type: 'code',
    redirect_uri:`${window.location.protocol}//${window.location.host}`,
    silent_redirect_uri:`${window.location.protocol}//${window.location.host}/silent`,
    logout_redirect_uri:`${window.location.protocol}//${window.location.host}/logout`,
    oauth_redirect_uri:`${window.location.protocol}//${window.location.host}/oauth`,
};
export const AUTHORIZATION_BASE = `Basic ${btoa(CLIENT.client_id + ':' + CLIENT.client_secret)}`;
