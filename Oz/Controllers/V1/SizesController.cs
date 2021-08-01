using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Size>> GetSize(int id)
        {
            var size = await _repository.GetByIdAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            return size;
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        // PUT: api/v1/Sizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="postSizeDto"></param>
        /// <returns></returns>
        // POST: api/v1/Sizes
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/v1/Sizes/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

