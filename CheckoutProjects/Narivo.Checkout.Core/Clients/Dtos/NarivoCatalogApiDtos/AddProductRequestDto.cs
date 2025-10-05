
using Narivo.Shared.Constants.Enums;

namespace Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;

public class AddProductRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductType ProductType { get; set; }
    public bool OpenForSale { get; set; }
    public int Stock { get; set; }
}
