using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Model
{
    public class CuttingListItem
    {
        public int Id { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Amount { get; set; }
        public int CuttingListId { get; set; }
        public CuttingList CuttingList { get; set; }
    }
}
