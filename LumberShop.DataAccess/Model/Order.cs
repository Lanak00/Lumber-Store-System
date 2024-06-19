using LumberStoreSystem.DataAccess.Model.Enummerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public OrderStatus Status { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public IEnumerable<OrderItem> items { get; set;} = Enumerable.Empty<OrderItem>();
        public IEnumerable<CuttingList> cuttingLists { get; set; } = Enumerable.Empty<CuttingList>();
    }
}
