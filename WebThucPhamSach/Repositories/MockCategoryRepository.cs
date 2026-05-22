namespace WebThucPhamSach.Repositories
{
    using WebThucPhamSach.Models;

    /// <summary>
    /// Lớp giả lập (Mock) dữ liệu danh mục sản phẩm trong bộ nhớ.
    /// Chứa sẵn 3 danh mục: Rau củ, Trái cây, Thịt tươi.
    /// </summary>
    public class MockCategoryRepository : ICategoryRepository
    {
        // Danh sách danh mục lưu trong bộ nhớ
        private readonly List<Category> _categories;

        /// <summary>
        /// Khởi tạo dữ liệu mẫu gồm 3 danh mục thực phẩm sạch.
        /// </summary>
        public MockCategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category { Id = 1, Name = "Rau củ" },
                new Category { Id = 2, Name = "Trái cây" },
                new Category { Id = 3, Name = "Thịt tươi" }
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách danh mục.
        /// </summary>
        public IEnumerable<Category> GetAll()
        {
            return _categories;
        }

        /// <summary>
        /// Tìm danh mục theo mã Id. Trả về null nếu không tìm thấy.
        /// </summary>
        public Category? GetById(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Thêm mới danh mục vào danh sách. Tự động tăng Id.
        /// </summary>
        public void Add(Category category)
        {
            category.Id = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1;
            _categories.Add(category);
        }

        /// <summary>
        /// Cập nhật thông tin danh mục đã tồn tại.
        /// </summary>
        public void Update(Category category)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
            }
        }

        /// <summary>
        /// Xóa danh mục khỏi danh sách theo mã Id.
        /// </summary>
        public void Delete(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _categories.Remove(category);
            }
        }
    }
}
