using System.Net;

namespace Testovoe.LogsViewer.Domain.Models;

public record JournalEntry(IPAddress Address, DateTime Date);