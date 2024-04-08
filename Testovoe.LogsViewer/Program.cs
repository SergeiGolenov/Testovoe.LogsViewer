using Microsoft.Extensions.Hosting;
using Testovoe.LogsViewer.Shared.Extensions;

namespace Testovoe.LogsViewer;

internal class Program
{
    static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args).AddAppServices().Build();
        await host.StartAsync();
    }
}
