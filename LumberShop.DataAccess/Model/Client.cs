using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Model
{
    public class Client : User
    {
        public IEnumerable<Order> orders { get; set; } = Enumerable.Empty<Order>();
    }
}
