using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Interfaces;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Add(ProductDTO productDTO)
        {
            var product = new Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Category = productDTO.Category,
                Manufacturer = productDTO.Manufacturer,
                Unit = productDTO.Unit,
                Price = productDTO.Price,
                Image = productDTO.Image,
                DimensionsId = productDTO.DimensionsId,
            };
            await _productRepository.Add(product);
        }

        public async Task Delete(string id)
        {
            var product = await _productRepository.GetById(id);
            await _productRepository.Delete(product);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productRepository.GetAll();           
        }

        public async Task<Product> GetById(string id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task Update(ProductDTO productDTO)
        {
            var product = new Product
            {
                Id= productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Category = productDTO.Category,
                Manufacturer = productDTO.Manufacturer,
                Unit = productDTO.Unit,
                Price = productDTO.Price,
                Image = productDTO.Image,
                DimensionsId = productDTO.DimensionsId
            };
            await _productRepository.Update(product);
        }
    }
}
