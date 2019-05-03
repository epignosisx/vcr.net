using System.IO;
using Vcr;
using Vcr.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VcrServiceCollectionExtensions
    {
        public static IServiceCollection AddVcr(this IServiceCollection services, string storageLocation)
        {
            services.AddSingleton<ICassetteProvider, CookieCassetteProvider>();
            services.AddSingleton<IVcrProvider, HttpContextVcrProvider>();
            services.AddSingleton<ICasseteStorage>(new FileSystemCassetteStorage(new DirectoryInfo(storageLocation)));
            return services;
        }
    }
}
