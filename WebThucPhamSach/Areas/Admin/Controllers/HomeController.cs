using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webthucphamsach.Repositories;
using webthucphamsach.Utility;

namespace webthucphamsach.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            ViewBag.TotalProducts = products.Count();
            ViewBag.TotalCategories = categories.Count();
            ViewBag.TotalRevenue = products.Sum(p => p.Price);
            ViewBag.AvgPrice = products.Any() ? products.Average(p => p.Price) : 0;
            ViewBag.RecentProducts = products.TakeLast(5).Reverse().ToList();
            ViewBag.CategoryStats = categories.Select(c => new
            {
                Name = c.Name,
                Count = products.Count(p => p.CategoryId == c.Id),
                Revenue = products.Where(p => p.CategoryId == c.Id).Sum(p => p.Price)
            }).ToList();

            return View();
        }
    }
}
