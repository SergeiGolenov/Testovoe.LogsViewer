using System.Net;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Domain.Services;

namespace Testovoe.LogsViewer.Tests;

[TestClass]
public class JournalEntrySerializerTests
{
    private readonly JournalEntry _validEntry = new(IPAddress.Loopback, DateTime.Now);
    private readonly JournalEntry? _invalidEntry = null;

    private readonly JournalEntrySerializer _sut = new();

    [TestMethod]
    public void Serialize_ValidEntry_ValidLine()
    {
        Assert.IsFalse(IsExceptionThrownDuringSerialization(_validEntry));
    }

    [TestMethod]
    public void Serialize_InvalidEntry_Exception()
    {
        Assert.IsTrue(IsExceptionThrownDuringSerialization(_invalidEntry!));
    }

    [TestMethod]
    public void Deserialize_ValidLine_ValidEntry()
    {
        Assert.IsFalse(IsExceptionThrownDuringDeserialization(_sut.Serialize(_validEntry)));
    }

    [TestMethod]
    public void Deserialize_InvalidLine_Exception()
    {
        Assert.IsTrue(IsExceptionThrownDuringDeserialization("invalid line"));
    }

    private bool IsExceptionThrownDuringSerialization(JournalEntry entry)
    {
        bool isExceptionThrown = false;
        try
        {
            string serializedEntry = _sut.Serialize(entry);

            int firstColumnIndex = serializedEntry.IndexOf(JournalEntrySerializer.Separator);
            string firstPart = serializedEntry.Substring(0, firstColumnIndex);
            string secondPart = serializedEntry.Substring(firstColumnIndex + 1);

            IPAddress parsedIP = IPAddress.Parse(firstPart);
            if (parsedIP.AddressFamily != JournalEntrySerializer.AllowedAddressFamily) throw new Exception();
            DateTime.ParseExact(secondPart, JournalEntrySerializer.AllowedFormat, null);
        }
        catch
        {
            isExceptionThrown = true;
        }
        return isExceptionThrown;
    }

    private bool IsExceptionThrownDuringDeserialization(string serializedEntry)
    {
        bool isExceptionThrown = false;
        try
        {
            _sut.Deserialize(serializedEntry);
        }
        catch
        {
            isExceptionThrown = true;
        }
        return isExceptionThrown;
    }
}