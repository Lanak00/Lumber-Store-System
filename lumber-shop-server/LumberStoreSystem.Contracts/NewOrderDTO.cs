using LumberStoreSystem.DataAccess.Model;
using LumberStoreSystem.DataAccess.Model.Enummerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.Contracts
{
    public class NewOrderDTO
    {
        public DateOnly Date { get; set; }
        public OrderStatus Status { get; set; }
        public int ClientId { get; set; }
        public ICollection<NewOrderItemDTO> Items { get; set; } = new List<NewOrderItemDTO>();
    }
}
