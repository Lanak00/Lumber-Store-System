using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using LumberStoreSystem.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository clientRepository)
        {
            _addressRepository = clientRepository;
        }

        public async Task<int> GetOrCreateAddressAsync(NewAddressDTO addressDTO)
        {
            var existingAddressId = await _addressRepository.FindByAll(new Address
            {
                Street = addressDTO.Street,
                Number = addressDTO.Number,
                City = addressDTO.City,
                Country = addressDTO.Country
            });

            if (existingAddressId.HasValue)
            {
                return existingAddressId.Value; 
            }

            var newAddress = new Address
            {
                Street = addressDTO.Street,
                Number = addressDTO.Number,
                City = addressDTO.City,
                Country = addressDTO.Country
            };

            await _addressRepository.Add(newAddress); 

            return newAddress.Id; 
        }


        public async Task<int> Add(NewAddressDTO addressDTO)
        {
            var address = new Address
            {
                Id = new int(),
                Street = addressDTO.Street,
                City = addressDTO.City,
                Country = addressDTO.Country,
                Number = addressDTO.Number,        
            };
            return await _addressRepository.Add(address);
        }

        public async Task Delete(int id)
        {
            var address = await _addressRepository.GetById(id);
            await _addressRepository.Delete(address);
        }

        public async Task<int?> FindByAll(AddressDTO addressDTO)
        {
            var address = new Address
            {
                Street = addressDTO.Street,
                City = addressDTO.City,
                Country = addressDTO.Country,
                Number = addressDTO.Number,
            };
            return await _addressRepository.FindByAll(address);
        }

        public async Task<Address> GetById(int id)
        {
            return await _addressRepository.GetById(id);
        }
    }
}
