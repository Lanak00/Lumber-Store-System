using LumberStoreSystem.DataAccess.Model;
using LumberStoreSystem.Contracts;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(string id);
        Task Add(ProductDTO product);
        Task Update(ProductDTO product);
        Task Delete(string id);
    }
}
