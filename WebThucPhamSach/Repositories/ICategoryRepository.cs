namespace WebThucPhamSach.Repositories
{
    using WebThucPhamSach.Models;

    /// <summary>
    /// Interface định nghĩa các phương thức CRUD cho Danh mục.
    /// Sử dụng Repository Pattern để tách biệt logic truy cập dữ liệu.
    /// </summary>
    public interface ICategoryRepository
    {
        // Lấy danh sách tất cả danh mục
        IEnumerable<Category> GetAll();

        // Lấy thông tin danh mục theo mã Id
        Category? GetById(int id);

        // Thêm mới một danh mục
        void Add(Category category);

        // Cập nhật thông tin danh mục
        void Update(Category category);

        // Xóa danh mục theo mã Id
        void Delete(int id);
    }
}
