using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebThucPhamSach.Models
{
    /// <summary>
    /// Model đại diện cho một Đơn hàng trong cơ sở dữ liệu.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        // Khóa ngoại liên kết tới AspNetUsers mở rộng
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [Required]
        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(500, ErrorMessage = "Địa chỉ nhận hàng tối đa 500 ký tự")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; } = null!;

        [Required]
        [MaxLength(20, ErrorMessage = "Số điện thoại tối đa 20 ký tự")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại nhận hàng")]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(1000, ErrorMessage = "Ghi chú tối đa 1000 ký tự")]
        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; }

        [Required]
        [Display(Name = "Tổng số tiền")]
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Chờ duyệt"; // Chờ duyệt, Đã duyệt, Đang giao, Đã giao, Đã hủy

        // Một đơn hàng có nhiều chi tiết đơn hàng
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
