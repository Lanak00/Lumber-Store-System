using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Interfaces
{
    public interface ICuttingListRepository
    {
        Task<IEnumerable<CuttingList>> GetAll();
        Task<CuttingList> GetById(int id);
        Task<IEnumerable<CuttingList>> GetByOrderId(int orderId);
        Task Add(CuttingList cuttingList);
        Task Delete(CuttingList cuttingList);
    }
}
