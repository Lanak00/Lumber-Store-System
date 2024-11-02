using LumberStoreSystem.DataAccess.Model.Enummerations;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.Contracts
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int Status { get; set; }
        public float TotalPrice { get; set; }
        public int ClientId { get; set; }
        public List<ItemDto> Items { get; set; } = new List<ItemDto>();
        public List<CuttingListDTO> CuttingLists { get; set; } = new List<CuttingListDTO>();
    }

    public class ItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ProductImage { get; set; }  // Add this field
    }
}
