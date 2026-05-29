using System.Collections.Generic;
using System.Linq;
using webthucphamsach.Models;

namespace webthucphamsach.Repositories
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categories;

        public MockCategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category { Id = 1, Name = "Rau xanh hữu cơ" },
                new Category { Id = 2, Name = "Trái cây sạch" },
                new Category { Id = 3, Name = "Trứng & Sữa tươi" },
                new Category { Id = 4, Name = "Đặc sản vùng miền" }
            };
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await Task.FromResult(_categories);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));
        }

        public async Task AddAsync(Category category)
        {
            category.Id = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1;
            _categories.Add(category);
            await Task.CompletedTask;
        }
    }
}
