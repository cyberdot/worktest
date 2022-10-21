using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Settings;

namespace ReportGenerator.Core.ReportWriter;

public class CSVReportWriter : IReportWriter
{
    private readonly ReportConfiguration config;
    private readonly ILogger<CSVReportWriter> logger;

    public CSVReportWriter(
        ReportConfiguration config,
        ILogger<CSVReportWriter> logger)
    {
        this.config = config;
        this.logger = logger;
    }
    
    public async Task WriteAsync(IntraDayReport report)
    {
        logger.LogInformation($"Check if report directory {config.ReportPath} is present");
        if (!Directory.Exists(config.ReportPath))
        {
            logger.LogInformation($"Report directory doesn't exist. Creating directory: {config.ReportPath} ...");
            Directory.CreateDirectory(config.ReportPath);
        }
        

        using (var writer = new StreamWriter(Path.Combine(config.ReportPath, report.FileName)))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
          
            csv.Context.RegisterClassMap<TradePeriodMap>();
            logger.LogInformation($"Writing report data into CSV file: {report.FileName}");
            csv.WriteHeader<TradePeriod>();
            csv.NextRecordAsync();
            
            await csv.WriteRecordsAsync(report.AggregatePositions);
            logger.LogInformation($"{report.AggregatePositions.Count()} records have been successfuly written to {report.FileName}");
        }
    }
}