using System.IO;
using Vcr;
using Vcr.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VcrServiceCollectionExtensions
    {
        /// <summary>
        /// Adds VCR services including FileSystemCassetteStorage.
        /// </summary>
        public static IServiceCollection AddVcr(this IServiceCollection services, string storageLocation)
        {
            services.AddSingleton<IStateProvider, DefaultStateProvider>();
            services.AddSingleton<IVcrProvider, HttpContextVcrProvider>();
            services.AddSingleton<ICasseteStorage>(new FileSystemCassetteStorage(new DirectoryInfo(storageLocation)));
            return services;
        }

        /// <summary>
        /// Adds VCR services without an ICassetteStorage implementation.
        /// </summary>
        public static IServiceCollection AddVcr(this IServiceCollection services)
        {
            services.AddSingleton<IStateProvider, DefaultStateProvider>();
            services.AddSingleton<IVcrProvider, HttpContextVcrProvider>();
            return services;
        }
    }
}
