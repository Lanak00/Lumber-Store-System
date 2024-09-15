using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetByOrderId(int orderId);
        Task<OrderItem> GetById(int id);
        Task Add(OrderItem orderItem);
        Task Update(OrderItem orderItem);
        Task Delete(OrderItem orderItem);
    }
}
