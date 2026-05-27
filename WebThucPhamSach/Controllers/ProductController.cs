using Microsoft.AspNetCore.Mvc;
using WebThucPhamSach.Models;
using WebThucPhamSach.Repositories;

namespace WebThucPhamSach.Controllers
{
    /// <summary>
    /// Controller công khai — Cho phép tất cả mọi người (kể cả chưa đăng nhập):
    ///   - Xem danh sách sản phẩm (có thể lọc theo danh mục)
    ///   - Xem chi tiết một sản phẩm
    /// Các chức năng CRUD (Thêm/Sửa/Xóa) đã chuyển sang Area Admin.
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// GET: /Product hoặc /Product?categoryId=1
        /// Hiển thị danh sách sản phẩm, có hỗ trợ lọc theo danh mục.
        /// </summary>
        public IActionResult Index(int? categoryId)
        {
            // Lấy danh sách tất cả danh mục để hiển thị bộ lọc
            var categories = _categoryRepository.GetAll();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;

            // Lọc sản phẩm theo danh mục nếu có tham số categoryId
            var products = _productRepository.GetAll();
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
                var selectedCat = _categoryRepository.GetById(categoryId.Value);
                ViewBag.SelectedCategoryName = selectedCat?.Name;
            }

            return View(products);
        }

        /// <summary>
        /// GET: /Product/Display/5
        /// Hiển thị chi tiết một sản phẩm theo Id.
        /// </summary>
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                return NotFound();

            var category = _categoryRepository.GetById(product.CategoryId);
            ViewBag.CategoryName = category?.Name ?? "Chưa phân loại";

            return View(product);
        }
    }
}
