using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebThucPhamSach.Data;

namespace WebThucPhamSach.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // === 1. DANH SÁCH TẤT CẢ ĐƠN HÀNG ===
        public async Task<IActionResult> Index(string? statusFilter)
        {
            var query = _context.Orders
                .Include(o => o.ApplicationUser)
                .AsQueryable();

            // Lọc theo trạng thái nếu có
            if (!string.IsNullOrEmpty(statusFilter))
            {
                query = query.Where(o => o.Status == statusFilter);
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Tính toán thống kê nhanh
            ViewBag.TotalOrders = await _context.Orders.CountAsync();
            ViewBag.PendingOrders = await _context.Orders.CountAsync(o => o.Status == "Chờ duyệt");
            ViewBag.DeliveredOrders = await _context.Orders.CountAsync(o => o.Status == "Đã giao");
            ViewBag.TotalRevenue = await _context.Orders
                .Where(o => o.Status == "Đã giao")
                .SumAsync(o => o.TotalPrice);

            ViewBag.CurrentFilter = statusFilter;

            return View(orders);
        }

        // === 2. XEM CHI TIẾT ĐƠN HÀNG (ADMIN) ===
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng");
            }

            return View(order);
        }

        // === 3. CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG (POST) ===
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng");
            }

            // Danh sách trạng thái hợp lệ
            var validStatuses = new List<string> { "Chờ duyệt", "Đã duyệt", "Đang giao", "Đã giao", "Đã hủy" };
            if (!validStatuses.Contains(status))
            {
                TempData["ErrorMessage"] = "Trạng thái đơn hàng không hợp lệ!";
                return RedirectToAction(nameof(Details), new { id = order.Id });
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã cập nhật trạng thái đơn hàng #{order.Id} sang \"{status}\"!";
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }
    }
}
