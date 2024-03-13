using FileMqBroker.MqLibrary.QueueDispatchers;

namespace FileMqBroker.MqLibrary.LoadTesting;

public class WriteMqDispatcherWorker : BackgroundService
{
    private readonly ILogger<WriteMqDispatcherWorker> _logger;
    private IMqDispatcher m_dispatcher;

    public WriteMqDispatcherWorker(
        ILogger<WriteMqDispatcherWorker> logger,
        WriteMqDispatcher dispatcher)
    {
        _logger = logger;
        m_dispatcher = dispatcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("WriteMqDispatcherWorker running at: {time}", DateTimeOffset.Now);
            m_dispatcher.ProcessMessageQueue();
            await Task.Delay(5000, stoppingToken);
        }
    }
}
