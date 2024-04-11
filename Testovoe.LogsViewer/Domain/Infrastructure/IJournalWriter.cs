using Testovoe.LogsViewer.Domain.Models;

namespace Testovoe.LogsViewer.Domain.Infrastructure;

public interface IJournalWriter
{
    Task Write(Stream stream, JournalEntry journalEntry);
}