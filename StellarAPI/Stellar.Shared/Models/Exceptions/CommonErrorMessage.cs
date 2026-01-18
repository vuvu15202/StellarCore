namespace Stellar.Shared.Models.Exceptions
{
    public static class CommonErrorMessage
    {
        public const string FORBIDDEN = "Bạn không có quyền truy cập";
        public const string BAD_REQUEST = "Dữ liệu không hợp lệ";
        public const string VALIDATION_FAILED = "Xác minh dữ liệu thất bại";
        public const string FIELD_CANT_SORT = "Trường #fieldname# không thể sắp xếp";
        public const string INTERNAL_SERVER = "Hệ thống có lỗi xảy ra xin vui lòng thử lại sau";
        public const string ENUM_FAILED = "Không thể chuyển đổi #value# thành loại #type#";
        public const string NOT_FOUND = "Không tìm thấy dữ liệu";
        public const string FEIGN_ERROR = "Giao tiếp service qua Feign gặp sự cố";
        public const string INIT_RESOURCE_FEATURE_ERROR = "Khởi tạo resource feature bị lỗi";
        public const string NO_RESOURCE_NOT_FOUND = "Đường dẫn truy cập tài nguyên không tồn tại";
        public const string MISSING_ANNOTATION = "Annotation @RolesAllowed không được cấu hình đúng";
        public const string UNAUTHORIZED_MISSING_HEADER = "Thiếu header X-User trong request";
        public const string REQUEST_CONTEXT_NOT_FOUND = "Không thể lấy thông tin request hiện tại";
        public const string FROM_TABLE_IS_REQUIRED = "From table is required";
        public const string OBJECT_NOT_FOUND = "Đối tượng không tồn tại";
        public const string DATABASE_NOT_CONNECTED = "Database chưa được kết nối";
        public const string INVALID_HEADER_FORMAT = "Định dạng X-User header không hợp lệ";
        public const string INVALID_JSON = "#string# không phải chuỗi JSON";
    }
}
