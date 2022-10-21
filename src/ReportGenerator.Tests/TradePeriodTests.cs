using FluentAssertions;
using ReportGenerator.Core;
using Xunit;

namespace ReportGenerator.Tests;

public class TradePeriodTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(25)]
    public void Should_throw_exception_if_period_is_invalid(int period)
    {
        Assert.Throws<PeriodOutOfRangeException>(() => 
            new TradePeriod(period, 123.00));
    }

    [Theory]
    [InlineData(1, "23:00")]
    [InlineData(2, "00:00")]
    [InlineData(3, "01:00")]
    [InlineData(4, "02:00")]
    [InlineData(5, "03:00")]
    [InlineData(6, "04:00")]
    [InlineData(7, "05:00")]
    [InlineData(8, "06:00")]
    [InlineData(9, "07:00")]
    [InlineData(10, "08:00")]
    [InlineData(11, "09:00")]
    [InlineData(12, "10:00")]
    [InlineData(13, "11:00")]
    [InlineData(14, "12:00")]
    [InlineData(15, "13:00")]
    [InlineData(16, "14:00")]
    [InlineData(17, "15:00")]
    [InlineData(18, "16:00")]
    [InlineData(19, "17:00")]
    [InlineData(20, "18:00")]
    [InlineData(21, "19:00")]
    [InlineData(22, "20:00")]
    [InlineData(23, "21:00")]
    [InlineData(24, "22:00")]
    public void Should_format_period_into_time_representation(int period, string timeRepresentation)
    {
        var tradePeriod = new TradePeriod(period, 123.00);
        tradePeriod.LocalTime.Should().Be(timeRepresentation);
    }
}