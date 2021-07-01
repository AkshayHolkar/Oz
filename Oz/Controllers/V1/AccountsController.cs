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

namespace Oz.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;

        public AccountsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/Accounts
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccount(bool withRoles)
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/v1/Accounts
        [HttpGet]
        public async Task<ActionResult<Account>> GetAccount()
        {
            return await _context.Accounts.FirstOrDefaultAsync(i => i.UserId == HttpContext.GetUserId());
        }

        // GET: api/v1/Accounts/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/v1/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, [FromBody] Account account)
        {
            if (id != account.UserId)
            {
                return BadRequest();
            }

            var userOwnAccount = UserOwnsAccount(account, HttpContext.GetUserId());

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
        public async Task<ActionResult<Account>> PostAccount([FromBody] Account account)
        {
            var a = await _context.Accounts.FindAsync(HttpContext.GetUserId());
            if (a != null)
            {
                return BadRequest(new { error = "This account exist" });
            }

            account.UserId = HttpContext.GetUserId();
            account.User = null;
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.UserId }, account);
        }

        // DELETE: api/v1/Accounts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Account>> DeleteAccount(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            var userOwnAccount = UserOwnsAccount(account, HttpContext.GetUserId());

            if (!userOwnAccount)
            {
                return BadRequest(new { error = "You do not own this account" });
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return account;
        }

        private bool AccountExists(string id)
        {
            return _context.Accounts.Any(e => e.UserId == id);
        }

        private bool UserOwnsAccount(Account account, string userId)
        {
            if (account.UserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
