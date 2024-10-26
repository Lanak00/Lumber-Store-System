using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using BCrypt.Net;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task Add(NewClientDTO clientDTO)
        {
            var client = new Client
            {
                Id = new int(),
                FirstName = clientDTO.FirstName,
                LastName = clientDTO.LastName,
                Email = clientDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(clientDTO.Password),
                DateOfBirth = clientDTO.DateOfBirth,
                PhoneNumber = clientDTO.PhoneNumber,
                AddressId = clientDTO.AddressId,
                UserRole = DataAccess.Model.Enummerations.UserRole.Client
            };
            await _clientRepository.Add(client);
        }

        public async Task Delete(int id)
        {
            var client = await _clientRepository.GetById(id);
            await _clientRepository.Delete(client);
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _clientRepository.GetAll();
        }

        public async Task<Client> GetById(int id)
        {
            return await _clientRepository.GetById(id);
        }

        public async Task Update(ClientDTO clientDTO)
        {
            var client = new Client
            {
                Id = clientDTO.Id,
                FirstName = clientDTO.FirstName,
                LastName = clientDTO.LastName,
                Email = clientDTO.Email,
                Password = clientDTO.Password,
                DateOfBirth = clientDTO.DateOfBirth,
                PhoneNumber = clientDTO.PhoneNumber,
                AddressId = clientDTO.AddressId,
                UserRole = DataAccess.Model.Enummerations.UserRole.Client
            };
            await _clientRepository.Update(client);
        }
    }
}
