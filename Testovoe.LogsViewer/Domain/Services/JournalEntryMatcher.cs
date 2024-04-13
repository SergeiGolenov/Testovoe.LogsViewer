using Microsoft.Extensions.Options;
using System.Net;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Shared.Options;

namespace Testovoe.LogsViewer.Domain.Services;

public class JournalEntryMatcher(IOptions<AppOptions> appOptions) : IJournalEntryMatcher
{
    private readonly IOptions<AppOptions> _appOptions = appOptions;

    public bool Match(JournalEntry entry)
    {
        if (_appOptions.Value.AddressStart != null)
        {
            int mask = _appOptions.Value.AddressMask;
            IPAddress startAddress = IPAddress.Parse(_appOptions.Value.AddressStart);

            uint maskBits = Convert.ToUInt32(Math.Pow(2, mask) - 1) << (32 - mask);
            uint startAddressBits = BitConverter.ToUInt32(startAddress.GetAddressBytes().Reverse().ToArray());
            uint entryAddressBits = BitConverter.ToUInt32(entry.Address.GetAddressBytes().Reverse().ToArray());

            uint subnetBits = startAddressBits & maskBits;

            if (((entryAddressBits & maskBits) | subnetBits) == subnetBits)
            {
                if (entryAddressBits < startAddressBits) return false;
            }
            else
            {
                return false;
            }
        }

        if (!(_appOptions.Value.ParsedTimeStart < entry.Date
            && entry.Date < _appOptions.Value.ParsedTimeEnd)) return false;

        return true;
    }
}