using ReportGenerator.Core;
using ReportGenerator.Core.Settings;

namespace ReportGenerator.Service;

public class Worker : BackgroundService
{
    private readonly ReportConfiguration config;
    private readonly ILogger<Worker> logger;
    private readonly ReportGeneratorService reportGenerator;

    public Worker(
        ReportConfiguration config,
        ReportGeneratorService reportGenerator,
        ILogger<Worker> logger)
    {
        this.config = config;
        this.reportGenerator = reportGenerator;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    await reportGenerator.GenerateAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error generating report, exception: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(config.ReportInterval), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Unrecoverable service error: {ex.Message}. Stopping service!");
            Environment.Exit(1);
        }
    }
}