using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using LumberStoreSystem.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class CuttingListItemService : ICuttingListItemService
    {
        private readonly ICuttingListItemRepository _cuttingListItemRepository;
     
        public CuttingListItemService (ICuttingListItemRepository cuttingListItemRepository)
        {
            _cuttingListItemRepository = cuttingListItemRepository;
        }
        
        public async Task Add(NewCuttingListItemDTO cuttingListItemDTO)
        {
            var cuttingListItem = new CuttingListItem
            {
                Id = new int(),
                Length = cuttingListItemDTO.Length,
                Width = cuttingListItemDTO.Width,
                Amount = cuttingListItemDTO.Amount,
                CuttingListId = cuttingListItemDTO.CuttingListId
            };
            await _cuttingListItemRepository.Add(cuttingListItem);
        }

        public async Task Delete(int id)
        {
            var cuttingListItem = await _cuttingListItemRepository.GetById(id);
            await _cuttingListItemRepository.Delete(cuttingListItem);
        }

        public Task<IEnumerable<CuttingListItem>> GetByCuttingListId(int cuttingListId)
        {
            throw new NotImplementedException();
        }

        public async Task<CuttingListItem> GetById(int id)
        {
            return await _cuttingListItemRepository.GetById(id);
        }
  
        public async Task Update(CuttingListItemDTO  cuttingListItemDTO)
        {
            var cuttingListItem = new CuttingListItem
            {
                Id = cuttingListItemDTO.Id,
                Length = cuttingListItemDTO.Length,
                Width= cuttingListItemDTO.Width,
                Amount = cuttingListItemDTO.Amount,
                CuttingListId = cuttingListItemDTO.CuttingListId
            };
            await _cuttingListItemRepository.Update(cuttingListItem);
        }
    }
}
