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
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public IEnumerable<CuttingListItem> cuttingListItems { get; set; } = Enumerable.Empty<CuttingListItem>();
    }
}
