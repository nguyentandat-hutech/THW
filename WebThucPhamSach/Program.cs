using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webthucphamsach.Data;
using webthucphamsach.Models;
using webthucphamsach.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. ĐĂNG KÝ DỊCH VỤ (SERVICE REGISTRATION)
// ============================================================

// Thêm dịch vụ Controllers với Views
builder.Services.AddControllersWithViews();

// Đăng ký CSDL SQL Server với DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký ASP.NET Core Identity với ApplicationUser và IdentityRole tùy chỉnh
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Cấu hình chính sách mật khẩu
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

    // Cấu hình yêu cầu xác thực email (tắt để đơn giản hóa)
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cấu hình Application Cookie (đường dẫn đăng nhập, đăng xuất, từ chối truy cập)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Thêm hỗ trợ Razor Pages (cần thiết cho Identity UI scaffolded pages)
builder.Services.AddRazorPages();

// Đăng ký EF Repositories cho Dependency Injection (DI) dưới dạng Scoped
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();

// ============================================================
// 2. XÂY DỰNG ỨNG DỤNG (BUILD & MIDDLEWARE PIPELINE)
// ============================================================

var app = builder.Build();

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// QUAN TRỌNG: UseAuthentication phải đặt TRƯỚC UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// ============================================================
// 3. CẤU HÌNH ROUTING (ROUTE CONFIGURATION)
// ============================================================

// Route cho Area (phải đặt TRƯỚC route default)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Map các trang Razor Pages của Identity
app.MapRazorPages();

app.Run();
