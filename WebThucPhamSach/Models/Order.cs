using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webthucphamsach.Models
{
    public class Order
    {
        [Key]
        [Display(Name = "Mã Đơn Hàng")]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Ngày Đặt")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Tổng Tiền")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng.")]
        [StringLength(250, ErrorMessage = "Địa chỉ giao hàng không vượt quá 250 ký tự.")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Ghi chú không vượt quá 500 ký tự.")]
        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; }

        // Thuộc tính điều hướng liên kết với ApplicationUser
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }

        // Quan hệ 1-nhiều với OrderDetail
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
