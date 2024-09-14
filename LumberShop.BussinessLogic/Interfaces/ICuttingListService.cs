using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{
    public interface ICuttingListService
    {
        Task<IEnumerable<CuttingList>> GetAll();
        Task<CuttingList> GetById(int id);
        Task<IEnumerable<CuttingList>> GetByOrderId(int orderId);
        Task Add(NewCuttingListDTO cuttingList);
        Task Delete(int id);
    }
}
