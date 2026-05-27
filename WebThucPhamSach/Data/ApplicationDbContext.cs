using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebThucPhamSach.Models;

namespace WebThucPhamSach.Data
{
    /// <summary>
    /// Lớp DbContext trung tâm của ứng dụng.
    /// Kế thừa IdentityDbContext để tích hợp đầy đủ ASP.NET Core Identity
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ============================================================
        // CÁC BẢNG (TABLE) TRONG DATABASE
        // ============================================================

        /// <summary>Bảng Products - lưu trữ thông tin sản phẩm thực phẩm sạch.</summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>Bảng Categories - lưu trữ các danh mục sản phẩm.</summary>
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>Bảng Orders - lưu trữ thông tin đơn hàng.</summary>
        public DbSet<Order> Orders { get; set; } = null!;

        /// <summary>Bảng OrderDetails - lưu trữ chi tiết từng đơn hàng.</summary>
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

        // ============================================================
        // CẤU HÌNH MODEL & DỮ LIỆU SEED
        // ============================================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // BẮT BUỘC gọi base để Identity tạo đủ các bảng AspNet*
            base.OnModelCreating(modelBuilder);

            // --- Cấu hình bảng Category ---
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // --- Cấu hình bảng Product ---
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");
                entity.Property(p => p.Description)
                      .HasMaxLength(1000);
                entity.Property(p => p.ImageUrl)
                      .HasMaxLength(500);
            });

            // --- Cấu hình bảng Order ---
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.TotalPrice)
                      .HasColumnType("decimal(18,2)");
                entity.HasOne(o => o.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Cấu hình bảng OrderDetail ---
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(od => od.Id);
                entity.Property(od => od.Price)
                      .HasColumnType("decimal(18,2)");
                entity.HasOne(od => od.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(od => od.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(od => od.Product)
                      .WithMany()
                      .HasForeignKey(od => od.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================================
            // SEED DATA — Danh mục và sản phẩm mẫu
            // ============================================================

            // --- 3 Danh mục sản phẩm ---
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Rau củ hữu cơ" },
                new Category { Id = 2, Name = "Trái cây tươi sạch" },
                new Category { Id = 3, Name = "Thịt & Hải sản sạch" }
            );

            // --- 5 Sản phẩm thực phẩm sạch ---
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Cải bó xôi hữu cơ",
                    Price = 35000,
                    Description = "Cải bó xôi trồng theo tiêu chuẩn hữu cơ VietGAP, không thuốc trừ sâu, thu hoạch buổi sáng đảm bảo độ tươi ngon.",
                    CategoryId = 1,
                    ImageUrl = "cai-bo-xoi.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Cà chua bi đà lạt",
                    Price = 28000,
                    Description = "Cà chua bi Đà Lạt đạt chứng nhận GlobalGAP, vị ngọt tự nhiên, màu đỏ đẹp, phù hợp salad và nấu ăn.",
                    CategoryId = 1,
                    ImageUrl = "ca-chua-bi.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Xoài cát Hòa Lộc",
                    Price = 85000,
                    Description = "Xoài cát Hòa Lộc chính hiệu Tiền Giang, thịt vàng dày, ngọt thanh, không xơ, đạt tiêu chuẩn xuất khẩu.",
                    CategoryId = 2,
                    ImageUrl = "xoai-cat.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Bơ sáp 034 Đắk Lắk",
                    Price = 65000,
                    Description = "Bơ sáp 034 từ Đắk Lắk, béo ngậy tự nhiên, giàu vitamin E và chất béo tốt, chín đều không đắng.",
                    CategoryId = 2,
                    ImageUrl = "bo-sap.jpg"
                },
                new Product
                {
                    Id = 5,
                    Name = "Thịt lợn hữu cơ Organic",
                    Price = 185000,
                    Description = "Thịt lợn nuôi hoàn toàn bằng thức ăn hữu cơ, không kháng sinh, không chất tăng trưởng, đảm bảo an toàn thực phẩm.",
                    CategoryId = 3,
                    ImageUrl = "thit-lon-organic.jpg"
                }
            );
        }
    }
}
