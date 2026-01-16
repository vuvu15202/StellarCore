import { useUserProfileStore } from '../stores/useUserProfileStore';

export const useUserProfile = () => {

    const {
        profile, groupId, nhom_nguoi_dung, dsNhomNguoiDung, menu, logo_id, theme, actions: {
            setPermissions, setMenu, setNhomNguoiDung, setDsNhomNguoiDung, setVaiTro, setToken, setGroupId
        }
    } = useUserProfileStore();

    return {
        profile,
        menu,
        theme,
        nhom_nguoi_dung,
        dsNhomNguoiDung,
        logo_id,
        groupId,
        actions: {
            setPermissions,
            setMenu,
            setNhomNguoiDung,
            setDsNhomNguoiDung,
            setVaiTro,
            setUserProfile: setToken,
            setGroupId
        }
    };
};