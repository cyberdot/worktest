using Microsoft.Extensions.Logging;
using Polly;
using ReportGenerator.Core.DateTime;
using ReportGenerator.Core.ReportWriter;
using Services;

namespace ReportGenerator.Core;

public class ReportGeneratorService
{
    
    private readonly IPowerService powerService;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IReportWriter reportWriter;
    private readonly ILogger<ReportGeneratorService> logger;

    public ReportGeneratorService(
        IPowerService powerService,
        IDateTimeProvider dateTimeProvider,
        IReportWriter reportWriter,
        ILogger<ReportGeneratorService> logger)
    {
        this.powerService = powerService;
        this.dateTimeProvider = dateTimeProvider;
        this.reportWriter = reportWriter;
        this.logger = logger;
    }

    public async Task GenerateAsync()
    {
        logger.LogInformation("Getting current London time");
        var currentUkDateTime = dateTimeProvider.GetCurrentUkDateTime();
        logger.LogInformation($"Current London time: {currentUkDateTime}");
        
        logger.LogInformation("Requesting power trades");
        var retryPolicy = Policy
            .Handle<PowerServiceException>()
            .RetryAsync(3, (exception, retryCount, context) => 
                logger.LogError(exception, $"Error getting power trades, attempt: {retryCount}, Exception: {exception.Message}"));
        
        var trades = await retryPolicy.ExecuteAsync(async () =>
            await powerService.GetTradesAsync(currentUkDateTime)
        );
       
        logger.LogInformation("Creating intraday report");
        var report = new IntraDayReport(currentUkDateTime);

        if (trades?.Any() == true)
        {
            logger.LogInformation($"There are {trades.Count()} trades to process");
            foreach (var trade in trades)
            {
                logger.LogInformation($"Running aggregation on trade {trade.Periods.Count()}  periods");
                
                foreach (var period in trade.Periods)
                {
                    var tradePeriod = new TradePeriod(period.Period, period.Volume);
                    report.Aggregate(tradePeriod);
                }
            }
            logger.LogInformation("Trades have been processed.");
            await reportWriter.WriteAsync(report);
        }
        else
        {
            logger.LogError($"No trades available to generate report. Time: {currentUkDateTime}");
        }
    }
}