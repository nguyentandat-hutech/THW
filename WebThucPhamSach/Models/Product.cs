using System.ComponentModel.DataAnnotations;

namespace webthucphamsach.Models
{
    public class Product
    {
        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá bán không được để trống")]
        [Range(1000, 10000000, ErrorMessage = "Giá bán phải nằm trong khoảng từ 1.000 đ đến 10.000.000 đ")]
        [Display(Name = "Giá bán (đ)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống")]
        [StringLength(500, ErrorMessage = "Mô tả sản phẩm không được vượt quá 500 ký tự")]
        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn danh mục sản phẩm")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        // Thuộc tính điều hướng thiết lập khóa ngoại
        public Category? Category { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; }
    }
}
