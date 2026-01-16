// httpStatus.config.js
export const HTTP_STATUS_CONFIG = {
    OK: {
        code: 200,
        label: 'OK',
        color: '#91CC75',
    },
    MOVED_PERMANENTLY: {
        code: 301,
        label: 'Redirect',
        color: '#5470C6',
    },
    BAD_REQUEST: {
        code: 400,
        label: 'Bad Request',
        color: '#3D3D5C',
    },
    UNAUTHORIZED: {
        code: 401,
        label: 'Unauthorized',
        color: '#FC8452',
    },
    FORBIDDEN: {
        code: 403,
        label: 'Forbidden',
        color: '#73C0DE',
    },
    NOT_FOUND: {
        code: 404,
        label: 'Not Found',
        color: '#FAC858',
    },
    INTERNAL_SERVER_ERROR: {
        code: 500,
        label: 'Server Error',
        color: '#EE6666',
    },
};
