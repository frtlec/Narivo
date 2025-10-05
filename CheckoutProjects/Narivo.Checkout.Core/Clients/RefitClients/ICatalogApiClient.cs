using Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;
using Refit;

namespace Narivo.Checkout.Core.Clients.RefitClients;
public interface ICatalogApiClient
{
    [Post("/api/product/add")]
    Task<ApiResponse<ProductDto>> Add([Body] AddProductRequestDto request);

    [Get("/api/product/getall")]
    Task<ApiResponse<List<ProductDto>>> GetAll();

    [Get("/api/product/get")]
    Task<ApiResponse<ProductDto>> Get([Query] int id);
}
