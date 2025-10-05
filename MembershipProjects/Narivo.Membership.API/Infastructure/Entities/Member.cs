using Narivo.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Narivo.Membership.API.Infastructure.Entities;

public class Member : BaseEntity
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(100)]
    public string SurName { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; }
    [Required, StringLength(15)]
    public string PhoneNumber { get; set; }


    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<Card> Carts { get; set; } = new List<Card>();

}
