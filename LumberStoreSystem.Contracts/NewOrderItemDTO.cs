using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.Contracts
{
    public class NewOrderItemDTO
    {
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public string ProductId { get; set; }
    }
}
