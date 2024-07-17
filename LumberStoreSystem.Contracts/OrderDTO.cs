using LumberStoreSystem.DataAccess.Model.Enummerations;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.Contracts
{
    public class OrderDTO
    {
        public DateOnly Date { get; set; }
        public OrderStatus Status { get; set; }
        public int ClientId { get; set; }
        public int Price { get; set; }
    }
}
