using Microsoft.Extensions.Options;
using System.Net;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Shared.Options;

namespace Testovoe.LogsViewer.Domain.Services;

public class JournalEntryMatcher(IOptions<AppOptions> appOptions) : IJournalEntryMatcher
{
    private readonly IOptions<AppOptions> _appOptions = appOptions;

    public bool Match(JournalEntry journalEntry)
    {
        if (_appOptions.Value.AddressStart != null)
        {
            if (GetUnmaskedBits(journalEntry.Address) <
                GetUnmaskedBits(IPAddress.Parse(_appOptions.Value.AddressStart)))
            {
                return false;
            }
        }

        if ((_appOptions.Value.ParsedTimeStart < journalEntry.Date
            && journalEntry.Date < _appOptions.Value.ParsedTimeEnd) == false) return false;

        return true;
    }

    private uint GetUnmaskedBits(IPAddress ip)
    {
        byte[] ipBytes = ip.GetAddressBytes();
        Array.Reverse(ipBytes);

        uint ipBits = BitConverter.ToUInt32(ipBytes);

        ipBits <<= _appOptions.Value.AddressMask;
        ipBits >>= _appOptions.Value.AddressMask;

        return ipBits;
    }
}