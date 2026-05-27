using System.ComponentModel.DataAnnotations;

namespace WebThucPhamSach.Models
{
    /// <summary>
    /// Model đại diện cho chi tiết của một sản phẩm trong đơn hàng.
    /// </summary>
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        [Required]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Giá tại thời điểm mua")]
        public decimal Price { get; set; }
    }
}
