using Narivo.Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narivo.Checkout.Core.Infastructure.Entites;

public class OrderItem : BaseEntity
{
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    [Required, StringLength(100)]
    public string ProductName { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public virtual Order Order { get; set; }
}
