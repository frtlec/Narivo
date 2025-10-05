using Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Shared.Helpers;

namespace Narivo.Checkout.Core.Business.Services
{
    public interface IProductService
    {
        public Task<ProductDto> GetProduct(int id);
        public Task<List<ProductDto>> GetProducts();
    }
    public class ProductService : IProductService
    {
        private readonly ICatalogApiClient _catalogApiClient;

        public ProductService(ICatalogApiClient catalogApiClient)
        {
            _catalogApiClient = catalogApiClient;
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await RefitWrapper.ExecuteAsync<ProductDto>(() => _catalogApiClient.Get(id));
            return product;
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var product = await RefitWrapper.ExecuteAsync<List<ProductDto>>(() => _catalogApiClient.GetAll());
            return product;
        }
    }
}
