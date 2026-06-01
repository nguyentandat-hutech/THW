using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using webthucphamsach.Models;
using webthucphamsach.Repositories;
using webthucphamsach.Utility;

namespace webthucphamsach.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller quản lý sản phẩm trong Area Admin.
    /// Chỉ người dùng có quyền Admin mới được truy cập.
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // ============================================================
        // 1. DANH SÁCH SẢN PHẨM (INDEX) - Chỉ Admin
        // ============================================================
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            var categories = (await _categoryRepository.GetAllAsync())
                .ToDictionary(c => c.Id, c => c.Name);
            ViewBag.Categories = categories;
            return View(products);
        }

        // ============================================================
        // 2. THÊM MỚI SẢN PHẨM - GET
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // ============================================================
        // 3. THÊM MỚI SẢN PHẨM - POST
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = "/images/" + uniqueFileName;
                }
                else
                {
                    product.ImageUrl = "/images/default-food.jpg";
                }

                await _productRepository.AddAsync(product);
                TempData["SuccessMessage"] = $"Sản phẩm '{product.Name}' đã được thêm thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // ============================================================
        // 4. CHỈNH SỬA SẢN PHẨM - GET
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // ============================================================
        // 5. CHỈNH SỬA SẢN PHẨM - POST
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(product.Id);
                if (existingProduct == null) return NotFound();

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }
                    existingProduct.ImageUrl = "/images/" + uniqueFileName;
                }

                await _productRepository.UpdateAsync(existingProduct);
                TempData["SuccessMessage"] = $"Sản phẩm '{existingProduct.Name}' đã được cập nhật!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // ============================================================
        // 6. XÓA SẢN PHẨM - GET (Xác nhận)
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
            ViewBag.CategoryName = category?.Name ?? "Không xác định";
            return View(product);
        }

        // ============================================================
        // 7. XÓA SẢN PHẨM - POST (Thực thi)
        // ============================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            string productName = product.Name;
            await _productRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = $"Sản phẩm '{productName}' đã được xóa thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
