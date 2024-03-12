using FileMqBroker.MqLibrary.Adapters.ReadAdapters;
using FileMqBroker.MqLibrary.Adapters.WriteAdapters;
using FileMqBroker.BackendService;
using FileMqBroker.BackendService.FileContentGenerators;
using FileMqBroker.MqLibrary.FileContentGenerators;
using FileMqBroker.MqLibrary.KeyCalculations;
using FileMqBroker.MqLibrary.KeyCalculations.FileNameGeneration;
using FileMqBroker.MqLibrary.KeyCalculations.RequestCollapsing;
using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // App init configs.
        services.AddSingleton(_ =>
        {
            return new AppInitConfigs
            {
                DbConnectionString = "",
                RequestDirectoryName = "",
                ResponseDirectoryName = "",
                OneTimeProcQueueElements = 20_000,
                DuplicateRequestCollapseType = DuplicateRequestCollapseType.Naive
            };
        });

        // File content generation.
        services.AddSingleton<IFileContentGenerator, MessageFileResponseGen>();

        // Key calculations.
        services.AddSingleton<KeyCalculationMD5>();
        services.AddSingleton<KeyCalculationSHA256>();
        services.AddSingleton<IFileNameGeneration, FileNameGenerationMD5>();
        services.AddSingleton<IRequestCollapser, RequestCollapserSHA256>();

        // Queues.
        services.AddSingleton<ReadMessageFileQueue>();
        services.AddSingleton<WriteMessageFileQueue>();

        // Queue adapters.
        services.AddSingleton<IReadAdapter, FileMqReadAdapter>();
        services.AddSingleton<IWriteAdapter, FileMqWriteAdapter>();

        // Backend worker service.
        services.AddHostedService<BackendServiceWorker>();
    })
    .Build();

await host.RunAsync();
