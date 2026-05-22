namespace WebThucPhamSach.Repositories
{
    using WebThucPhamSach.Models;

    /// <summary>
    /// Lớp giả lập (Mock) dữ liệu sản phẩm trong bộ nhớ.
    /// Dùng để phát triển và kiểm thử mà không cần cơ sở dữ liệu thực.
    /// Đăng ký dưới dạng Singleton để dữ liệu tồn tại xuyên suốt phiên chạy ứng dụng.
    /// </summary>
    public class MockProductRepository : IProductRepository
    {
        // Danh sách sản phẩm lưu trong bộ nhớ (giả lập Database)
        private readonly List<Product> _products;

        /// <summary>
        /// Khởi tạo dữ liệu mẫu gồm 6 sản phẩm thực phẩm sạch đa dạng.
        /// </summary>
        public MockProductRepository()
        {
            _products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Rau cải hữu cơ VietGAP",
                    Price = 35000,
                    Description = "Rau cải xanh được trồng theo tiêu chuẩn VietGAP, không sử dụng thuốc trừ sâu hóa học. Nguồn gốc từ nông trại hữu cơ Đà Lạt, thu hoạch trong ngày, đảm bảo tươi sạch và an toàn cho sức khỏe gia đình bạn.",
                    CategoryId = 1,
                    ImageUrl = "rau-cai-huu-co.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Táo Fuji Organic",
                    Price = 120000,
                    Description = "Táo Fuji nhập khẩu Nhật Bản, đạt chứng nhận Organic JAS. Quả to đều, vỏ mỏng, thịt giòn ngọt thanh tự nhiên. Giàu vitamin C và chất xơ, thích hợp cho cả gia đình.",
                    CategoryId = 2,
                    ImageUrl = "tao-fuji-organic.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Thịt ba chỉ heo sạch",
                    Price = 185000,
                    Description = "Thịt ba chỉ heo được chăn nuôi theo quy trình sạch, không sử dụng chất tăng trọng hay kháng sinh. Thịt tươi ngon, có vân mỡ đều, phù hợp để nướng, kho hoặc luộc.",
                    CategoryId = 3,
                    ImageUrl = "thit-ba-chi-sach.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Bông cải xanh Organic",
                    Price = 45000,
                    Description = "Bông cải xanh (Broccoli) trồng hữu cơ tại Đà Lạt, giàu vitamin K, C và chất chống oxy hóa. Được thu hoạch và đóng gói cẩn thận, giữ nguyên độ tươi ngon.",
                    CategoryId = 1,
                    ImageUrl = "bong-cai-xanh.jpg"
                },
                new Product
                {
                    Id = 5,
                    Name = "Chuối già lùn Organic",
                    Price = 38000,
                    Description = "Chuối già lùn canh tác hữu cơ từ vùng đất Đồng Nai, quả to, vỏ dày, ruột chắc và ngọt tự nhiên. Không sử dụng thuốc bảo quản, an toàn tuyệt đối.",
                    CategoryId = 2,
                    ImageUrl = "chuoi-gia-lun.jpg"
                },
                new Product
                {
                    Id = 6,
                    Name = "Ức gà ta sạch",
                    Price = 95000,
                    Description = "Ức gà ta nuôi thả vườn, ăn thức ăn tự nhiên, thịt săn chắc và thơm ngon đặc trưng. Đạt tiêu chuẩn vệ sinh an toàn thực phẩm, thích hợp cho người ăn kiêng.",
                    CategoryId = 3,
                    ImageUrl = "uc-ga-ta-sach.jpg"
                }
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sản phẩm.
        /// </summary>
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        /// <summary>
        /// Tìm sản phẩm theo mã Id. Trả về null nếu không tìm thấy.
        /// </summary>
        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Thêm mới sản phẩm vào danh sách.
        /// Tự động tạo Id mới bằng cách lấy Id lớn nhất hiện tại + 1.
        /// </summary>
        public void Add(Product product)
        {
            // Tự động tăng ID: lấy max Id hiện tại + 1, nếu danh sách rỗng thì bắt đầu từ 1
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm đã tồn tại.
        /// Tìm sản phẩm cũ theo Id rồi thay thế bằng thông tin mới.
        /// </summary>
        public void Update(Product product)
        {
            // Tìm vị trí sản phẩm cần cập nhật trong danh sách
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                // Cập nhật từng thuộc tính của sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
            }
        }

        /// <summary>
        /// Xóa sản phẩm khỏi danh sách theo mã Id.
        /// </summary>
        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
