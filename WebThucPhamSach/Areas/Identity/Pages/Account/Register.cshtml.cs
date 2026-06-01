// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using webthucphamsach.Models;
using webthucphamsach.Utility;

namespace webthucphamsach.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Dữ liệu ràng buộc 2 chiều với form Đăng ký.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// URL chuyển hướng sau khi đăng ký thành công.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Danh sách các scheme xác thực bên ngoài (Google, Facebook...).
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// Danh sách Vai trò để hiển thị dropdown khi đăng ký.
        /// </summary>
        public IEnumerable<SelectListItem> RoleList { get; set; }

        /// <summary>
        /// Model chứa các trường input của form Đăng ký.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
            [Display(Name = "Họ và Tên")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
            [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
            [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} và tối đa {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Vai trò")]
            public string Role { get; set; }
        }

        /// <summary>
        /// Xử lý GET: Chuẩn bị form đăng ký - tạo Role nếu chưa có, đổ danh sách Role.
        /// </summary>
        public async Task OnGetAsync(string returnUrl = null)
        {
            // Tự động tạo các Role nếu chúng chưa tồn tại trong DB
            if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Role_Employee))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Role_Customer))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
            }

            // Đổ danh sách Role vào RoleList để hiển thị dropdown
            RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            });

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// Xử lý POST: Tạo tài khoản mới với FullName và Role được chọn.
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Đổ lại danh sách Role trong trường hợp validation thất bại
            RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            });

            if (ModelState.IsValid)
            {
                // Tạo đối tượng ApplicationUser với thông tin bổ sung
                var user = CreateUser();
                user.FullName = Input.FullName; // Gán FullName từ form

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Người dùng đã tạo tài khoản mới với mật khẩu.");

                    // Gán Role cho người dùng vừa đăng ký
                    if (!string.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    else
                    {
                        // Mặc định gán Role Customer nếu không chọn
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }

                    // Đăng nhập ngay sau khi đăng ký thành công
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                // Hiển thị lỗi nếu tạo tài khoản thất bại
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Nếu có lỗi, render lại trang với dữ liệu hiện có
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Không thể tạo instance của '{nameof(ApplicationUser)}'. " +
                    $"Đảm bảo '{nameof(ApplicationUser)}' không phải là lớp abstract và có constructor không tham số.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Giao diện mặc định yêu cầu một user store hỗ trợ email.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
