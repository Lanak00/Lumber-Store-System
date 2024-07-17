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
    public class DimensionsService : IDimensionsService
    {
        private readonly IDimensionsRepository _dimensionsRepository;
        public DimensionsService(IDimensionsRepository dimensionsRepository)
        {
            _dimensionsRepository = dimensionsRepository;
        }

        public async Task<int> Add(NewDimensionsDTO dimensionsDTO)
        {
            var dimensions = new Dimensions
            {
                Id = new int(),
                Height = dimensionsDTO.Height,
                Width = dimensionsDTO.Width,
                Length = dimensionsDTO.Length,
            };
            return await _dimensionsRepository.Add(dimensions);
        }

        public async Task Delete(int id)
        {
            var dimensions = await _dimensionsRepository.GetById(id);
            await _dimensionsRepository.Delete(dimensions);
        }

        public async Task<IEnumerable<Dimensions>> GetAll()
        {
            return await _dimensionsRepository.GetAll();
        }

        public async Task<Dimensions> GetById(int id)
        {
            return await _dimensionsRepository.GetById(id);
        }

        public async Task<int?> FindByAll(DimensionsDTO dimensionsDTO)
        {
            var dimensions = new Dimensions
            {
                Height = dimensionsDTO.Height,
                Width = dimensionsDTO.Width,
                Length = dimensionsDTO.Length
            };
            return await _dimensionsRepository.FindByAll(dimensions);
        }
    }
}
