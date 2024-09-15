using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
