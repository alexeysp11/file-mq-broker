namespace FileMqBroker.MqLibrary.LoadTesting;

public class LoadTestingWorker : BackgroundService
{
    private readonly ILogger<LoadTestingWorker> _logger;

    public LoadTestingWorker(ILogger<LoadTestingWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
