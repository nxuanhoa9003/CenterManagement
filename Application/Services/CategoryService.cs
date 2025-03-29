using Application.InterfacesServices;
using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddCategory(Category newCategory)
        {
            await _categoryRepository.AddCategoryAsync(newCategory);
        }

        public async Task<bool> CategoryExists(Guid Id)
        {
            return await _categoryRepository.CategoryExistsAsync(Id);
        }

        public async Task<bool> CategoryNameExistsAsync(string name)
        {
            return await _categoryRepository.CategoryNameExistsAsync(name);
        }

        public async Task DeleteCategory(Guid Id)
        {
            await _categoryRepository.DeleteCategoryAsync(Id);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryRepository.GetCategories();
        }

        public async Task<Category?> GetCategoryById(Guid Id)
        {
            return await _categoryRepository.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Category>> GetCategoryByName(string? name)
        {
            return await _categoryRepository.GetCategoryByName(name);
        }

        public async Task UpdateCategory(Category newCategory)
        {
            await _categoryRepository.UpdateCategoryAsync(newCategory);
        }
    }
}
