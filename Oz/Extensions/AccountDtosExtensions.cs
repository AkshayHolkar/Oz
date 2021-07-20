using Oz.Domain;
using Oz.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oz.Extensions
{
    public static class AccountDtosExtensions
    {
        public static AccountDto AsDto(this Account account)
        {
            return new AccountDto
            {
                UserId = account.UserId,
                ContactName = account.ContactName,
                BusinessName = account.BusinessName,
                ABN = account.ABN,
                Phone = account.Phone,
                StreetAddress = account.StreetAddress,
                City = account.City,
                State = account.State,
                Postcode = account.Postcode,
                Country = account.Country,
                Approved = account.Approved
            };
        }

        public static Account AsAccountFromAccountDto(this AccountDto accountDto)
        {
            return new Account()
            {
                UserId = accountDto.UserId,
                ContactName = accountDto.ContactName,
                BusinessName = accountDto.BusinessName,
                ABN = accountDto.ABN,
                Phone = accountDto.Phone,
                StreetAddress = accountDto.StreetAddress,
                City = accountDto.City,
                State = accountDto.State,
                Postcode = accountDto.Postcode,
                Country = accountDto.Country,
                Approved = accountDto.Approved
            };
        }

        public static Account AsAccountFromCreateAccountDto(this CreateAccountDto accountDto)
        {
            return new Account()
            {
                ContactName = accountDto.ContactName,
                BusinessName = accountDto.BusinessName,
                ABN = accountDto.ABN,
                Phone = accountDto.Phone,
                StreetAddress = accountDto.StreetAddress,
                City = accountDto.City,
                State = accountDto.State,
                Postcode = accountDto.Postcode,
                Country = "Australia",
                Approved = false
            };
        }
    }
}
