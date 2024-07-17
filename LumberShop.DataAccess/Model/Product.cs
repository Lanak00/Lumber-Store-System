using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumberStoreSystem.DataAccess.Model.Enummerations;

namespace LumberStoreSystem.DataAccess.Model
{

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public ProductCategory Category { get; set; }
        public string Manufacturer { get; set; }
        public MeasureUnit Unit { get; set; }
        public int Price { get; set; }
        public string? Image { get; set; } = null;
        public Dimensions Dimensions { get; set; }
        public int DimensionsId { get; set; }
        public IEnumerable<OrderItem> orderItems { get; set; }
        public IEnumerable<CuttingList> cuttingLists { get; set; }
    }
}
