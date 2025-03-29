using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Category>> GetCategoryByName(string? name);
        Task<Category?> GetCategoryById(Guid? Id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Guid Id);
        Task<bool> CategoryExistsAsync(Guid Id);
        Task<bool> CategoryNameExistsAsync(string name);
    }
}
