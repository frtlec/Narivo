using Narivo.Shared.Dtos;

namespace Narivo.Checkout.Core.Clients.Dtos.NarivoMembershipApi;

public class MemberAddressDto : DtoBase
{
    public string City { get; set; }
    public string County { get; set; }
    public string Town { get; set; }
    public string BuildingNo { get; set; }
    public string FlatNumber { get; set; }
    public bool IsDefault { get; set; }
}
