using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webthucphamsach.Migrations
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
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Rau xanh hữu cơ" },
                    { 2, "Trái cây sạch" },
                    { 3, "Trứng & Sữa tươi" },
                    { 4, "Đặc sản vùng miền" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Cải bó xôi tươi giòn, giàu chất sắt và vitamin, trồng theo tiêu chuẩn hữu cơ, không thuốc hóa học.", "https://images.unsplash.com/photo-1576045057995-568f588f82fb?w=600&auto=format&fit=crop&q=80", "Rau cải bó xôi hữu cơ Đà Lạt", 35000m },
                    { 2, 2, "Táo Fuji giòn ngọt, mọng nước, nhập khẩu chính ngạch từ trang trại đạt chuẩn hữu cơ thế giới.", "https://images.unsplash.com/photo-1619546813926-a78fa6372cd2?w=600&auto=format&fit=crop&q=80", "Táo đỏ Fuji hữu cơ cao cấp", 89000m },
                    { 3, 1, "Củ cà rốt tươi ngon, ngọt đậm, thích hợp dùng làm nước ép hoặc chế biến món ăn bổ dưỡng mỗi ngày.", "https://images.unsplash.com/photo-1598170845058-32b9d6a5da37?w=600&auto=format&fit=crop&q=80", "Cà rốt ngọt Đà Lạt VietGAP", 28000m },
                    { 4, 2, "Dâu tây căng mọng, vị chua ngọt hài hòa, được thu hoạch trực tiếp tại vườn công nghệ cao vào sáng sớm.", "https://images.unsplash.com/photo-1464965911861-746a04b4bca6?w=600&auto=format&fit=crop&q=80", "Dâu tây đỏ New Zealand trồng Đà Lạt", 120000m },
                    { 5, 3, "Trứng từ đàn gà ta thả vườn được nuôi dưỡng bằng thảo mộc tự nhiên, hàm lượng dinh dưỡng cao vượt trội.", "https://images.unsplash.com/photo-1550583724-b2692b85b150?w=600&auto=format&fit=crop&q=80", "Trứng gà ta thảo mộc tự nhiên", 45000m },
                    { 6, 4, "Mật ong rừng tự nhiên nguyên chất 100%, mùi thơm đặc trưng, vị ngọt thanh tốt cho hệ tiêu hóa.", "https://images.unsplash.com/photo-1587049352846-4a222e784d38?w=600&auto=format&fit=crop&q=80", "Mật ong rừng tràm U Minh nguyên chất", 180000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
