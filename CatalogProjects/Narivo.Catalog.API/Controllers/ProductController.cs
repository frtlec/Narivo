using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Narivo.Catalog.API.Dtos.Requests;
using Narivo.Catalog.API.Dtos.Responses;
using Narivo.Catalog.API.Infastructure.Persistence;

namespace Narivo.Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CatalogDbContext _dbContext;

        public ProductController(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _dbContext.Product.ToListAsync();
            var mapped = products.Select(p => new ProductDto(p));
            return Ok(mapped);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int id)
        {
            var product = await _dbContext.Product.FirstOrDefaultAsync(f => f.Id == id);
            if (product == null)
                return NotFound($"{id} product not found");
            var mapped = new ProductDto(product);
            return Ok(mapped);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddProductRequestDto request)
        {
            var product = request.ToProduct();
            await _dbContext.Product.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return Ok(new ProductDto(product));
        }

    }
}
