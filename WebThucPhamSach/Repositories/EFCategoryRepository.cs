using WebThucPhamSach.Data;
using WebThucPhamSach.Models;

namespace WebThucPhamSach.Repositories
{
    /// <summary>
    /// Triển khai ICategoryRepository bằng Entity Framework Core.
    /// Thay thế MockCategoryRepository, truy vấn bảng Categories từ SQL Server.
    /// Được đăng ký dưới dạng Scoped Service trong Program.cs.
    /// </summary>
    public class EFCategoryRepository : ICategoryRepository
    {
        // ApplicationDbContext được tiêm vào thông qua Constructor Injection
        private readonly ApplicationDbContext _context;

        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách danh mục từ database, sắp xếp theo tên.
        /// </summary>
        public IEnumerable<Category> GetAll()
        {
            // SELECT * FROM Categories ORDER BY Name
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }

        /// <summary>
        /// Lấy thông tin danh mục theo mã Id.
        /// Trả về null nếu không tìm thấy.
        /// </summary>
        public Category? GetById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Thêm một danh mục mới vào database.
        /// </summary>
        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        /// <summary>
        /// Cập nhật thông tin danh mục trong database.
        /// </summary>
        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        /// <summary>
        /// Xóa danh mục theo Id. Không làm gì nếu không tìm thấy.
        /// </summary>
        public void Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
