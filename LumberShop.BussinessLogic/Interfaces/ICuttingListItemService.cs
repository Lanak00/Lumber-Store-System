using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{  
    public interface ICuttingListItemService
    {
        Task<IEnumerable<CuttingListItem>> GetByCuttingListId(int cuttingListId);
        Task<CuttingListItem> GetById(int id);
        Task Add(NewCuttingListItemDTO cuttingListItem);  
        Task Update(CuttingListItemDTO cuttingListItem);
        Task Delete(int id);
    }
}
