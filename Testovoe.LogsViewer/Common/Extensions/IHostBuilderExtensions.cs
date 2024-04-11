using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testovoe.LogsViewer.Domain.Infrastructure;
using Testovoe.LogsViewer.Domain.Services;
using Testovoe.LogsViewer.Shared.Options;

namespace Testovoe.LogsViewer.Shared.Extensions;

internal static class IHostBuilderExtensions
{
    public static IHostBuilder ConfigureApp(this IHostBuilder hostBuilder, string[] args)
    {
        Dictionary<string, string> commandLineMappings = new()
        {
            { "--file-log", "FileLog" },
            { "--file-output", "FileOutput" },
            { "--address-start", "AddressStart" },
            { "--address-mask", "AddressMask" },
            { "--time-start", "TimeStart" },
            { "--time-end", "TimeEnd" }
        };

        hostBuilder.ConfigureAppConfiguration(configure =>
        {
            configure.AddCommandLine(args);
            configure.AddCommandLine(args, commandLineMappings);
        });

        return hostBuilder.ConfigureServices((hostBuilderContext, services) =>
        {
            services.AddOptions<AppOptions>()
                    .Bind(hostBuilderContext.Configuration)
                    .ValidateDataAnnotations()
                    .Validate(AppOptions.Validate, "You can not use AddressMask without AddressStart")
                    .ValidateOnStart();

            services.AddScoped<IJournalReader, JournalReader>();
            services.AddScoped<IJournalWriter, JournalWriter>();

            services.AddScoped<IJournalEntrySerializer, JournalEntrySerializer>();
            services.AddScoped<IJournalEntryMatcher, JournalEntryMatcher>();
            services.AddScoped<IJournalService, JournalService>();
        });
    }
}