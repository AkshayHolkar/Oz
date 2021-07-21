namespace Oz.Services
{
    public class SharedService : ISharedService
    {
        public bool UserOwnsDomain(string domainUserId, string userId)
        {
            return domainUserId == userId;
        }
    }
}
