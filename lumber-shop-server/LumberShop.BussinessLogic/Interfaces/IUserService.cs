using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task Add(UserDTO user);
        Task Update(UserDTO user);
        Task Delete(int id);
    }
}
