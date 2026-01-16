
import { random as cryptoRandom } from 'crypto-js/lib-typedarrays';
import SHA256 from 'crypto-js/sha256';
import Base64 from 'crypto-js/enc-base64';
/**
 * Creates an array of length `size` of random bytes
 * @param size
 * @returns Array of random ints (0 to 255)
 */
function getRandomValues(size) {
    const randomWords = cryptoRandom(size);
    return randomWords.words
        .flatMap((word) => [
            (word >> 24) & 0xff,
            (word >> 16) & 0xff,
            (word >> 8) & 0xff,
            word & 0xff,
        ])
        .slice(0, size);

}
/**
 * Creates an array of length `size` of random bytes
 * @param wordArray
 * @returns base64url encoded string
 * @description This function encodes a WordArray to a base64url string.
 */
function base64UrlEncodeWordArray(wordArray) {
    return Base64.stringify(wordArray)
        .replace(/\+/g, '-')
        .replace(/\//g, '_')
        .replace(/=+$/, '');
}
/** Generate cryptographically strong random string
 * @param size The desired length of the string
 * @returns The random string
 */
function random(size) {
    const mask =
        'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~';
    let result = '';
    const randomUints = getRandomValues(size);
    for (let i = 0; i < size; i++) {
        // cap the value of the randomIndex to mask.length - 1
        const randomIndex = randomUints[i] % mask.length;
        result += mask[randomIndex];
    }
    return result;
}

/** Generate a PKCE challenge verifier
 * @param length Length of the verifier
 * @returns A random verifier `length` characters long
 */
function generateVerifier(length) {
    return random(length);
}

/** Generate a PKCE code challenge from a code verifier
 * @param code_verifier
 * @returns The base64 url encoded code challenge
 */
export function generateChallenge(code_verifier) {
    if (typeof code_verifier !== 'string') {
        throw 'code_verifier must be a string';
    }
    if (code_verifier.length < 43 || code_verifier.length > 128) {
        throw `Expected a code_verifier length between 43 and 128. Received ${code_verifier.length}.`;
    }
    const hash = SHA256(code_verifier);
    return base64UrlEncodeWordArray(hash);
}

/** Generate a PKCE challenge pair
 * @param length Length of the verifer (between 43-128). Defaults to 43.
 * @returns PKCE challenge pair
 */
export  function pkceChallenge(length) {
    if (!length) length = 43;

    if (length < 43 || length > 128) {
        throw `Expected a length between 43 and 128. Received ${length}.`;
    }

    const verifier = generateVerifier(length);
    const challenge = generateChallenge(verifier);

    return {
        code_verifier: verifier,
        code_challenge: challenge,
    };
}

/** Verify that a code_verifier produces the expected code challenge
 * @param code_verifier
 * @param expectedChallenge The code challenge to verify
 * @returns True if challenges are equal. False otherwise.
 */
export function verifyChallenge(
    code_verifier,
    expectedChallenge
) {
    const actualChallenge =  generateChallenge(code_verifier);
    return actualChallenge === expectedChallenge;
}