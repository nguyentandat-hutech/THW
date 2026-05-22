using System.ComponentModel.DataAnnotations;

namespace WebThucPhamSach.Models
{
    /// <summary>
    /// Lớp Model đại diện cho Danh mục sản phẩm (Rau củ, Trái cây, Thịt tươi,...).
    /// </summary>
    public class Category
    {
        // Mã danh mục - Khóa chính
        public int Id { get; set; }

        // Tên danh mục - Bắt buộc nhập
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;
    }
}
