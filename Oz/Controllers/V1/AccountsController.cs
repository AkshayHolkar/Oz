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

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISharedService _sharedService;

        public AccountsController(DataContext context, IIdentityService identityService, ISharedService sharedService)
        {
            _context = context;
            _identityService = identityService;
            _sharedService = sharedService;
        }

        //GET: api/v1/Accounts/true
        [Authorize(Roles = "Admin")]
        [HttpGet("{_}/{unused}")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts(bool _, bool unused)
        {
            return await _context.Accounts.Select(account => account.AsDto()).ToListAsync();
        }

        // GET: api/v1/Accounts
        [HttpGet]
        public async Task<ActionResult<AccountDto>> GetAccount()
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(i => i.UserId == HttpContext.GetUserId());

            if (account == null)
                return NotFound();
            return account.AsDto();

        }

        // GET: api/v1/Accounts/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(string id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account.AsDto();
        }

        // PUT: api/v1/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, [FromBody] AccountDto accountDto)
        {
            if (id != accountDto.UserId)
            {
                return BadRequest();
            }

            var account = accountDto.AsAccountFromAccountDto();
            var userId = HttpContext.GetUserId();
            var userOwnAccount = _sharedService.UserOwnsDomain(account.UserId, userId) || await _identityService.IsAdminAsync(userId);

            if (!userOwnAccount)
            {
                return BadRequest(new { error = "You do not own this account" });
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/Accounts
        [HttpPost]
        public async Task<ActionResult<AccountDto>> PostAccount([FromBody] CreateAccountDto accountDto)
        {
            var a = await _context.Accounts.FindAsync(HttpContext.GetUserId());
            if (a != null)
            {
                return BadRequest(new { error = "This account exist" });
            }

            Account account = accountDto.AsAccountFromCreateAccountDto();
            account.UserId = HttpContext.GetUserId();

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.UserId }, account.AsDto());
        }

        // DELETE: api/v1/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            var userOwnAccount = _sharedService.UserOwnsDomain(account.UserId, HttpContext.GetUserId());

            if (!userOwnAccount)
            {
                return BadRequest(new { error = "You do not own this account" });
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(string id)
        {
            return _context.Accounts.Any(e => e.UserId == id);
        }
    }
}
