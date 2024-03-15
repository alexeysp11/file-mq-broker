using FileMqBroker.MqLibrary.QueueDispatchers;

namespace FileMqBroker.BackendService;

/// <summary>
/// A worker that writes messages back into directory.
/// </summary>
public class WriteMessagesDbWorker : BackgroundService
{
    private readonly ILogger<WriteMessagesDbWorker> _logger;
    private IMqDispatcher m_dispatcher;

    public WriteMessagesDbWorker(
        ILogger<WriteMessagesDbWorker> logger,
        WriteMqDispatcher dispatcher)
    {
        _logger = logger;
        m_dispatcher = dispatcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("WriteMessagesDbWorker running at: {time}", DateTimeOffset.Now);
            m_dispatcher.ProcessMessageQueue();
            await Task.Delay(1000, stoppingToken);
        }
    }
}
