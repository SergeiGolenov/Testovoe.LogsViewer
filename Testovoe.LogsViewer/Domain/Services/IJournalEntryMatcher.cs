using Testovoe.LogsViewer.Domain.Models;

namespace Testovoe.LogsViewer.Domain.Services;

public interface IJournalEntryMatcher
{
    bool Match(JournalEntry journalEntry);
}