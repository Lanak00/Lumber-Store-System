using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Interfaces
{
    public interface IDimensionsRepository
    {
        Task<IEnumerable<Dimensions>> GetAll();
        Task<int?> FindByAll(Dimensions dimensions);
        Task<Dimensions> GetById(int id);
        Task<int> Add(Dimensions dimensions);
        Task Delete(Dimensions dimensions);
    }
}
