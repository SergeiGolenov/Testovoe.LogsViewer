using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testovoe.LogsViewer.Domain.Services;
using Testovoe.LogsViewer.Shared.Extensions;

namespace Testovoe.LogsViewer;

internal class Program
{
    static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args).ConfigureApp(args).Build();
        await host.StartAsync();

        using (IServiceScope serviceScope = host.Services.CreateScope())
        {
            Console.WriteLine("Matched entries "
                + await serviceScope.ServiceProvider.GetRequiredService<IJournalService>().ProcessEntries());
        }
    }
}
