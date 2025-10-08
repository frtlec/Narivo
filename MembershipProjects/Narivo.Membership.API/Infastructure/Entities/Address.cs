using Narivo.Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narivo.Membership.API.Infastructure.Entities;
public class Address : BaseEntity
{
    [ForeignKey(nameof(Member))]
    public int MemberId { get; set; }

    [Required, StringLength(100)]
    public string City { get; set; }
    [Required, StringLength(100)]
    public string County { get; set; }
    [Required, StringLength(100)]
    public string Town { get; set; }
    [Required, StringLength(100)]
    public string Street { get; set; }
    [Required, StringLength(4)]
    public string BuildingNo { get; set; }
    [Required, StringLength(4)]
    public string FlatNumber { get; set; }

    public bool IsDefault { get; set; }
    public virtual Member Member { get; set; }
}
