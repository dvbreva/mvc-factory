using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FF.Data.Entities;
using FF.Models;
using FF.Models.Repositories;
using FF.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FF.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productRepository
                .GetAll()
                .Include(c => c.Category)
                .ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            if (id < 1)
            {
                //TODO: return service error of not found
            }

            return await _productRepository
                .GetAll()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceResult> Insert(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _productRepository.Add(product);
            await _productRepository.Save();

            return new ServiceResult();
        }

        public async Task<ServiceResult> Update(int id, Product product)
        {
            var productToUpdate = await _productRepository.GetById(id);

            if (productToUpdate != null)
            {
                productToUpdate.Name = product.Name;
                productToUpdate.Description = product.Description;
                productToUpdate.WeightInKilos = product.WeightInKilos;
                productToUpdate.Price = product.Price;
                productToUpdate.CategoryId = product.CategoryId;

                await _productRepository.Save();
            }

            return new ServiceResult();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var productToDelete = await _productRepository.GetById(id);

            if (productToDelete != null)
            {
                _productRepository.Delete(productToDelete);
                await _productRepository.Save();
            }

            return new ServiceResult();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _productRepository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            return true;
        }
    }
}
