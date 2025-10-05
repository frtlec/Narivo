
using Narivo.Shared.Constants.Enums;
using Narivo.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Narivo.Catalog.API.Infastructure.Entities;

public class Product : BaseEntity
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required, StringLength(1000)]
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    [Required,StringLength(30)]
    public ProductType ProductType { get; set; }
    public bool OpenForSale { get; set; }
    public int Stock { get; set; }

}
