using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Oz.Extensions;
using Oz.Services;
using Oz.Dtos;
using System.Net.Mime;
using Oz.Repositories.Contracts;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/Accounts")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly ISharedService _sharedService;

        public AccountsController(IAccountRepository repository, IIdentityService identityService, ISharedService sharedService)
        {
            _repository = repository;
            _identityService = identityService;
            _sharedService = sharedService;
        }

        /// <summary>
        /// Returns All Accounts (Role Administrator Required)
        /// </summary>
        /// <param name="_"></param>
        /// <param name="unused"></param>
        /// <returns></returns>
        //GET: api/v1/Accounts/true
        [Authorize(Roles = "Admin")]
        [HttpGet("{_}/{unused}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts(bool _, bool unused)
        {
            var accounts = await _repository.GetAllAsync();
            return Ok(accounts.Select(account => account.AsDto()));
        }

        /// <summary>
        /// Returns User Account
        /// </summary>
        /// <returns></returns>
        // GET: api/v1/Accounts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetAccount()
        {
            var userId = HttpContext.GetUserId();
            if (!_repository.IsExist(userId))
                return NotFound();
            var account = await _repository.GetIndividualAsync(userId);

            return Ok(account.AsDto());

        }

        /// <summary>
        /// Returns Account by Id (Role Administrator Required)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/v1/Accounts/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetAccount(string id)
        {
            if (!_repository.IsExist(id))
                return NotFound();

            var account = await _repository.GetByIdAsync(id);

            return Ok(account.AsDto());
        }

        // PUT: api/v1/Accounts/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutAccount(string id, [FromBody] AccountDto accountDto)
        {
            if (id != accountDto.UserId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.IsExist(accountDto.UserId))
            {
                return NotFound();
            }

            var userId = HttpContext.GetUserId();
            var isApprovedUser = _sharedService.UserOwnsDomain(accountDto.UserId, userId) || await _identityService.IsAdminAsync(userId);

            if (!isApprovedUser)
            {
                return BadRequest(new { error = "You do not own this account" });
            }

            await _repository.UpdateAsync(accountDto.AsAccountFromAccountDto());

            return NoContent();
        }

        // POST: api/v1/Accounts
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AccountDto>> PostAccount([FromBody] CreateAccountDto createAccountDto)
        {
            if (_repository.IsExist(HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "This account exist" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = createAccountDto.AsAccountFromCreateAccountDto();
            account.UserId = HttpContext.GetUserId();

            var accountDto = await _repository.CreateAsync(account);

            return CreatedAtAction(nameof(GetAccount), new { id = accountDto.UserId }, accountDto);
        }

        // DELETE: api/v1/Accounts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            if (!_repository.IsExist(id))
            {
                return NotFound();
            }

            var accountDto = await _repository.GetByIdAsync(id);

            if (!_sharedService.UserOwnsDomain(accountDto.UserId, HttpContext.GetUserId()))
            {
                return BadRequest(new { error = "You do not own this account" });
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
