using LumberStoreSystem.DataAccess.Model.Enummerations;

namespace LumberStoreSystem.Contracts
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ProductCategory Category { get; set; }
        public string Manufacturer { get; set; }
        public MeasureUnit Unit { get; set; }
        public int Price { get; set; }
        public string? Image { get; set; }
        public int DimensionsId { get; set; }
    }
}
