namespace webthucphamsach.Utility
{
    /// <summary>
    /// Static Detail (SD) - Lớp tiện ích chứa các hằng số dùng chung toàn ứng dụng.
    /// Đặc biệt là các hằng số định nghĩa tên Vai trò (Role) trong hệ thống phân quyền.
    /// </summary>
    public static class SD
    {
        // ============================================================
        // HẰNG SỐ PHÂN QUYỀN (ROLE CONSTANTS)
        // ============================================================

        /// <summary>
        /// Vai trò Khách hàng - Có quyền xem sản phẩm và đặt hàng.
        /// </summary>
        public const string Role_Customer = "Customer";

        /// <summary>
        /// Vai trò Quản trị viên - Có toàn quyền quản lý hệ thống.
        /// </summary>
        public const string Role_Admin = "Admin";

        /// <summary>
        /// Vai trò Nhân viên - Có quyền quản lý đơn hàng và sản phẩm.
        /// </summary>
        public const string Role_Employee = "Employee";
    }
}
