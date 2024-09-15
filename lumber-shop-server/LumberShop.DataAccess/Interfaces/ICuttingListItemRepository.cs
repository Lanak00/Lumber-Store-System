using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Interfaces
{
    public interface ICuttingListItemRepository
    {
        Task<IEnumerable<CuttingListItem>> GetByCuttingListId(int cuttingListId);
        Task<CuttingListItem> GetById(int id);
        Task Add(CuttingListItem cuttingListItem);
        Task Update(CuttingListItem cuttingListItem);
        Task Delete(CuttingListItem cuttingListItem);
    }

}
