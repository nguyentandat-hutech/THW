using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webthucphamsach.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Đơn giá")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Thuộc tính điều hướng trỏ về Order
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        // Thuộc tính điều hướng trỏ về Product
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
