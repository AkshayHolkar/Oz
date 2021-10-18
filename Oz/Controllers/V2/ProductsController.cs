using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using Oz.Extensions;
using Oz.Repositories;
using System.Threading.Tasks;

namespace Oz.Controllers.V2
{
    [Route("api/v2/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: api/v2/Products/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (HttpContext.IsApprovedUser())
            {
                return product;
            }

            product.Price = 0;
            return product;
        }
    }
}
