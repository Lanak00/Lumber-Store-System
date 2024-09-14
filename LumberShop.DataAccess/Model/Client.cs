using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Model
{
    public class Client : User
    {
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
