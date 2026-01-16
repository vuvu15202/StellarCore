import {create} from 'zustand';
import {createJSONStorage, persist} from 'zustand/middleware';

const initState = {
    isLoading: false, isAuthenticated: false, authorizeSession: {
        code_verifier: '', code_challenge: '', code: '', state: ''
    }
};

export const useOauthStore = create(persist((set) => ({
    ...initState
    , actions: {
        setData: (data) => {
            set(() => {
                return {data};
            });
        }, setLoading: (isLoading) => {
            set({isLoading});
        }, setAuthenticated: (isAuthenticated) => {
            set({isAuthenticated});
        }, setAuthorizeSession: (authorizeSession) => {
            set({authorizeSession});
        }, setCodeChallenge: (code_challenge, code_verifier) => {
            set((state) => ({authorizeSession: {...state?.authorizeSession, code_challenge, code_verifier}}));
        }, setSessionState: (authorizeState, code) => {
            set(state => ({authorizeSession: {...state?.authorizeSession, state: authorizeState, code}}));
        }, clearStorage: () => {
            set(initState);
        }
    }
}), {
    name: 'oauth-storage', // name of the item in the storage (must be unique)
    storage: createJSONStorage(() => localStorage), // (optional) by default, 'localStorage' is used
    partialize: (state) => ({
        isAuthenticated: state.isAuthenticated,
        authorizeSession: state.authorizeSession,
    })
}));

