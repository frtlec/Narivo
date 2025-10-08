using Narivo.Shipping.Core.Integration.Dtos;
using Refit;

namespace Narivo.Shipping.Core.Integration;

public interface IMembershipApiRefitClient
{
    [Get("/api/Members/GetByIdAndAddressId")]
    Task<ApiResponse<GetByIdAndAddressIdDto>> GetByIdAndAddressId([Query] int id, [Query] int addressId);
}
