using System.Collections.Generic;
using System.Threading.Tasks;
using FF.Data.Entities;
using FF.Models;

namespace FF.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<Product> GetById(int id);

        Task<ServiceResult> Insert(Product product);

        Task<ServiceResult> Update(int id, Product product);

        Task<ServiceResult> Delete(int id);

        Task<bool> Exists(int id);
    }
}
