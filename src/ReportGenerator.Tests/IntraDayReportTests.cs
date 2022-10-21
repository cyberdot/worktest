using System;
using System.Linq;
using FluentAssertions;
using ReportGenerator.Core;
using Xunit;

namespace ReportGenerator.Tests;

public class IntraDayReportTests
{
    [Fact]
    public void Should_generate_expected_report_file_name()
    {
        var dateTime = new DateTime(2004, 12, 12, 13, 5, 0);
        const string expectedFileName = "PowerPosition_20041212_1305.csv";
        
        var report = new IntraDayReport(dateTime);

        report.FileName.Should().Be(expectedFileName);
    }

    [Fact]
    public void Should_add_trade_period_if_not_present_to_aggregated_results()
    {
        var report = new IntraDayReport(DateTime.Now);
        var tradePeriod = new TradePeriod(1, 123.00);

        report.Aggregate(tradePeriod);

        report.AggregatePositions.Should().HaveCount(1);
        report.AggregatePositions.First().Period.Should().Be(tradePeriod.Period);
        report.AggregatePositions.First().Volume.Should().Be(tradePeriod.Volume);
    }

    [Fact]
    public void Should_update_existing_trade_period_volume_in_aggregated_results()
    {
        var report = new IntraDayReport(DateTime.Now);
        const double volume = 123.23;
        var tradePeriod = new TradePeriod(1, volume);
        report.Aggregate(tradePeriod);
        
        report.Aggregate(tradePeriod);

        report.AggregatePositions.Should().HaveCount(1);
        report.AggregatePositions.First().Period.Should().Be(tradePeriod.Period);
        report.AggregatePositions.First().Volume.Should().Be(volume + volume);
    }

    [Fact]
    public void Should_subtract_existing_trade_period_volume_in_aggregated_results()
    {
        var report = new IntraDayReport(DateTime.Now);
        var tradePeriod = new TradePeriod(1, 123.00);
        report.Aggregate(tradePeriod);
        
        report.Aggregate(new TradePeriod(1, -2.00));

        report.AggregatePositions.Should().HaveCount(1);
        report.AggregatePositions.First().Period.Should().Be(tradePeriod.Period);
        report.AggregatePositions.First().Volume.Should().Be(121.00);
    }
}