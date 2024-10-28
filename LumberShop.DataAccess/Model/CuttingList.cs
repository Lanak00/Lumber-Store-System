using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Model
{
    public class CuttingList
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; } 
        public int OrderId { get; set; }
        public float Price {  get; set; }
        public ICollection<CuttingListItem> cuttingListItems { get; set; } = new List<CuttingListItem>();
    }
}

