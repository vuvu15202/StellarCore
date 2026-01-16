import {create} from 'zustand';
import {createJSONStorage, persist} from 'zustand/middleware';
import {jwtDecode} from 'jwt-decode';

const initState = {
    id_token: null,
    access_token: null,
    expires_in: null,
    token_type: null,
    refresh_token: null,
    profile: null,
    permissions: [],
    menu: [],
    theme: {},
    session_state: null,
    nhom_nguoi_dung: null,
    dsNhomNguoiDung: [],
    groupId: null,
    verify: false,
    otp: {
        privateKey: null,
        suite: null,
        secretKey: null
    },
    vai_tro: {
        don_vi_quan_ly_id: null, ban_khoa_id: null, ly_lich_id: null, don_vi_id: null, ma_vai_tro: null
    }
};

export const useUserProfileStore = create(persist((set, getState) => ({
    ...initState, actions: {
        setData: (data) => {
            set((state) => ({
                ...state, ...data
            }));
        },
        setToken: (profile) => {
            set({...processToken(profile)});
        }, clearStorage: () => {
            set(initState);
        },
        setPermissions: (permissions) => {
            set({permissions});
        },
        setMenu: (menu) => {
            set({menu});
        },
        setNhomNguoiDung: (nhom_nguoi_dung) => {
            set({nhom_nguoi_dung});
        },
        setDsNhomNguoiDung: (dsNhomNguoiDung) => {
            set({dsNhomNguoiDung});
        },
        setGroupId: (groupId) => {
            set({groupId});
        },
        setVaiTro: (vai_tro) => {
            set({vai_tro});
        },
        setOtpPrivateKey: (privateKey) => {
            set((state) => ({otp: {...state.otp, privateKey}}))
        },
        setOtpSuite: (suite) => {
            set((state) => ({otp: {...state.otp, suite}}));
        },
        setOtpSecretKey: (payload) => {
            set((state) => ({otp: {...state.otp, payload}}));
        }
    }
}), {
    name: 'user-profile-storage', // name of the item in the storage (must be unique)
    storage: createJSONStorage(() => localStorage), // (optional) by default, 'localStorage' is used
    partialize: (state) => {

        const data = ({
            ...Object.assign(initState, state)
        });

        delete data.actions;

        return data;
    }
}));

function processToken(token) {
    const profile = jwtDecode(token.access_token);
    let theme = {};

    try {
        theme = JSON.parse(profile.theme);
    } catch (err) {
        // empty
    }

    //let vai_tro = getVaiTroNguoiDung(profiles, token.permissions);
    let result = {
        ...token,
        profile: {...profile},
        logo_id: token.logo || '',
        theme: theme,
    };
    result = Object.assign({}, initState, result);
    return result;
}

