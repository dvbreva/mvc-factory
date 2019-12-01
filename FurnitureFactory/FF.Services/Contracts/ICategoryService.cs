using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FF.Data;
using FF.Models;

namespace FF.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<Category> GetById(int id);

        Task<ServiceResult> Insert(Category category);

        Task<ServiceResult> Update(int id, Category category);

        Task<ServiceResult> Delete(int id);

        Task<bool> Exists(int id);
    }
}