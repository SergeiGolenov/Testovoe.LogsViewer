using Testovoe.LogsViewer.Domain.Models;

namespace Testovoe.LogsViewer.Domain.Infrastructure;

public interface IJournalReader
{
    IAsyncEnumerable<JournalEntry> ReadAll(Stream stream);
}