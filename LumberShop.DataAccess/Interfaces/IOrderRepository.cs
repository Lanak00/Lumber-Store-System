using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order> GetById(string id); 
        Task Add(Order order);
        Task Update(Order order);
        Task Delete(Order order);
    }
}
