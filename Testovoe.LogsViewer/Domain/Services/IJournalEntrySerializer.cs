using Testovoe.LogsViewer.Domain.Models;

namespace Testovoe.LogsViewer.Domain.Services;

public interface IJournalEntrySerializer
{
    JournalEntry Deserialize(string serializedEntry);
    string Serialize(JournalEntry entry);
}