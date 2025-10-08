using Narivo.Shared.Constants.Enums;

namespace Narivo.WebUI.Models;
public class BasketItem
{
    public Guid Uid { get; set; } = Guid.NewGuid();
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public ProductType ProductType { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public int MemberId { get; set; }
}