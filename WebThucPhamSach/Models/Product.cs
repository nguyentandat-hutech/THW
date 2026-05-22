using System.ComponentModel.DataAnnotations;

namespace WebThucPhamSach.Models
{
    /// <summary>
    /// Lớp Model đại diện cho một Sản phẩm thực phẩm sạch.
    /// Chứa các thuộc tính mô tả sản phẩm cùng Data Annotations để validate dữ liệu.
    /// </summary>
    public class Product
    {
        // Mã sản phẩm - Khóa chính, tự động tăng
        public int Id { get; set; }

        // Tên sản phẩm - Bắt buộc nhập
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        // Giá sản phẩm - Bắt buộc, giới hạn từ 1,000 đến 10,000,000 VNĐ
        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        [Range(1000, 10000000, ErrorMessage = "Giá phải từ 1,000 đến 10,000,000 VNĐ")]
        [Display(Name = "Giá (VNĐ)")]
        public decimal Price { get; set; }

        // Mô tả sản phẩm (nguồn gốc, chứng nhận hữu cơ,...)
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        // Mã danh mục sản phẩm thuộc về
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        // Đường dẫn file ảnh sản phẩm (lưu tên file trong wwwroot/images/)
        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; }
    }
}
