using System.ComponentModel.DataAnnotations;

namespace webthucphamsach.Models
{
    public class Category
    {
        [Display(Name = "Mã danh mục")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        // Thuộc tính điều hướng thiết lập quan hệ 1-nhiều
        public List<Product>? Products { get; set; }
    }
}
