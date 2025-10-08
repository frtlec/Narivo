
using Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;
using Refit;

namespace Narivo.Checkout.Core.Clients.RefitClients;
public interface IMembershipApiClient
{
    [Get("/api/cards/get")]
    Task<ApiResponse<GetCardResponseDto>> GetCard([Query] int memberId, [Query] int cardId);

    [Get("/api/members/getall")]
    Task<ApiResponse<List<MemberDto>>> GetAll();


    [Get("/api/Members/GetById")]
    Task<ApiResponse<MemberDto>> Get([Query] int id);
}

