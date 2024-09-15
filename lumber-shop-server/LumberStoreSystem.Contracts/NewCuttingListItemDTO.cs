using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.Contracts
{
    public class NewCuttingListItemDTO
    {
        public int Length { get; set; }
        public int Width { get; set; }
        public int Amount { get; set; }
        public int CuttingListId { get; set; }
    }
}
