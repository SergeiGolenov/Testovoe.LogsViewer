using Microsoft.Extensions.Logging;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Domain.Services;

namespace Testovoe.LogsViewer.Domain.Infrastructure;

public class JournalReader(
    ILogger<JournalReader> logger,
    IJournalEntrySerializer journalEntrySerializer) : IJournalReader
{
    private readonly ILogger _logger = logger;
    private readonly IJournalEntrySerializer _journalEntrySerializer = journalEntrySerializer;

    public async IAsyncEnumerable<JournalEntry> ReadAll(Stream stream)
    {
        using StreamReader streamReader = new StreamReader(stream, leaveOpen: true);

        int i = 1;
        while (streamReader.Peek() >= 0)
        {
            JournalEntry? journalEntry = null;

            try
            {
                string line = (await streamReader.ReadLineAsync())!;
                journalEntry = _journalEntrySerializer.Deserialize(line);
            }
            catch
            {
                _logger.LogWarning("Invalid entry at line " + i);
            }

            if (journalEntry != null) yield return journalEntry;
            i++;
        }
    }
}