using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebThucPhamSach.Models;
using WebThucPhamSach.Repositories;

namespace WebThucPhamSach.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller quản lý Sản phẩm dành riêng cho Admin.
    /// CHỈ những tài khoản có Role "Admin" mới truy cập được.
    /// Đường dẫn: /Admin/Product/...
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _environment;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment environment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _environment = environment;
        }

        /// <summary>
        /// GET: /Admin/Product — Danh sách toàn bộ sản phẩm
        /// </summary>
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            // Tạo dictionary CategoryId -> CategoryName để hiển thị tên danh mục
            var categories = _categoryRepository.GetAll()
                             .ToDictionary(c => c.Id, c => c.Name);
            ViewBag.Categories = categories;
            return View(products);
        }

        /// <summary>
        /// GET: /Admin/Product/Add — Form thêm sản phẩm mới
        /// </summary>
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        /// <summary>
        /// POST: /Admin/Product/Add — Xử lý thêm sản phẩm, hỗ trợ upload ảnh
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                    product.ImageUrl = SaveImage(imageFile);

                _productRepository.Add(product);
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// GET: /Admin/Product/Update/5 — Form chỉnh sửa sản phẩm
        /// </summary>
        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// POST: /Admin/Product/Update — Xử lý cập nhật sản phẩm
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    product.ImageUrl = SaveImage(imageFile);
                }
                else
                {
                    // Giữ lại ảnh cũ nếu không upload ảnh mới
                    var existing = _productRepository.GetById(product.Id);
                    if (existing != null)
                        product.ImageUrl = existing.ImageUrl;
                }

                _productRepository.Update(product);
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// GET: /Admin/Product/Delete/5 — Trang xác nhận xóa
        /// </summary>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();

            var category = _categoryRepository.GetById(product.CategoryId);
            ViewBag.CategoryName = category?.Name ?? "Chưa phân loại";
            return View(product);
        }

        /// <summary>
        /// POST: /Admin/Product/DeleteConfirmed — Thực hiện xóa sản phẩm
        /// </summary>
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        // --- Helper: Lưu file ảnh vào wwwroot/images/ ---
        private string SaveImage(IFormFile imageFile)
        {
            var imagesFolder = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(imagesFolder, uniqueFileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            imageFile.CopyTo(fileStream);

            return uniqueFileName;
        }
    }
}
