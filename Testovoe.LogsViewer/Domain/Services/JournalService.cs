using Microsoft.Extensions.Options;
using Testovoe.LogsViewer.Domain.Infrastructure;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Shared.Options;

namespace Testovoe.LogsViewer.Domain.Services;

public class JournalService(
    IJournalReader journalReader,
    IJournalWriter journalWriter,
    IJournalEntryMatcher journalEntryMatcher,
    IOptions<AppOptions> appOptions) : IJournalService
{
    private readonly IJournalReader _journalReader = journalReader;
    private readonly IJournalWriter _journalWriter = journalWriter;
    private readonly IJournalEntryMatcher _journalEntryMatcher = journalEntryMatcher;
    private readonly IOptions<AppOptions> _appOptions = appOptions;

    public async Task<int> ProcessEntries()
    {
        using FileStream input = new(_appOptions.Value.FileLog, FileMode.Open);
        using FileStream output = new(_appOptions.Value.FileOutput, FileMode.Append);

        int i = 1;
        await foreach (JournalEntry entry in _journalReader.ReadAll(input))
        {
            if (_journalEntryMatcher.Match(entry))
            {
                await _journalWriter.Write(output, entry);
                i++;
            }
        }

        return i;
    }
}