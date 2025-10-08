using Narivo.Checkout.Core.Infastructure.Enums;
using Narivo.Shared.Constants.Enums;
using Narivo.Shared.Dtos;
using Narivo.Shared.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narivo.Checkout.Core.Infastructure.Entites;

public class OrderItem : BaseEntity
{
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required, StringLength(100)]
    public string ProductName { get; set; }
    public ProductType ProductType { get; set; }
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [Required]
    public OrderItemStatus Status { get; set; }
    [Required, StringLength(10)]
    public string StatusText => Status.ToDescription();

    public virtual Order Order { get; set; }
}
