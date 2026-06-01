using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webthucphamsach.Models;
using webthucphamsach.Repositories;
using webthucphamsach.Utility;

namespace webthucphamsach.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // DANH SÁCH DANH MỤC
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();
            ViewBag.ProductCounts = products
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count());
            return View(categories);
        }

        // THÊM DANH MỤC - GET
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // THÊM DANH MỤC - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                TempData["SuccessMessage"] = $"Danh mục '{category.Name}' đã được thêm thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // SỬA DANH MỤC - GET
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // SỬA DANH MỤC - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                TempData["SuccessMessage"] = $"Danh mục '{category.Name}' đã được cập nhật!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // XÓA DANH MỤC - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();
            var products = await _productRepository.GetAllAsync();
            ViewBag.ProductCount = products.Count(p => p.CategoryId == id);
            return View(category);
        }

        // XÓA DANH MỤC - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _productRepository.GetAllAsync();
            if (products.Any(p => p.CategoryId == id))
            {
                TempData["ErrorMessage"] = "Không thể xóa danh mục này vì đang có sản phẩm thuộc danh mục!";
                return RedirectToAction(nameof(Index));
            }
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            await _categoryRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = $"Danh mục đã được xóa thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
