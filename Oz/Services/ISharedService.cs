namespace Oz.Services
{
    public interface ISharedService
    {
        bool UserOwnsDomain(string domainUserId, string userId);
    }
}
