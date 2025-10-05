using Narivo.Catalog.API.Infastructure.Entities;
using Narivo.Shared.Constants.Enums;

namespace Narivo.Catalog.API.Dtos.Requests;

public class AddProductRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductType ProductType { get; set; }
    public bool OpenForSale { get; set; }
    public int Stock { get; set; }

    public Product ToProduct()=> new()
        {
            Name = Name,
            Description = Description,
            UnitPrice = UnitPrice,
            ProductType = ProductType,
            OpenForSale = OpenForSale,
            Stock = Stock,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
    };
}
