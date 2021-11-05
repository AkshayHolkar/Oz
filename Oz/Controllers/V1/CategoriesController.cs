using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oz.Domain;
using Oz.Dtos;
using Oz.Repositories.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oz.Controllers.V1
{
    [Route("api/v1/Categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/v1/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/v1/Categories/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        // PUT: api/v1/Categories/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id)
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

            await _repository.UpdateAsync(category);

            return NoContent();
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="postCategoryDto"></param>
        /// <returns></returns>
        // POST: api/v1/Categories
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> PostCategory([FromBody] PostCategoryDto postCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }

            var category = new Category { Name = postCategoryDto.Name };
            category = await _repository.CreateAsync(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        /// <summary>
        /// Role Administrator Required
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/v1/Categories/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCategory(int id)
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
