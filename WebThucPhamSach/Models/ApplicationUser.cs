using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace webthucphamsach.Models
{
    /// <summary>
    /// Lớp người dùng mở rộng từ IdentityUser, bổ sung thông tin khách hàng mua thực phẩm sạch.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Họ và tên đầy đủ của khách hàng (bắt buộc).
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập họ và tên đầy đủ.")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự.")]
        [Display(Name = "Họ và Tên")]
        public string? FullName { get; set; }

        /// <summary>
        /// Địa chỉ giao hàng của khách hàng.
        /// </summary>
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự.")]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        /// <summary>
        /// Tuổi của khách hàng.
        /// </summary>
        [Range(1, 120, ErrorMessage = "Tuổi phải nằm trong khoảng từ 1 đến 120.")]
        [Display(Name = "Tuổi")]
        public int? Age { get; set; }
    }
}
