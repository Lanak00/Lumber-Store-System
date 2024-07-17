using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> GetById(int id);
        Task<int?> FindByAll(Address address);
        Task<int> Add(Address address);
        Task Delete(Address address);
    }
}
