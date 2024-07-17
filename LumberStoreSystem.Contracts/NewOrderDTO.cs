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
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public OrderStatus Status { get; set; }
        public int ClientId { get; set; }

    }
}
