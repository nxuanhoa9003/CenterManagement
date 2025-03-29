using Domain.Entities;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddCategoryAsync(Category category) => await InsertAsync(category);

        public async Task UpdateCategoryAsync(Category category) => await UpdateAsync(category);


        public async Task DeleteCategoryAsync(Guid Id) => await DeleteAsync(x => x.Id == Id);

        public async Task<IEnumerable<Category>> GetCategories() => await GetAllAsync();

        public async Task<Category?> GetCategoryById(Guid? Id) => await GetByIdAsync(Id);

        public async Task<bool> CategoryExistsAsync(Guid Id) => await AnyAsync(x => x.Id == Id);
        public async Task<bool> CategoryNameExistsAsync(string name) => await AnyAsync(x => x.Name.ToLower() == name.ToLower());

        public async Task<IEnumerable<Category>> GetCategoryByName(string? name) => await GetManyAsync(x => x.Name.ToLower().Contains(name.ToLower()));
    }
}
