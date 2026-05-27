using System.ComponentModel.DataAnnotations;

namespace WebThucPhamSach.Models
{
    /// <summary>
    /// ViewModel cho form Đăng ký tài khoản mới.
    /// Thu thập đầy đủ thông tin: tên đăng nhập, email, họ tên, địa chỉ và mật khẩu.
    /// </summary>
    public class RegisterViewModel
    {
        // Tên đăng nhập (Username)
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập từ 3 đến 50 ký tự")]
        public string UserName { get; set; } = string.Empty;

        // Địa chỉ Email
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        // Họ và tên đầy đủ
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [Display(Name = "Họ và tên")]
        [StringLength(100, ErrorMessage = "Họ tên không vượt quá 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        // Địa chỉ giao hàng (không bắt buộc)
        [Display(Name = "Địa chỉ")]
        [StringLength(300, ErrorMessage = "Địa chỉ không vượt quá 300 ký tự")]
        public string? Address { get; set; }

        // Mật khẩu
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        // Xác nhận mật khẩu — phải khớp với Password
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
