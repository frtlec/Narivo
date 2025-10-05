using Narivo.Checkout.Core.Business.Dtos.MyPaynetAPiDtos;
using Refit;

namespace Narivo.Checkout.Core.Clients.RefitClients;
public interface IMyPayNetApiClient
{
    [Post("/api/Pay/Create")]
    Task<ApiResponse<PayDto>> Create([Body] PayRequestDto request);
}