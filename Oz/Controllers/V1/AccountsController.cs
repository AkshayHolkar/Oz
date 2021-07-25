using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Oz.Extensions;
using Oz.Services;
using Oz.Dtos;
using Oz.Repositories;

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/Accounts")]
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

        //GET: api/v1/Accounts/true
        [Authorize(Roles = "Admin")]
        [HttpGet("{_}/{unused}")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts(bool _, bool unused)
        {
            return await _repository.GetAllAsync();
        }

        // GET: api/v1/Accounts
        [HttpGet]
        public async Task<ActionResult<AccountDto>> GetAccount()
        {
            var userId = HttpContext.GetUserId();
            if (!_repository.IsExist(userId))
                return NotFound();
            return await _repository.GetIndividualAsync(userId);

        }

        // GET: api/v1/Accounts/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(string id)
        {
            if (!_repository.IsExist(id))
                return NotFound();

            return await _repository.GetByIdAsync(id);
        }

        // PUT: api/v1/Accounts/5
        [HttpPut("{id}")]
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

            await _repository.UpdateAsync(accountDto);

            return NoContent();
        }

        // POST: api/v1/Accounts
        [HttpPost]
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
