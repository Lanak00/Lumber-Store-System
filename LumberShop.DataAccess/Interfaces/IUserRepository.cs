using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(int id);

        Task Add(User user);

        Task Update(User user);

        Task Delete(User user);
    }
}
