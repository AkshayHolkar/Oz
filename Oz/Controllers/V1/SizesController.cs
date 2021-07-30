using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oz.Domain;
using Oz.Dtos;
using Oz.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/Sizes")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly IDomainsRepository<Size> _repository;

        public SizesController(IDomainsRepository<Size> repository)
        {
            _repository = repository;
        }

        // GET: api/v1/Sizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Size>>> GetSizes()
        {
            return await _repository.GetAllAsync();
        }

        // GET: api/v1/Sizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Size>> GetSize(int id)
        {
            var size = await _repository.GetByIdAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            return size;
        }

        // PUT: api/v1/Sizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSizeAsync(int id, [FromBody] Size size)
        {
            if (id != size.Id)
            {
                return BadRequest();
            }

            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            await _repository.UpdateAsync(size);

            return NoContent();
        }

        // POST: api/v1/Sizes
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Size>> PostSize([FromBody] PostSizeDto postSizeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            var size = new Size { Name = postSizeDto.Name };
            size = await _repository.CreateAsync(size);

            return CreatedAtAction(nameof(GetSize), new { id = size.Id }, size);
        }

        // DELETE: api/v1/Sizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSizeAsync(int id)
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

