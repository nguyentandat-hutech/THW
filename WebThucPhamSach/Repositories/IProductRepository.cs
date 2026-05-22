namespace WebThucPhamSach.Repositories
{
    using WebThucPhamSach.Models;

    /// <summary>
    /// Interface định nghĩa các phương thức CRUD cho Sản phẩm.
    /// Sử dụng Repository Pattern để tách biệt logic truy cập dữ liệu.
    /// </summary>
    public interface IProductRepository
    {
        // Lấy danh sách tất cả sản phẩm
        IEnumerable<Product> GetAll();

        // Lấy thông tin sản phẩm theo mã Id
        Product? GetById(int id);

        // Thêm mới một sản phẩm
        void Add(Product product);

        // Cập nhật thông tin sản phẩm
        void Update(Product product);

        // Xóa sản phẩm theo mã Id
        void Delete(int id);
    }
}
