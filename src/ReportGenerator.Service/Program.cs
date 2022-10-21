using ReportGenerator.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Power Trades Report Generator";
    })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        DependenciesConfig.Configure(services, configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();