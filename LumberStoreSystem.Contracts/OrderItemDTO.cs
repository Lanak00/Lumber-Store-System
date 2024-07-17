using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.Contracts
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public int Price { get; set; }
    }
}
