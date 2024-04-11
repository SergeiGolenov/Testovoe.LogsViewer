using Microsoft.Extensions.Options;
using System.Net;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Domain.Services;
using Testovoe.LogsViewer.Shared.Options;

namespace Testovoe.LogsViewer.Tests;

[TestClass]
public class JournalEntryMatcherTests
{
    private readonly IOptions<AppOptions> _optionsValidDateTimes;
    private readonly IOptions<AppOptions> _optionsInvalidDateTimes;
    private readonly JournalEntry _dateTimesJournalEntry = new(IPAddress.Loopback, DateTime.UtcNow);

    private readonly IOptions<AppOptions> _optionsValidDateTimesValidIpRange;
    private readonly IOptions<AppOptions> _optionsValidDateTimesInvalidIpRange;
    private readonly JournalEntry _dateTimesAndIpRangeJournalEntry
        = new(IPAddress.Parse("127.0.0.100"), DateTime.UtcNow);

    public JournalEntryMatcherTests()
    {
        _optionsValidDateTimes = Options.Create(new AppOptions
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
        });

        _optionsInvalidDateTimes = Options.Create(new AppOptions
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString()
        });

        _optionsValidDateTimesValidIpRange = Options.Create(new AppOptions
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
            AddressStart = "127.0.0.100",
            AddressMask = 24
        });

        _optionsValidDateTimesInvalidIpRange = Options.Create(new AppOptions
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
            AddressStart = "127.0.0.101",
            AddressMask = 24
        });
    }

    [TestMethod]
    public void Match_ValidDateTimes_True()
    {
        JournalEntryMatcher sut = new(_optionsValidDateTimes);
        Assert.IsTrue(sut.Match(_dateTimesJournalEntry));
    }

    [TestMethod]
    public void Match_InvalidDateTimes_False()
    {
        JournalEntryMatcher sut = new(_optionsInvalidDateTimes);
        Assert.IsFalse(sut.Match(_dateTimesJournalEntry));
    }

    [TestMethod]
    public void Match_ValidDateTimesWithValidIpRange_True()
    {
        JournalEntryMatcher sut = new(_optionsValidDateTimesValidIpRange);
        Assert.IsTrue(sut.Match(_dateTimesAndIpRangeJournalEntry));
    }

    [TestMethod]
    public void Match_ValidDateTimesWithInvalidIpRange_False()
    {
        JournalEntryMatcher sut = new(_optionsValidDateTimesInvalidIpRange);
        Assert.IsFalse(sut.Match(_dateTimesAndIpRangeJournalEntry));
    }
}