using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oz.Data;
using Oz.Domain;
using Oz.Dtos;
using Oz.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<AccountDto>> GetAllAsync()
        {
            return await _context.Accounts.Select(account => account.AsDto()).ToListAsync();
        }

        public async Task<AccountDto> GetIndividualAsync(string userId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(i => i.UserId == userId);
            return account.AsDto();

        }

        public async Task<AccountDto> GetByIdAsync(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            return account.AsDto();
        }

        public async Task UpdateAsync(AccountDto accountDto)
        {
            _context.Entry(accountDto.AsAccountFromAccountDto()).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<AccountDto> CreateAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return account.AsDto();

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
