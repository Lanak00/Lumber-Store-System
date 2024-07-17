using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{
    public interface IAddressService
    {
        Task<Address> GetById(int id);
        Task<int?> FindByAll(AddressDTO address);
        Task<int> Add(NewAddressDTO address);
        Task Delete(int id);
    }
}
