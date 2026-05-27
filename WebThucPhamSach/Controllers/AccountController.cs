using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebThucPhamSach.Models;

namespace WebThucPhamSach.Controllers
{
    /// <summary>
    /// Controller xử lý đăng ký, đăng nhập và đăng xuất tài khoản.
    /// Sử dụng ASP.NET Core Identity thông qua UserManager và SignInManager.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // ============================================================
        // ĐĂNG KÝ TÀI KHOẢN
        // ============================================================

        /// <summary>
        /// GET: /Account/Register — Hiển thị form đăng ký
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            // Nếu đã đăng nhập rồi thì chuyển về trang chủ
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Product");

            return View();
        }

        /// <summary>
        /// POST: /Account/Register — Xử lý đăng ký tài khoản mới.
        /// Tự động gán Role "Member" cho tài khoản vừa tạo.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Tạo đối tượng ApplicationUser từ dữ liệu form
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                Address = model.Address
            };

            // Tạo tài khoản với mật khẩu (Identity tự động hash)
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Đảm bảo Role "Member" tồn tại trước khi gán
                if (!await _roleManager.RoleExistsAsync("Member"))
                    await _roleManager.CreateAsync(new IdentityRole("Member"));

                // Gán Role "Member" mặc định cho tài khoản mới đăng ký
                await _userManager.AddToRoleAsync(user, "Member");

                // Tự động đăng nhập sau khi đăng ký thành công
                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["SuccessMessage"] = $"Chào mừng {user.FullName}! Đăng ký thành công.";
                return RedirectToAction("Index", "Product");
            }

            // Nếu có lỗi (vd: email đã tồn tại, mật khẩu quá yếu), hiển thị lại form
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // ============================================================
        // ĐĂNG NHẬP
        // ============================================================

        /// <summary>
        /// GET: /Account/Login — Hiển thị form đăng nhập
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Product");

            // Lưu returnUrl vào ViewData để redirect sau khi đăng nhập
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// POST: /Account/Login — Xử lý đăng nhập bằng Email + Password
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            // Tìm tài khoản theo Email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
                return View(model);
            }

            // Đăng nhập bằng Username (Identity dùng UserName nội bộ)
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Chào mừng trở lại, {user.FullName}!";

                // Redirect về trang yêu cầu trước đó hoặc trang chủ
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
            return View(model);
        }

        // ============================================================
        // ĐĂNG XUẤT
        // ============================================================

        /// <summary>
        /// POST: /Account/Logout — Đăng xuất tài khoản hiện tại
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "Bạn đã đăng xuất thành công.";
            return RedirectToAction("Index", "Product");
        }

        /// <summary>
        /// GET: /Account/AccessDenied — Trang thông báo không có quyền truy cập
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
