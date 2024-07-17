using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDTO>> GetByOrderId(int orderId);
        Task<OrderItemDTO> GetById(int id);
        Task Add(NewOrderItemDTO orderItem);
        Task Update(OrderItemDTO orderItem);
        Task Delete(int id);
    }
}
