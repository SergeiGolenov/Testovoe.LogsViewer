namespace Testovoe.LogsViewer.Domain.Services;

public interface IJournalService
{
    Task<int> ProcessEntries();
}