using System.Collections.Generic;
using System.Threading.Tasks;
using webthucphamsach.Models;

namespace webthucphamsach.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
    }
}
