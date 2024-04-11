using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Domain.Services;

namespace Testovoe.LogsViewer.Domain.Infrastructure;

public class JournalWriter(IJournalEntrySerializer journalEntrySerializer) : IJournalWriter
{
    private readonly IJournalEntrySerializer _journalEntrySerializer = journalEntrySerializer;

    public async Task Write(Stream stream, JournalEntry journalEntry)
    {
        using StreamWriter streamWriter = new(stream, leaveOpen: true);

        if (stream.Length != 0)
            streamWriter.Write('\n');

        await streamWriter.WriteAsync(_journalEntrySerializer.Serialize(journalEntry));
        await streamWriter.FlushAsync();
    }
}