using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using Oz.Extensions;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/ProductSizes")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private readonly IProductSizeRepository _repository;

        public ProductSizesController(IProductSizeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns All ProductSizes Or If ProductId Provided Returns All Available ProductSizes For Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        // GET: api/v1/ProductSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSizeDto>>> GetProductSizes([FromQuery] int productId = 0)
        {
            if (productId != 0)
            {
                var productSizes = await _repository.GetAllProductSizesByProductIdAsync(productId);
                return Ok(productSizes.Select(productSize => productSize.AsDto()));

            }
            var allProductSizes = await _repository.GetAllAsync();
            return Ok(allProductSizes.Select(productSize => productSize.AsDto()));
        }

        // GET: api/v1/ProductSizes/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductSizeDto>> GetProductSize(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            var productSize = await _repository.GetByIdAsync(id);
            return Ok(productSize.AsDto());
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productSizeDto"></param>
        /// <returns></returns>
        // PUT: api/v1/ProductSizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutProductSize(int id, [FromBody] ProductSizeDto productSizeDto)
        {
            if (id != productSizeDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            await _repository.UpdateAsync(productSizeDto.AsProductSizeFromProductSizeDto());

            return NoContent();
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="postProductSizeDto"></param>
        /// <returns></returns>
        // POST: api/v1/ProductSizes
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductSizeDto>> PostProductSize([FromBody] PostProductSizeDto postProductSizeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productSizeDto = await _repository.CreateAsync(postProductSizeDto.AsProductSizeFromPostProductSizeDto());

            return CreatedAtAction(nameof(GetProductSize), new { id = productSizeDto.Id }, productSizeDto);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/v1/ProductSizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductSize(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
