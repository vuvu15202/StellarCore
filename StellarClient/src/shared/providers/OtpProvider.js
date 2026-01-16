import {useEffect} from "react";
import forge from "node-forge";
import {useOtpStore} from "shared/stores/useOtpStore";
import {otpService} from "shared/services/OtpService";
import {useUserProfileStore} from "shared/stores/useUserProfileStore";
import CryptoJS from "crypto-js";
import {io} from "socket.io-client";

forge.options.usePureJavaScript = true;

// Make crypto libraries globally available
if (typeof window !== 'undefined') {
    window.CryptoJS = CryptoJS;
    // Ensure Web Crypto API is available
    if (window.crypto && window.crypto.subtle) {
        window.crypto.webCrypto = window.crypto;
    }
}

if (typeof global !== 'undefined') {
    global.CryptoJS = CryptoJS;
}

export const OtpProvider = ({children} = {}) => {
    // // Patch random bytes để dùng CryptoJS
    // forge.random.getBytesSync = function (count) {
    //     let result = "";
    //     // CryptoJS.lib.WordArray.random trả về wordArray với số byte random
    //     const wordArray = CryptoJS.lib.WordArray.random(count);
    //     const hex = wordArray.toString(CryptoJS.enc.Hex);
    //
    //     for (let i = 0; i < hex.length; i += 2) {
    //         const byte = parseInt(hex.substr(i, 2), 16);
    //         result += String.fromCharCode(byte);
    //     }
    //     return result;
    // };

    const {profile = {}, access_token} = useUserProfileStore();
    const {user_id} = profile || {};
    const {
        actions: {setSuite, setPrivateKey, setSecretKey, setPublicKey}
    } = useOtpStore();

    const {privateKey, suite, secretKey} = useOtpStore()[user_id] ?? {};

    useEffect(() => {
        if (user_id && !privateKey) {
            const keypair = forge.pki.rsa.generateKeyPair({bits: 2048});
            const publicKey = forge.pki.publicKeyToPem(keypair.publicKey);
            const privateKey = forge.pki.privateKeyToPem(keypair.privateKey);
            otpService.register({deviceId: "web", publicKey}).subscribe({
                next: (data) => {
                    console.log(data);
                    const jsonData = decode(privateKey, data);
                    setSecretKey(user_id, jsonData.secretKey);
                    setSuite(user_id, jsonData.suite);
                    setPrivateKey(user_id, privateKey);
                }
            })
        }
    }, [user_id]);

    function b64decode(b64) {
        return forge.util.decode64(b64); // trả về binary string
    }

    function utf8ToBytes(str) {
        return forge.util.encodeUtf8(str); // binary string (UTF-8)
    }


    function bytesToUtf8(bytes) {
        return forge.util.decodeUtf8(bytes);
    }

    function signChallenge(privatePem, challenge) {
        const privateKey = forge.pki.privateKeyFromPem(privatePem);

        const md = forge.md.sha256.create();
        md.update(challenge, 'utf8');

        const signatureBytes = privateKey.sign(md); // raw bytes
        return forge.util.encode64(signatureBytes); // base64 để dễ truyền
    }

    function generateNonceSimple(length = 16) {
        let result = "";
        const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        for (let i = 0; i < length; i++) {
            result += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        return result;
    }

    useEffect(() => {

        if (suite && secretKey) {
            const now = Date.now();
            const nonce = generateNonceSimple(16);
            const challenge = `${nonce}|${now}`;

            const socket = io(`${API_URL}queue/otps.generate`, {
                query: {
                    token: access_token
                },
                transports: ["websocket"],
                path: "/otp-service/ws",
                reconnection: true,            // bật chế độ reconnect
                reconnectionAttempts: Infinity, // số lần thử reconnect (∞ = không giới hạn)
                reconnectionDelay: 10000,        // thời gian chờ giữa mỗi lần reconnect (ms)
                reconnectionDelayMax: 10000      // max delay (đảm bảo luôn 5s)
            }); // đổi URL thành server của bạn
            // Lắng nghe sự kiện từ server
            // socket.emit("subscribe", {
            //     "x-signature": signChallenge(privateKey, challenge),
            //     "x-challenge": challenge
            // })
            socket.on('message', (msg) => {
                // console.log(msg);
                // const data = decode(privateKey, msg);
                // const sessionId = data?.sessionId;
                // const timestamp = Math.floor(Date.now() / data.stepTime);
                // OCRA.generateOCRA("OCRA-1:HOTP-SHA256-6:QA08-PSHA256-S128-T5M", secretKey, 0, data, password, sessionId, timestamp).then(otp => {
                //     beh.success(`Otp của bạn là: ${otp}`)
                // })
            });

            // Cleanup khi component unmount
            return () => {
                socket.off('chat message');
            };
        }
    }, [suite, secretKey]);

    function decode(privateKey, msg) {
        const encodedMessage = msg;
        const privateKeyObj = forge.pki.privateKeyFromPem(privateKey);
        const decryptedBytes = privateKeyObj.decrypt(b64decode(encodedMessage), 'RSA-OAEP', {
            md: forge.md.sha256.create(),
        });
        const plainText = bytesToUtf8(decryptedBytes);
        return JSON.parse(plainText);
    }

    // eslint-disable-next-line react/react-in-jsx-scope
    return (<>{children}</>);
}