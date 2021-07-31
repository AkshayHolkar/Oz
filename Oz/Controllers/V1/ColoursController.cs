using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oz.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oz.Repositories;

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
        public async Task<ActionResult<ColourDto>> GetColour(int id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            return await _repository.GetByIdAsync(id);
        }

        // PUT: api/v1/Colours/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
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

        // POST: api/v1/Colours
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ColourDto>> PostColour([FromBody] PostColourDto postColourDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var colourDto = await _repository.CreateAsync(postColourDto);

            return CreatedAtAction(nameof(GetColour), new { id = colourDto.Id }, colourDto);
        }

        // DELETE: api/v1/Colours/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
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
