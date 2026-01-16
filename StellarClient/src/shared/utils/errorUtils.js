// utils/errorUtils.js

/**
 * Trả về thông báo lỗi từ đối tượng lỗi, nếu không có thì dùng fallback.
 *
 * @param {Object} err - Đối tượng lỗi có thể chứa nhiều cấp độ thông báo.
 * @param {string} fallback - Thông báo mặc định nếu không tìm thấy thông báo lỗi cụ thể.
 * @returns {string} - Thông báo lỗi phù hợp.
 */
export const getErrorMessage = (err, fallback) => {
    return (
        err?.error?.errors?.[0]?.message ||
        err?.error?.message ||
        err?.message ||
        fallback
    );
};
