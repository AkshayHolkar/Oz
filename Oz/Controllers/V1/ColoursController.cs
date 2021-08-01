using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oz.Repositories;
using Microsoft.AspNetCore.Http;

namespace Oz.Controllers.V1
{
    [Route("api/v1/Colours")]
    [ApiController]
    public class ColoursController : ControllerBase
    {
        private readonly IColourRepository _repository;

        public ColoursController(IColourRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns All Colours Or If ProductId Provided Returns All Product Colours
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        // GET: api/v1/Colours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColourDto>>> GetColours([FromQuery] int productId = 0)
        {
            if (productId != 0)
                return await _repository.GetAllProductColorsAsync(productId);
            return await _repository.GetAllAsync();
        }

        // GET: api/v1/Colours/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ColourDto>> GetColour(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colourDto"></param>
        /// <returns></returns>
        // PUT: api/v1/Colours/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutColour(int id, [FromBody] ColourDto colourDto)
        {
            if (id != colourDto.Id)
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

            await _repository.UpdateAsync(colourDto);

            return NoContent();
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="postColourDto"></param>
        /// <returns></returns>
        // POST: api/v1/Colours
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ColourDto>> PostColour([FromBody] PostColourDto postColourDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var colourDto = await _repository.CreateAsync(postColourDto);

            return CreatedAtAction(nameof(GetColour), new { id = colourDto.Id }, colourDto);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/v1/Colours/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteColour(int id)
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
