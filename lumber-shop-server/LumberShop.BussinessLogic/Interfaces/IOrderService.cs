using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order> GetById(int id);
        Task<IEnumerable<Order>> GetByClientId(int clientId);
        Task Add(NewOrderDTO order);
        Task Update(OrderDTO order);
        Task Delete(int id);
    }
}


