using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FF.Data;
using FF.Models;
using FF.Models.Repositories;
using FF.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FF.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryRepository.GetAll().ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            if (id < 1)
            {
                await Task.FromResult<Category>(null);
            }

            return await _categoryRepository.GetAll().Include(c => c.Products)
                                      .Where(c => c.Id == id)
                                      .FirstOrDefaultAsync();
        }

        public async Task<ServiceResult> Insert(Category category)
        {
            _categoryRepository.Add(category);
            await _categoryRepository.Save();

            return new ServiceResult();
        }

        public async Task<ServiceResult> Update(int id, Category category)
        {
            var categoryToUpdate = await _categoryRepository.GetById(id);

            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.Products = category.Products;

                await _categoryRepository.Save();
            }

            return new ServiceResult();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var categoryToDelete = await _categoryRepository.GetById(id);

            if (categoryToDelete != null)
            {
                _categoryRepository.Delete(categoryToDelete);
                await _categoryRepository.Save();
            }

            return new ServiceResult();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _categoryRepository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            return true;
        }
    }
}
