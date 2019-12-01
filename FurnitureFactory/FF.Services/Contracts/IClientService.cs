using System.Collections.Generic;
using System.Threading.Tasks;
using FF.Data.Entities;
using FF.Models;

namespace FF.Services.Contracts
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetClients();

        Task<Client> GetById(int id);

        Task<ServiceResult> Insert(Client client);

        Task<ServiceResult> Update(int id, Client client);

        Task<ServiceResult> Delete(int id);

        Task<bool> Exists(int id);
    }
}
