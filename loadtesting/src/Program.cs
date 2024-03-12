using FileMqBroker.MqLibrary.LoadTesting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<LoadTestingWorker>();
    })
    .Build();

await host.RunAsync();
