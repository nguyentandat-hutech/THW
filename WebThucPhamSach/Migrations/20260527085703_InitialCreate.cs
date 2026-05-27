using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebThucPhamSach.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Rau củ hữu cơ" },
                    { 2, "Trái cây tươi sạch" },
                    { 3, "Thịt & Hải sản sạch" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Cải bó xôi trồng theo tiêu chuẩn hữu cơ VietGAP, không thuốc trừ sâu, thu hoạch buổi sáng đảm bảo độ tươi ngon.", "cai-bo-xoi.jpg", "Cải bó xôi hữu cơ", 35000m },
                    { 2, 1, "Cà chua bi Đà Lạt đạt chứng nhận GlobalGAP, vị ngọt tự nhiên, màu đỏ đẹp, phù hợp salad và nấu ăn.", "ca-chua-bi.jpg", "Cà chua bi đà lạt", 28000m },
                    { 3, 2, "Xoài cát Hòa Lộc chính hiệu Tiền Giang, thịt vàng dày, ngọt thanh, không xơ, đạt tiêu chuẩn xuất khẩu.", "xoai-cat.jpg", "Xoài cát Hòa Lộc", 85000m },
                    { 4, 2, "Bơ sáp 034 từ Đắk Lắk, béo ngậy tự nhiên, giàu vitamin E và chất béo tốt, chín đều không đắng.", "bo-sap.jpg", "Bơ sáp 034 Đắk Lắk", 65000m },
                    { 5, 3, "Thịt lợn nuôi hoàn toàn bằng thức ăn hữu cơ, không kháng sinh, không chất tăng trưởng, đảm bảo an toàn thực phẩm.", "thit-lon-organic.jpg", "Thịt lợn hữu cơ Organic", 185000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
