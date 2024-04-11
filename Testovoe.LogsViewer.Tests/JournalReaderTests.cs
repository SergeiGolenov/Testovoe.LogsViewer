using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;
using Testovoe.LogsViewer.Domain.Infrastructure;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Domain.Services;

namespace Testovoe.LogsViewer.Tests;

[TestClass]
public class JournalReaderTests
{
    private readonly JournalEntry _validEntry = new(IPAddress.Loopback, DateTime.UtcNow);
    private readonly string _invalidLine = "151.229.250.75:2024-04-10 15:54\n\n";

    private readonly MemoryStream _validLinesStream;
    private readonly MemoryStream _mixedLinesStream;

    private readonly Mock<ILogger<JournalReader>> _loggerMock = new();
    private readonly JournalEntrySerializer _journalEntrySerializer = new();
    private readonly JournalReader _sut;

    public JournalReaderTests()
    {
        string serializedValidLine = _journalEntrySerializer.Serialize(_validEntry);

        _validLinesStream = new MemoryStream(
            Encoding.UTF8.GetBytes(serializedValidLine + '\n' + serializedValidLine));

        _mixedLinesStream = new MemoryStream(
            Encoding.UTF8.GetBytes(serializedValidLine + '\n' + serializedValidLine + '\n' + _invalidLine));

        _sut = new(_loggerMock.Object, _journalEntrySerializer);
    }

    [TestMethod]
    public async Task ReadAll_ValidLines_Entries()
    {
        List<JournalEntry> result = new();
        await foreach (JournalEntry entry in _sut.ReadAll(_validLinesStream))
            result.Add(entry);
        Assert.AreEqual(result.Count, 2);
    }

    [TestMethod]
    public async Task ReadAll_MixedLines_EntriesForValidAndLogWarningsForInvalid()
    {
        List<JournalEntry> result = new();
        await foreach (JournalEntry entry in _sut.ReadAll(_mixedLinesStream))
            result.Add(entry);

        Assert.AreEqual(result.Count, 2);

        _loggerMock.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.AtLeastOnce);
    }

    [TestMethod]
    public async Task ReadAll_InvalidStream_Exception()
    {
        _validLinesStream.Close();

        bool isExceptionThrown = false;
        try
        {
            List<JournalEntry> result = new();
            await foreach (JournalEntry entry in _sut.ReadAll(_validLinesStream))
                result.Add(entry);
        }
        catch
        {
            isExceptionThrown = true;
        }

        Assert.IsTrue(isExceptionThrown);
    }
}