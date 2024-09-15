using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{
    public interface IDimensionsService
    {
        Task<IEnumerable<Dimensions>> GetAll();
        Task<int?> FindByAll(DimensionsDTO dimensions);
        Task<Dimensions> GetById(int id);
        Task<int> Add(NewDimensionsDTO dimensions);
        Task Delete(int id);
    }
}
