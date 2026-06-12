using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webthucphamsach.Models;

namespace webthucphamsach.Data
{
    /// <summary>
    /// ApplicationDbContext kế thừa từ IdentityDbContext để tích hợp
    /// ASP.NET Core Identity với ApplicationUser tùy chỉnh.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // BẮT BUỘC gọi base để Identity tạo đủ các bảng cần thiết
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ 1-nhiều giữa Product và Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ cho OrderDetail (Giảm thiểu lỗi trùng lặp cascade path trên SQL Server)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Gieo dữ liệu mẫu (Seeding) Danh mục
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Rau xanh hữu cơ" },
                new Category { Id = 2, Name = "Trái cây sạch" },
                new Category { Id = 3, Name = "Trứng & Sữa tươi" },
                new Category { Id = 4, Name = "Đặc sản vùng miền" }
            );

            // Gieo dữ liệu mẫu (Seeding) Sản phẩm
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Rau cải bó xôi hữu cơ Đà Lạt",
                    Price = 35000,
                    Description = "Cải bó xôi tươi giòn, giàu chất sắt và vitamin, trồng theo tiêu chuẩn hữu cơ, không thuốc hóa học.",
                    CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1576045057995-568f588f82fb?w=600&auto=format&fit=crop&q=80"
                },
                new Product
                {
                    Id = 2,
                    Name = "Táo đỏ Fuji hữu cơ cao cấp",
                    Price = 89000,
                    Description = "Táo Fuji giòn ngọt, mọng nước, nhập khẩu chính ngạch từ trang trại đạt chuẩn hữu cơ thế giới.",
                    CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1619546813926-a78fa6372cd2?w=600&auto=format&fit=crop&q=80"
                },
                new Product
                {
                    Id = 3,
                    Name = "Cà rốt ngọt Đà Lạt VietGAP",
                    Price = 28000,
                    Description = "Củ cà rốt tươi ngon, ngọt đậm, thích hợp dùng làm nước ép hoặc chế biến món ăn bổ dưỡng mỗi ngày.",
                    CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1598170845058-32b9d6a5da37?w=600&auto=format&fit=crop&q=80"
                },
                new Product
                {
                    Id = 4,
                    Name = "Dâu tây đỏ New Zealand trồng Đà Lạt",
                    Price = 120000,
                    Description = "Dâu tây căng mọng, vị chua ngọt hài hòa, được thu hoạch trực tiếp tại vườn công nghệ cao vào sáng sớm.",
                    CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1464965911861-746a04b4bca6?w=600&auto=format&fit=crop&q=80"
                },
                new Product
                {
                    Id = 5,
                    Name = "Trứng gà ta thảo mộc tự nhiên",
                    Price = 45000,
                    Description = "Trứng từ đàn gà ta thả vườn được nuôi dưỡng bằng thảo mộc tự nhiên, hàm lượng dinh dưỡng cao vượt trội.",
                    CategoryId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1550583724-b2692b85b150?w=600&auto=format&fit=crop&q=80"
                },
                new Product
                {
                    Id = 6,
                    Name = "Mật ong rừng tràm U Minh nguyên chất",
                    Price = 180000,
                    Description = "Mật ong rừng tự nhiên nguyên chất 100%, mùi thơm đặc trưng, vị ngọt thanh tốt cho hệ tiêu hóa.",
                    CategoryId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1587049352846-4a222e784d38?w=600&auto=format&fit=crop&q=80"
                }
            );
        }
    }
}
