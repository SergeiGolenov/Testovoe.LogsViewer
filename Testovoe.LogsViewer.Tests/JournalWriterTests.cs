using System.Net;
using Testovoe.LogsViewer.Domain.Infrastructure;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Domain.Services;

namespace Testovoe.LogsViewer.Tests;

[TestClass]
public class JournalWriterTests
{
    private readonly JournalEntry _entry = new(IPAddress.Loopback, DateTime.UtcNow);
    private readonly JournalEntry? _invalidEntry = null;
    private readonly string _line;

    private readonly MemoryStream _outputStream = new(1024);

    private readonly JournalEntrySerializer _journalEntrySerializer = new();
    private readonly JournalWriter _sut;

    public JournalWriterTests()
    {
        _line = _journalEntrySerializer.Serialize(_entry);
        _sut = new(_journalEntrySerializer);
    }

    [TestMethod]
    public async Task Write_ValidEntry_WrittenLine()
    {
        await _sut.Write(_outputStream, _entry);

        _outputStream.Position = 0;

        StreamReader streamReader = new(_outputStream);
        Assert.AreEqual(streamReader.ReadToEnd(), _line);
    }

    [TestMethod]
    public async Task ManyWrite_ManyValidEntries_ManyWrittenLines()
    {
        await _sut.Write(_outputStream, _entry);
        await _sut.Write(_outputStream, _entry);

        _outputStream.Position = 0;

        StreamReader streamReader = new(_outputStream);
        Assert.AreEqual(streamReader.ReadToEnd(), _line + '\n' + _line);
    }

    [TestMethod]
    public async Task Write_InvalidEntry_Exception()
    {
        bool isExceptionThrown = false;
        try
        {
            await _sut.Write(_outputStream, _invalidEntry!);
        }
        catch
        {
            isExceptionThrown = true;
        }
        Assert.IsTrue(isExceptionThrown);
    }

    [TestMethod]
    public async Task Write_InvalidStream_Exception()
    {
        _outputStream.Close();

        bool isExceptionThrown = false;
        try
        {
            await _sut.Write(_outputStream, _entry);
        }
        catch
        {
            isExceptionThrown = true;
        }
        Assert.IsTrue(isExceptionThrown);
    }
}