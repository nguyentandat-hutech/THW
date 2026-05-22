using WebThucPhamSach.Repositories;

var builder = WebApplication.CreateBuilder(args);

// === ĐĂNG KÝ CÁC SERVICE VÀO DI CONTAINER ===

// Đăng ký MVC Controllers và Views
builder.Services.AddControllersWithViews();

// Đăng ký Repository dưới dạng Singleton để dữ liệu Mock tồn tại xuyên suốt phiên chạy.
// Khi dùng Database thật, nên đổi sang AddScoped.
builder.Services.AddSingleton<IProductRepository, MockProductRepository>();
builder.Services.AddSingleton<ICategoryRepository, MockCategoryRepository>();

var app = builder.Build();

// === CẤU HÌNH MIDDLEWARE PIPELINE ===

// Hiển thị trang lỗi thân thiện khi không ở môi trường Development
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Bật phục vụ file tĩnh từ thư mục wwwroot (CSS, JS, Images)
app.UseStaticFiles();

// Bật định tuyến (Routing)
app.UseRouting();

// Bật xác thực quyền truy cập
app.UseAuthorization();

// Cấu hình Route mặc định: Trang chủ sẽ chuyển đến danh sách sản phẩm
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
