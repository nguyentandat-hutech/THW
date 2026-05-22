using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebThucPhamSach.Models;
using WebThucPhamSach.Repositories;

namespace WebThucPhamSach.Controllers
{
    /// <summary>
    /// Controller xử lý tất cả các thao tác liên quan đến Sản phẩm:
    /// Danh sách, Chi tiết, Thêm mới, Cập nhật, Xóa.
    /// Sử dụng Dependency Injection để nhận Repository từ DI Container.
    /// </summary>
    public class ProductController : Controller
    {
        // Repository sản phẩm - được inject qua constructor
        private readonly IProductRepository _productRepository;
        // Repository danh mục - dùng để hiển thị dropdown danh mục trong form
        private readonly ICategoryRepository _categoryRepository;
        // Đối tượng môi trường web - dùng để lấy đường dẫn thư mục wwwroot
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Constructor: Inject các dependency cần thiết thông qua DI Container.
        /// </summary>
        /// <param name="productRepository">Repository quản lý sản phẩm</param>
        /// <param name="categoryRepository">Repository quản lý danh mục</param>
        /// <param name="environment">Thông tin môi trường web (để truy cập wwwroot)</param>
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
        /// Action Index: Hiển thị danh sách tất cả sản phẩm.
        /// GET: /Product hoặc /Product/Index
        /// </summary>
        public IActionResult Index()
        {
            // Lấy toàn bộ sản phẩm từ Repository và truyền sang View
            var products = _productRepository.GetAll();
            return View(products);
        }

        /// <summary>
        /// Action Display: Hiển thị chi tiết một sản phẩm theo Id.
        /// GET: /Product/Display/5
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xem chi tiết</param>
        public IActionResult Display(int id)
        {
            // Tìm sản phẩm theo Id
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                // Nếu không tìm thấy sản phẩm, trả về trang 404
                return NotFound();
            }

            // Truyền tên danh mục qua ViewBag để hiển thị trên View
            var category = _categoryRepository.GetById(product.CategoryId);
            ViewBag.CategoryName = category?.Name ?? "Chưa phân loại";

            return View(product);
        }

        /// <summary>
        /// Action Add [GET]: Hiển thị form thêm sản phẩm mới.
        /// GET: /Product/Add
        /// </summary>
        [HttpGet]
        public IActionResult Add()
        {
            // Chuẩn bị danh sách danh mục cho dropdown trên form
            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        /// <summary>
        /// Action Add [POST]: Xử lý dữ liệu form thêm sản phẩm mới.
        /// Bao gồm xử lý upload file ảnh vào thư mục wwwroot/images/.
        /// POST: /Product/Add
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm từ form</param>
        /// <param name="imageFile">File ảnh người dùng upload</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product product, IFormFile? imageFile)
        {
            // Kiểm tra dữ liệu form có hợp lệ không (theo Data Annotations)
            if (ModelState.IsValid)
            {
                // Xử lý lưu file ảnh nếu người dùng có upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    product.ImageUrl = SaveImage(imageFile);
                }

                // Thêm sản phẩm vào Repository (Id sẽ được tự động tăng)
                _productRepository.Add(product);

                // Hiển thị thông báo thành công qua TempData
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";

                // Chuyển hướng về trang danh sách sản phẩm
                return RedirectToAction("Index");
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form với thông báo lỗi
            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Action Update [GET]: Hiển thị form chỉnh sửa sản phẩm.
        /// GET: /Product/Update/5
        /// </summary>
        /// <param name="id">Mã sản phẩm cần chỉnh sửa</param>
        [HttpGet]
        public IActionResult Update(int id)
        {
            // Tìm sản phẩm theo Id
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            // Chuẩn bị dropdown danh mục, chọn sẵn danh mục hiện tại của sản phẩm
            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Action Update [POST]: Xử lý cập nhật thông tin sản phẩm.
        /// Hỗ trợ thay đổi ảnh sản phẩm hoặc giữ ảnh cũ nếu không upload ảnh mới.
        /// POST: /Product/Update/5
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm với thông tin mới từ form</param>
        /// <param name="imageFile">File ảnh mới (tùy chọn)</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Nếu người dùng upload ảnh mới, lưu ảnh mới
                if (imageFile != null && imageFile.Length > 0)
                {
                    product.ImageUrl = SaveImage(imageFile);
                }
                else
                {
                    // Nếu không upload ảnh mới, giữ lại ảnh cũ từ sản phẩm hiện tại
                    var existingProduct = _productRepository.GetById(product.Id);
                    if (existingProduct != null)
                    {
                        product.ImageUrl = existingProduct.ImageUrl;
                    }
                }

                // Cập nhật sản phẩm trong Repository
                _productRepository.Update(product);

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form
            ViewBag.Categories = new SelectList(
                _categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Action Delete [GET]: Hiển thị trang xác nhận xóa sản phẩm.
        /// GET: /Product/Delete/5
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            // Truyền tên danh mục để hiển thị trên trang xác nhận xóa
            var category = _categoryRepository.GetById(product.CategoryId);
            ViewBag.CategoryName = category?.Name ?? "Chưa phân loại";

            return View(product);
        }

        /// <summary>
        /// Action DeleteConfirmed [POST]: Thực hiện xóa sản phẩm sau khi người dùng xác nhận.
        /// POST: /Product/DeleteConfirmed
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Thực hiện xóa sản phẩm khỏi Repository
            _productRepository.Delete(id);

            TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Phương thức private: Xử lý lưu file ảnh upload vào thư mục wwwroot/images/.
        /// Tạo tên file duy nhất bằng GUID để tránh trùng lặp.
        /// </summary>
        /// <param name="imageFile">File ảnh từ form upload</param>
        /// <returns>Tên file ảnh đã lưu (để gán vào thuộc tính ImageUrl)</returns>
        private string SaveImage(IFormFile imageFile)
        {
            // Tạo thư mục images nếu chưa tồn tại
            var imagesFolder = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Tạo tên file duy nhất: GUID + phần mở rộng gốc của file
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(imagesFolder, uniqueFileName);

            // Lưu file ảnh vào đĩa
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            // Trả về tên file để lưu vào thuộc tính ImageUrl của sản phẩm
            return uniqueFileName;
        }
    }
}
