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
        Task<ActionResult<IEnumerable<AccountDto>>> GetAllAsync();
        Task<ActionResult<AccountDto>> GetIndividualAsync(string userId);
        Task<ActionResult<AccountDto>> GetByIdAsync(string id);
        Task<AccountDto> CreateAsync(Account account);
        Task<ActionResult<bool>> UpdateAsync(AccountDto accountDto);
        Task<ActionResult<bool>> DeleteAsync(string id);
        bool IsExist(string id);
    }
}
