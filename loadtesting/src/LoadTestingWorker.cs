using FileMqBroker.MqLibrary.LoadTesting.LoadGenerators;

namespace FileMqBroker.MqLibrary.LoadTesting;

public class LoadTestingWorker : BackgroundService
{
    private readonly ILogger<LoadTestingWorker> _logger;
    private ILoadGenerator m_loadGenerator;

    public LoadTestingWorker(
        ILogger<LoadTestingWorker> logger,
        ILoadGenerator loadGenerator)
    {
        _logger = logger;
        m_loadGenerator = loadGenerator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // _logger.LogInformation("LoadTestingWorker running at: {time}", DateTimeOffset.Now);
            m_loadGenerator.GenerateLoad();
            await Task.Delay(1000, stoppingToken);
        }
    }
}
