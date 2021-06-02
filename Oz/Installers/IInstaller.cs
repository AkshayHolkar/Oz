using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oz.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection service, IConfiguration configuration);
    }
}
