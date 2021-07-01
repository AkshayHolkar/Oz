using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Oz.Authentication;


namespace Oz.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task<IdentityResult> ApproveUserAsync(string userId);
    }
}
