using ReportGenerator.Core;
using ReportGenerator.Core.DateTime;
using ReportGenerator.Core.ReportWriter;
using ReportGenerator.Core.Settings;
using Services;

namespace ReportGenerator.Service;

public static class DependenciesConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("AppSettings")
            .Get<ReportConfiguration>();

        services.AddSingleton(config);
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IReportWriter, CSVReportWriter>();
        services.AddSingleton<ReportGeneratorService>();
    }
}