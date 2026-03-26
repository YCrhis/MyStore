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

        public async Task AddAsync(CategoryVM viewModel)
        {
            var entity = new Category
            {
                Name = viewModel.Name
            };
            await _categoryRepository.AddAsync(entity);
        }

        public async Task<CategoryVM?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            var categortyVM = new CategoryVM();

            if (category != null)
            {
                categortyVM.CategoryId = category.CategoryId;
                categortyVM.Name = category.Name;

            }
            return categortyVM;
        }

        public async Task EditAsync(CategoryVM viewModel)
        {
            var entity = new Category
            {
                CategoryId = viewModel.CategoryId,
                Name = viewModel.Name
            };
            await _categoryRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
            }

        }
    }
}
