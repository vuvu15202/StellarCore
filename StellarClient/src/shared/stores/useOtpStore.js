import {create} from 'zustand';
import {createJSONStorage, persist} from 'zustand/middleware';

const initState = {
    userId: {
        privateKey: null,
        suite: null,
        secretKey: null,
        publicKey: null
    }
};

export const useOtpStore = create(persist((set, get) => ({
    ...initState, actions: {
        setData: (data) => {
            set((state) => ({
                ...state, ...data
            }));
        },
        setPrivateKey: (userId, payload) => {
            set((state) => ({
                [userId]: {...state[userId], privateKey: payload}
            }));
        },
        setPublicKey: (userId, payload) => {
            set((state) => ({
                [userId]: {...state[userId], publicKey: payload}
            }));
        },
        setSuite: (userId, payload) => {
            set((state) => ({
                [userId]: {...state[userId], suite: payload}
            }));
        },
        setSecretKey: (userId, payload) => {
            set((state) => ({
                [userId]: {...state[userId], secretKey: payload}
            }));
        },
        clearStorage: () => {
            set(initState);
        }
    }
}), {
    name: 'otp-storage', // name of the item in the storage (must be unique)
    storage: createJSONStorage(() => localStorage), // (optional) by default, 'localStorage' is used
    partialize: (state) => {

        const data = ({
            ...Object.assign(initState, state)
        });

        delete data.actions;

        return data;
    }
}));

