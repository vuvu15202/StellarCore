import {http} from 'shared/utils';

class OtpService {

    constructor() {
    }

    register({deviceId = "web", publicKey}) {
        return http.post(`/otp-service/otp-registrations`, {deviceId, publicKey});
    }
}

const otpService = new OtpService();
export {otpService, OtpService};
