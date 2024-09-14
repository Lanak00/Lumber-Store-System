using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class CuttingListService : ICuttingListService
    {
        private readonly ICuttingListRepository _cuttingListRepository;
        public CuttingListService(ICuttingListRepository cuttingListRepository)
        {
            _cuttingListRepository = cuttingListRepository;
        } 
        public async Task Add(NewCuttingListDTO cuttingListDTO)
        {
            var cuttingList = new CuttingList
            {
                Id = new int(),
                ProductId = cuttingListDTO.ProductId,
                OrderId = cuttingListDTO.OrderId,
            };
            await _cuttingListRepository.Add(cuttingList);
        }

        public async Task Delete(int id)
        {
            var cuttingList = await _cuttingListRepository.GetById(id);
            await _cuttingListRepository.Delete(cuttingList);
        }

        public async Task<CuttingList> GetById(int id)
        {
            return await _cuttingListRepository.GetById(id);
        }

        public async Task<IEnumerable<CuttingList>> GetByOrderId(int orderId)
        {
            return await _cuttingListRepository.GetByOrderId(orderId);
        }
        public async Task<IEnumerable<CuttingList>> GetAll()
        {
            return await _cuttingListRepository.GetAll();
        }
    }
}
