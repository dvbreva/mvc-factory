using System.Collections.Generic;
using System.Threading.Tasks;
using FF.Data.Entities;
using FF.Models;

namespace FF.Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders();

        Task<Order> GetById(int id);

        Task<ServiceResult> Insert(Order order, IEnumerable<int> selectedProducts);

        Task<ServiceResult> Update(int id, Order order, IEnumerable<int> selectedProducts);

        Task<ServiceResult> Delete(int id);

        Task<bool> Exists(int id);
    }
}
