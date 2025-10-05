using Narivo.Catalog.API.Infastructure.Entities;
using Narivo.Shared.Constants.Enums;

namespace Narivo.Catalog.API.Dtos.Responses
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductType ProductType { get; set; }
        public bool OpenForSale { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ProductDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            UnitPrice = product.UnitPrice;
            ProductType = product.ProductType;
            OpenForSale = product.OpenForSale;
            Stock = product.Stock;
            CreatedAt = product.CreatedAt;
            UpdatedAt = product.UpdatedAt;
        }
    }
}
