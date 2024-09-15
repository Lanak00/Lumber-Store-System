using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly LumberStoreSystemDbContext _context;
        public AddressRepository(LumberStoreSystemDbContext context)
        {
           _context = context;
        }

        public async Task<int> Add(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address.Id; 
        }


        public async Task Delete(Address address)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }

        public async Task<Address> GetById(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task<int?> FindByAll(Address address)
        {
            var existingAddress = await _context.Addresses
             .FirstOrDefaultAsync(a => a.Street == address.Street
                                  && a.Number == address.Number
                                  && a.City == address.City
                                  && a.Country == address.Country);

            return existingAddress?.Id;
        }
    }
}
