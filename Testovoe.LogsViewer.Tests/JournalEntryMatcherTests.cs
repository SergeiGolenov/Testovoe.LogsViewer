using Microsoft.Extensions.Options;
using System.Net;
using Testovoe.LogsViewer.Domain.Models;
using Testovoe.LogsViewer.Domain.Services;
using Testovoe.LogsViewer.Shared.Options;

namespace Testovoe.LogsViewer.Tests;

[TestClass]
public class JournalEntryMatcherTests
{
    private readonly JournalEntry _testEntry = new(IPAddress.Parse("246.37.90.100"), DateTime.UtcNow);

    [TestMethod]
    public void Match_ValidDateTimes_True()
    {
        IOptions<AppOptions> testOptions = Options.Create(new AppOptions()
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
        });

        JournalEntryMatcher sut = new(testOptions);
        Assert.IsTrue(sut.Match(_testEntry));
    }

    [TestMethod]
    public void Match_InvalidDateTimes_True()
    {
        IOptions<AppOptions> testOptions = Options.Create(new AppOptions()
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString(),
        });

        JournalEntryMatcher sut = new(testOptions);
        Assert.IsFalse(sut.Match(_testEntry));
    }

    [TestMethod]
    public void Match_ValidIpRange_True()
    {
        IOptions<AppOptions> testOptions = Options.Create(new AppOptions()
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
            AddressStart = "246.37.90.100",
            AddressMask = 24
        });

        JournalEntryMatcher sut = new(testOptions);
        Assert.IsTrue(sut.Match(_testEntry));
    }

    [TestMethod]
    public void Match_InvalidIpRange_True()
    {
        IOptions<AppOptions> testOptions = Options.Create(new AppOptions()
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
            AddressStart = "246.37.90.101",
            AddressMask = 24
        });

        JournalEntryMatcher sut = new(testOptions);

        Assert.IsFalse(sut.Match(_testEntry));

        testOptions = Options.Create(new AppOptions()
        {
            FileLog = "",
            FileOutput = "",
            TimeStart = (DateTime.UtcNow - TimeSpan.FromDays(1)).ToString(),
            TimeEnd = (DateTime.UtcNow + TimeSpan.FromDays(1)).ToString(),
            AddressStart = "246.37.89.100",
            AddressMask = 24
        });

        sut = new(testOptions);

        Assert.IsFalse(sut.Match(_testEntry));
    }
}