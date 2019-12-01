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
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;

        public OrderService(
            IRepository<Order> orderRepository,
            IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderRepository.GetAll()
                .Include(c => c.Client)
                .Include(p => p.OrderedProducts).ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            if (id < 1)
            {
                return await Task.FromResult<Order>(null);
            }

            return await _orderRepository.GetAll()
                .Include(c => c.Client)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceResult> Insert(Order order, IEnumerable<int> selectedProducts)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (IsInvoiceNumberTaken(order.InvoiceNumber))
            {
                return ServiceResult.Failed(new ServiceError(nameof(Order.InvoiceNumber), "An order with the same Invoice Number already exists."));
            }

            order.OrderedProducts = _productRepository
                    .GetAll()
                    .Where(p => selectedProducts.Contains(p.Id))
                    .Select(p => new OrderProduct
                    {
                        ProductId = p.Id,
                        Quantity = 1,
                        UnitPrice = p.Price
                    })
                    .ToList();

            _orderRepository.Add(order);
            await _orderRepository.Save();

            return new ServiceResult();
        }

        public async Task<ServiceResult> Update(int id, Order order, IEnumerable<int> selectedProducts)
        {
            if(order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.OrderedProducts = _productRepository
                    .GetAll()
                    .Where(p => selectedProducts.Contains(p.Id))
                    .Select(p => new OrderProduct
                    {
                        ProductId = p.Id,
                        Quantity = 1,
                        UnitPrice = p.Price
                    })
                    .ToList();

            var orderToUpdate = await _orderRepository.GetById(id);

            if (orderToUpdate != null)
            {
                orderToUpdate.OrderedDate = order.OrderedDate;
                orderToUpdate.InvoiceNumber = order.InvoiceNumber;
                orderToUpdate.ClientId = order.ClientId;
                orderToUpdate.OrderedProducts = order.OrderedProducts;

                await _orderRepository.Save();
            }

            return new ServiceResult();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var orderToDelete = await _orderRepository.GetById(id);

            if (orderToDelete != null)
            {
                _orderRepository.Delete(orderToDelete);
                await _orderRepository.Save();
            }

            return new ServiceResult();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _orderRepository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            return true;
        }

        private bool IsInvoiceNumberTaken(string invoiceNumber, int? orderId = null)
        {
            var existingOrder = _orderRepository.GetAll()
                .FirstOrDefault(o => o.InvoiceNumber == invoiceNumber);

            return existingOrder != null && existingOrder.Id != orderId;
        }
    }
}
