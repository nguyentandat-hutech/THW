using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebThucPhamSach.Data;
using WebThucPhamSach.Helpers;
using WebThucPhamSach.Models;
using WebThucPhamSach.Repositories;

namespace WebThucPhamSach.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private const string CART_KEY = "Cart";

        public CartController(
            IProductRepository productRepository, 
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        // Lấy giỏ hàng từ Session
        private List<CartItem> GetCartItems()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY);
            return cart ?? new List<CartItem>();
        }

        // Lưu giỏ hàng vào Session
        private void SaveCartItems(List<CartItem> cart)
        {
            HttpContext.Session.Set(CART_KEY, cart);
        }

        // === 1. XEM GIỎ HÀNG ===
        public IActionResult Index()
        {
            var cart = GetCartItems();
            
            // Tính tổng tiền
            decimal total = cart.Sum(item => item.Product.Price * item.Quantity);
            ViewBag.TotalPrice = total;
            
            return View(cart);
        }

        // === 2. THÊM SẢN PHẨM VÀO GIỎ ===
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            var cart = GetCartItems();
            var cartItem = cart.FirstOrDefault(item => item.ProductId == id);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = id,
                    Product = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Description = product.Description,
                        CategoryId = product.CategoryId
                    },
                    Quantity = quantity
                });
            }

            SaveCartItems(cart);

            // Quay lại trang trước hoặc vào Giỏ hàng
            TempData["SuccessMessage"] = $"Đã thêm \"{product.Name}\" vào giỏ hàng!";
            return RedirectToAction(nameof(Index));
        }

        // === 3. CẬP NHẬT SỐ LƯỢNG ===
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            if (quantity <= 0)
            {
                return RemoveFromCart(id);
            }

            var cart = GetCartItems();
            var cartItem = cart.FirstOrDefault(item => item.ProductId == id);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                SaveCartItems(cart);
                TempData["SuccessMessage"] = "Đã cập nhật số lượng!";
            }

            return RedirectToAction(nameof(Index));
        }

        // === 4. XÓA SẢN PHẨM KHỎI GIỎ ===
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCartItems();
            var cartItem = cart.FirstOrDefault(item => item.ProductId == id);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCartItems(cart);
                TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng!";
            }

            return RedirectToAction(nameof(Index));
        }

        // === 5. XÓA SẠCH GIỎ HÀNG ===
        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CART_KEY);
            TempData["SuccessMessage"] = "Đã làm trống giỏ hàng!";
            return RedirectToAction(nameof(Index));
        }

        // === 6. ĐẶT HÀNG (CHECKOUT - GET) ===
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCartItems();
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm trước khi thanh toán!";
                return RedirectToAction(nameof(Index));
            }

            // Lấy thông tin user hiện tại
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // Truyền thông tin sẵn có để điền vào form
            ViewBag.FullName = user.FullName;
            ViewBag.Address = user.Address;
            ViewBag.PhoneNumber = user.PhoneNumber; // Nếu có

            decimal total = cart.Sum(item => item.Product.Price * item.Quantity);
            ViewBag.TotalPrice = total;

            return View(cart);
        }

        // === 7. XỬ LÝ ĐẶT HÀNG (CHECKOUT - POST) ===
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string shippingAddress, string phoneNumber, string? notes)
        {
            var cart = GetCartItems();
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(shippingAddress) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ Số điện thoại và Địa chỉ giao hàng.");
                ViewBag.TotalPrice = cart.Sum(item => item.Product.Price * item.Quantity);
                return View("Checkout", cart);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // Tạo hóa đơn mới
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                ShippingAddress = shippingAddress,
                PhoneNumber = phoneNumber,
                Notes = notes,
                TotalPrice = cart.Sum(item => item.Product.Price * item.Quantity),
                Status = "Chờ duyệt"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Lưu để lấy Order.Id tự động sinh

            // Thêm chi tiết hóa đơn
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price // Lưu giá tại thời điểm mua
                };
                _context.OrderDetails.Add(orderDetail);
            }

            await _context.SaveChangesAsync();

            // Xóa giỏ hàng sau khi đặt thành công
            HttpContext.Session.Remove(CART_KEY);

            return RedirectToAction(nameof(OrderSuccess), new { id = order.Id });
        }

        // === 8. TRANG ĐẶT HÀNG THÀNH CÔNG ===
        [Authorize]
        public async Task<IActionResult> OrderSuccess(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || order.UserId != _userManager.GetUserId(User))
            {
                return NotFound("Không tìm thấy đơn hàng");
            }

            return View(order);
        }
    }
}
