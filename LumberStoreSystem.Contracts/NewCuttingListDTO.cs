using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.Contracts
{
    public class NewCuttingListDTO
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int OrderId { get; set; }
        public ICollection<NewCuttingListItemDTO> Items { get; set; } = new List<NewCuttingListItemDTO>();

    }
}
