using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ReportGenerator.Core;
using ReportGenerator.Core.DateTime;
using ReportGenerator.Core.ReportWriter;
using ReportGenerator.Core.Settings;
using Services;
using Xunit;

namespace ReportGenerator.Tests;

public class ReportGeneratorServiceTests
{
    [Fact]
    public async Task GetPowerServiceDataAsync()
    {
        var currentTime = DateTime.Now;
        var powerService = new PowerService();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.GetCurrentUkDateTime().Returns(currentTime);
        var reportGenerator = new ReportGeneratorService(powerService, 
            dateTimeProvider, 
            new CSVReportWriter(new ReportConfiguration
            {
                ReportPath = "reports"
            }, Substitute.For<ILogger<CSVReportWriter>>()), 
            Substitute.For<ILogger<ReportGeneratorService>>());
        
        await reportGenerator.GenerateAsync();

        File.Exists(Path.Combine("reports", $"PowerPosition_{currentTime.ToString("yyyyMMdd_HHmm")}.csv"));
    }
}