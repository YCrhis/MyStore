using Microsoft.EntityFrameworkCore;
using MyStore.Entities;
using MyStore.Models;
using MyStore.Repositories;

namespace MyStore.Services
{
    
    public class CategoryService
    {
        private readonly GenericRepository<Category> _categoryRepository;

        public CategoryService(GenericRepository<Category> category)
        {
            _categoryRepository = category;
        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var categoriesVM = categories.Select(c => new CategoryVM
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }).ToList();

            return categoriesVM;

        }

    }
}
