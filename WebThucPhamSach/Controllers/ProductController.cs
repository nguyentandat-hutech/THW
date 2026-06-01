using Microsoft.AspNetCore.Mvc;
using webthucphamsach.Models;
using webthucphamsach.Repositories;

namespace webthucphamsach.Controllers
{
    /// <summary>
    /// Controller sản phẩm dành cho KHÁCH HÀNG.
    /// Chỉ có chức năng xem danh sách và xem chi tiết sản phẩm.
    /// Các chức năng CRUD (thêm/sửa/xóa) được chuyển sang Areas/Admin/Controllers/ProductController.cs
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // HIỂN THỊ DANH SÁCH SẢN PHẨM (INDEX) - Dành cho tất cả người dùng
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            var categories = (await _categoryRepository.GetAllAsync())
                .ToDictionary(c => c.Id, c => c.Name);
            ViewBag.Categories = categories;
            return View(products);
        }

        // HIỂN THỊ CHI TIẾT SẢN PHẨM (DISPLAY) - Dành cho tất cả người dùng
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
            ViewBag.CategoryName = category != null ? category.Name : "Không xác định";
            return View(product);
        }
    }
}
