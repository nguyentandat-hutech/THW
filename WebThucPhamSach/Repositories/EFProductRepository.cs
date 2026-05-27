using Microsoft.EntityFrameworkCore;
using WebThucPhamSach.Data;
using WebThucPhamSach.Models;

namespace WebThucPhamSach.Repositories
{
    /// <summary>
    /// Triển khai IProductRepository bằng Entity Framework Core.
    /// Thay thế hoàn toàn MockProductRepository, tương tác trực tiếp với SQL Server.
    /// Được đăng ký dưới dạng Scoped Service trong Program.cs.
    /// </summary>
    public class EFProductRepository : IProductRepository
    {
        // ApplicationDbContext được tiêm vào thông qua Constructor Injection
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sản phẩm từ database, sắp xếp theo Id.
        /// </summary>
        public IEnumerable<Product> GetAll()
        {
            // ToList() thực thi truy vấn SQL: SELECT * FROM Products ORDER BY Id
            return _context.Products.OrderBy(p => p.Id).ToList();
        }

        /// <summary>
        /// Lấy thông tin một sản phẩm theo mã Id.
        /// Trả về null nếu không tìm thấy.
        /// </summary>
        public Product? GetById(int id)
        {
            // FirstOrDefault sinh ra: SELECT TOP(1) * FROM Products WHERE Id = @id
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Thêm một sản phẩm mới vào database.
        /// </summary>
        public void Add(Product product)
        {
            _context.Products.Add(product);       // Đánh dấu entity là Added
            _context.SaveChanges();               // INSERT INTO Products ... thực thi ở đây
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm đã tồn tại trong database.
        /// </summary>
        public void Update(Product product)
        {
            _context.Products.Update(product);    // Đánh dấu entity là Modified
            _context.SaveChanges();               // UPDATE Products SET ... WHERE Id = @id
        }

        /// <summary>
        /// Xóa sản phẩm theo Id. Không làm gì nếu không tìm thấy sản phẩm.
        /// </summary>
        public void Delete(int id)
        {
            // Tìm sản phẩm trước khi xóa
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product); // Đánh dấu entity là Deleted
                _context.SaveChanges();            // DELETE FROM Products WHERE Id = @id
            }
        }
    }
}
