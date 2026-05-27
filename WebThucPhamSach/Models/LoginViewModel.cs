using System.ComponentModel.DataAnnotations;

namespace WebThucPhamSach.Models
{
    /// <summary>
    /// ViewModel cho form Đăng nhập.
    /// Người dùng đăng nhập bằng Email và Mật khẩu.
    /// </summary>
    public class LoginViewModel
    {
        // Địa chỉ Email dùng để đăng nhập
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        // Mật khẩu
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        // Tùy chọn "Nhớ đăng nhập" — lưu cookie lâu hơn
        [Display(Name = "Nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}
