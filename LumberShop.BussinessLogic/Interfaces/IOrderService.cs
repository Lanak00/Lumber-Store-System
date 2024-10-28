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
        Task<IEnumerable<OrderDTO>> GetAll();
        Task<OrderDTO> GetById(int id);
        Task<IEnumerable<OrderDTO>> GetByClientId(int clientId);
        Task Add(NewOrderDTO order);
        Task Update(OrderDTO order);
        Task Delete(int id);
    }
}


