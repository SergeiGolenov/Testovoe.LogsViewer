using System.Net;
using System.Net.Sockets;
using Testovoe.LogsViewer.Domain.Models;

namespace Testovoe.LogsViewer.Domain.Services;

public class JournalEntrySerializer : IJournalEntrySerializer
{
    public const string AllowedFormat = "yyyy-MM-dd HH:mm:ss";
    public const AddressFamily AllowedAddressFamily = AddressFamily.InterNetwork;
    public const char Separator = ':';

    public string Serialize(JournalEntry entry)
    {
        return entry.Address.ToString() + Separator + entry.Date.ToString(AllowedFormat);
    }

    public JournalEntry Deserialize(string serializedEntry)
    {
        int firstColumnIndex = serializedEntry.IndexOf(Separator);
        string firstPart = serializedEntry.Substring(0, firstColumnIndex);
        string secondPart = serializedEntry.Substring(firstColumnIndex + 1);

        JournalEntry deserializedEntry = new(IPAddress.Parse(firstPart),
            DateTime.ParseExact(secondPart, AllowedFormat, null));

        if (deserializedEntry.Address.AddressFamily != AllowedAddressFamily) throw new Exception();

        return deserializedEntry;
    }
}