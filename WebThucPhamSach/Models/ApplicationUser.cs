using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebThucPhamSach.Models
{
    /// <summary>
    /// Lớp ApplicationUser mở rộng IdentityUser mặc định của ASP.NET Core Identity.
    /// Bổ sung thêm thông tin: Họ tên đầy đủ và Địa chỉ giao hàng.
    /// Dữ liệu được lưu trong bảng AspNetUsers của SQL Server.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        // Họ và tên đầy đủ của người dùng
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [Display(Name = "Họ và tên")]
        [StringLength(100, ErrorMessage = "Họ tên không vượt quá 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        // Địa chỉ giao hàng (tùy chọn)
        [Display(Name = "Địa chỉ")]
        [StringLength(300, ErrorMessage = "Địa chỉ không vượt quá 300 ký tự")]
        public string? Address { get; set; }
    }
}
