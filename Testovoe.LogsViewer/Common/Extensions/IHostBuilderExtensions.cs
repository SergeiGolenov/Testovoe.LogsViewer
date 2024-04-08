using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testovoe.LogsViewer.Shared.Options;

namespace Testovoe.LogsViewer.Shared.Extensions;

internal static class IHostBuilderExtensions
{
    public static IHostBuilder AddAppServices(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((hostBuilderContext, services) =>
        {
            services.AddOptions<AppOptions>()
                    .Bind(hostBuilderContext.Configuration)
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
        });
    }
}