using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebThucPhamSach.Data;
using WebThucPhamSach.Models;
using WebThucPhamSach.Repositories;

var builder = WebApplication.CreateBuilder(args);

// === ĐĂNG KÝ CÁC SERVICE VÀO DI CONTAINER ===

// Đăng ký MVC Controllers và Views (hỗ trợ cả Areas)
builder.Services.AddControllersWithViews();

// --- Cấu hình Entity Framework Core với SQL Server ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// --- Cấu hình ASP.NET Core Identity ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Cấu hình độ phức tạp mật khẩu
    options.Password.RequireDigit = true;              // Phải có chữ số
    options.Password.RequireLowercase = true;           // Phải có chữ thường
    options.Password.RequireUppercase = true;           // Phải có chữ hoa
    options.Password.RequireNonAlphanumeric = false;   // Không bắt buộc ký tự đặc biệt
    options.Password.RequiredLength = 6;               // Tối thiểu 6 ký tự

    // Không yêu cầu xác nhận email khi đăng ký (đơn giản hóa demo)
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()     // Lưu Identity vào SQL Server
.AddDefaultTokenProviders();                          // Tạo token cho reset mật khẩu, v.v.

// --- Cấu hình đường dẫn khi chưa đăng nhập / không có quyền ---
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";          // Chuyển đến trang Login khi chưa đăng nhập
    options.AccessDeniedPath = "/Account/AccessDenied"; // Chuyển đến trang từ chối khi không đủ quyền
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Cookie hết hạn sau 7 ngày
});

// --- Đăng ký EF Repositories (Scoped) ---
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// --- Cấu hình Session ---
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session tồn tại trong 30 phút
    options.Cookie.HttpOnly = true;                 // Chỉ truy cập cookie qua HTTP
    options.Cookie.IsEssential = true;              // Đánh dấu cookie là cần thiết
});

var app = builder.Build();

// === SEED DATA: TẠO ROLES VÀ TÀI KHOẢN MẪU ===
// Chạy sau khi app build xong, trước khi bắt đầu lắng nghe request
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedRolesAndUsers(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi seed Roles và Users.");
    }
}

// === CẤU HÌNH MIDDLEWARE PIPELINE ===

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Sử dụng Session Middleware
app.UseSession();

// QUAN TRỌNG: UseAuthentication phải trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Đăng ký route cho Areas (phải đặt trước default route)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

// Route mặc định: Trang chủ → danh sách sản phẩm
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();

// ============================================================
// HÀM SEED: Tạo 2 Roles và 2 tài khoản mẫu nếu chưa có
// ============================================================
static async Task SeedRolesAndUsers(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // --- Tạo 2 Roles: Admin và Member ---
    string[] roles = { "Admin", "Member" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // --- Tạo tài khoản Admin mẫu ---
    const string adminEmail = "admin@organicfood.vn";
    const string adminPassword = "Admin@123";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = adminEmail,
            FullName = "Quản trị viên",
            Address = "254 Dương Đình Hội, TP.HCM",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // --- Tạo tài khoản Member mẫu ---
    const string memberEmail = "member@organicfood.vn";
    const string memberPassword = "Member@123";

    if (await userManager.FindByEmailAsync(memberEmail) == null)
    {
        var memberUser = new ApplicationUser
        {
            UserName = "member",
            Email = memberEmail,
            FullName = "Nguyễn Văn A",
            Address = "123 Lê Văn Việt, TP.HCM",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(memberUser, memberPassword);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(memberUser, "Member");
    }
}
