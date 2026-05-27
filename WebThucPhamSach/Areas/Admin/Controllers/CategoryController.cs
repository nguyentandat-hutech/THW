using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebThucPhamSach.Models;
using WebThucPhamSach.Repositories;

namespace WebThucPhamSach.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller quản lý Danh mục sản phẩm dành riêng cho Admin.
    /// CHỈ những tài khoản có Role "Admin" mới truy cập được.
    /// Đường dẫn: /Admin/Category/...
    /// Tính năng đặc biệt: Khi xóa danh mục sẽ XÓA CASCADE toàn bộ sản phẩm cùng danh mục.
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryController(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// GET: /Admin/Category — Danh sách tất cả danh mục, kèm số sản phẩm mỗi danh mục
        /// </summary>
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            // Đếm số sản phẩm theo từng danh mục để hiển thị
            var productCounts = _productRepository.GetAll()
                                .GroupBy(p => p.CategoryId)
                                .ToDictionary(g => g.Key, g => g.Count());
            ViewBag.ProductCounts = productCounts;
            return View(categories);
        }

        /// <summary>
        /// GET: /Admin/Category/Add — Form thêm danh mục mới
        /// </summary>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// POST: /Admin/Category/Add — Xử lý thêm danh mục mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Add(category);
                TempData["SuccessMessage"] = "Thêm danh mục thành công!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        /// <summary>
        /// GET: /Admin/Category/Update/1 — Form chỉnh sửa danh mục
        /// </summary>
        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        /// <summary>
        /// POST: /Admin/Category/Update — Xử lý cập nhật danh mục
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category);
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        /// <summary>
        /// GET: /Admin/Category/Delete/1 — Trang xác nhận xóa danh mục
        /// Hiển thị số sản phẩm sẽ bị xóa theo để người dùng nắm rõ hậu quả.
        /// </summary>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null) return NotFound();

            // Đếm số sản phẩm sẽ bị xóa kèm theo
            var productCount = _productRepository.GetAll()
                               .Count(p => p.CategoryId == id);
            ViewBag.ProductCount = productCount;

            return View(category);
        }

        /// <summary>
        /// POST: /Admin/Category/DeleteConfirmed — Xóa danh mục và CASCADE toàn bộ sản phẩm cùng danh mục.
        /// Thứ tự: Xóa tất cả sản phẩm → Xóa danh mục.
        /// </summary>
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Bước 1: Lấy tất cả sản phẩm thuộc danh mục này
            var productsToDelete = _productRepository.GetAll()
                                   .Where(p => p.CategoryId == id)
                                   .ToList();

            // Bước 2: Xóa từng sản phẩm trước (tránh lỗi khóa ngoại)
            foreach (var product in productsToDelete)
            {
                _productRepository.Delete(product.Id);
            }

            // Bước 3: Xóa danh mục sau khi đã xóa hết sản phẩm
            _categoryRepository.Delete(id);

            TempData["SuccessMessage"] = $"Đã xóa danh mục và {productsToDelete.Count} sản phẩm liên quan!";
            return RedirectToAction("Index");
        }
    }
}
