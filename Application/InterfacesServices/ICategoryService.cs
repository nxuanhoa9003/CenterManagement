using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category?> GetCategoryById(Guid Id);
        Task<IEnumerable<Category>> GetCategoryByName(string? name);
        Task<bool> CategoryExists(Guid Id);
        Task<bool> CategoryNameExistsAsync(string name);
        Task AddCategory(Category newCategory);
        Task UpdateCategory(Category newCategory);
        Task DeleteCategory(Guid Id);
    }
}
