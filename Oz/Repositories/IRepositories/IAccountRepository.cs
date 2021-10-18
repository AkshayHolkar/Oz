using Microsoft.AspNetCore.Mvc;
using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Repositories
{
    public interface IAccountRepository
    {
        Task<List<AccountDto>> GetAllAsync();
        Task<AccountDto> GetIndividualAsync(string userId);
        Task<AccountDto> GetByIdAsync(string id);
        Task<AccountDto> CreateAsync(Account account);
        Task UpdateAsync(AccountDto accountDto);
        Task DeleteAsync(string id);
        bool IsExist(string id);
    }
}
