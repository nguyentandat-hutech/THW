using System.Collections.Generic;
using System.Linq;
using webthucphamsach.Models;

namespace webthucphamsach.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public MockProductRepository()
        {
            _products = new List<Product>
            {
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
            };
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.FromResult(_products);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public async Task AddAsync(Product product)
        {
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            await Task.CompletedTask;
        }
    }
}
