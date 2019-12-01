using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FF.Data.Entities;
using FF.Models;
using FF.Models.Repositories;
using FF.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FF.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _clientRepository;

        public ClientService(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _clientRepository.GetAll().ToListAsync();
        }

        public async Task<Client> GetById(int id)
        {
            if (id < 1)
            {
                return null;
            }

            return await _clientRepository.GetById(id);
        }

        public async Task<ServiceResult> Insert(Client client)
        {
            if(client == null)
            {
                await Task.FromResult<ServiceResult>(null);
            }

            _clientRepository.Add(client);
            await _clientRepository.Save();

            return new ServiceResult();
        }

        public async Task<ServiceResult> Update(int id, Client client)
        {
            var clientToUpdate = await _clientRepository.GetById(id);

            if (clientToUpdate != null)
            {
                clientToUpdate.Name = client.Name;
                clientToUpdate.Address = client.Address;
                clientToUpdate.Bulstat = client.Bulstat;
                clientToUpdate.Vat = client.Vat;
                clientToUpdate.ResponsiblePerson = client.ResponsiblePerson;

                await _clientRepository.Save();
            }

            return new ServiceResult();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var clientToDelete = await _clientRepository.GetById(id);

            if (clientToDelete != null)
            {
                _clientRepository.Delete(clientToDelete);
                await _clientRepository.Save();
            }

            return new ServiceResult();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _clientRepository.GetById(id);

            if (entity == null)
            {
                return false;
            }

            return true;
        }
    }
}

