using Narivo.Checkout.Core.Clients.Dtos.NarivoMembershipApi;
using Narivo.Shared.Dtos;

namespace Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;

public class MemberDto : DtoBase
{
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }


    public List<MemberAddressDto> Addresses { get; set; }
    public List<MemberCardDto> Cards { get; set; }
}