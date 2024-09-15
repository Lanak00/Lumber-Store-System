using LumberStoreSystem.DataAccess.Model.Enummerations;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.Contracts
{
    public class ReturnOrderDTO
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderItem> Items { get; set; } = Enumerable.Empty<OrderItem>();
        public IEnumerable<CuttingList> CuttingLists { get; set; } = Enumerable.Empty<CuttingList>();
    }
}
