using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oz.Repositories.Contracts;

namespace Oz.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetIndividualAsync(string userId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(i => i.UserId == userId);
            return account;
        }

        public async Task<Account> GetByIdAsync(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            return account;
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Account> CreateAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return account;

        }

        public async Task DeleteAsync(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        public bool IsExist(string id)
        {
            return _context.Accounts.Any(e => e.UserId == id);
        }
    }
}
