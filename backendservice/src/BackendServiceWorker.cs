using FileMqBroker.MqLibrary.Adapters.ReadAdapters;
using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.ResponseHandlers;

namespace FileMqBroker.BackendService;

/// <summary>
/// A worker who processes messages on the backend.
/// </summary>
public class BackendServiceWorker : BackgroundService
{
    private readonly ILogger<BackendServiceWorker> m_logger;
    private IReadAdapter m_readAdapter;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BackendServiceWorker(
        ILogger<BackendServiceWorker> logger,
        IReadAdapter readAdapter,
        WriteBackResponseHandler responseHandler,
        AppInitConfigs appInitConfigs)
    {
        m_logger = logger;
        m_readAdapter = readAdapter;
        appInitConfigs.BackendContinuationDelegate = responseHandler.ContinuationMethod;
    }

    /// <summary>
    /// Method for executing worker functionality.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            m_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            m_readAdapter.ReadMessageQueue();
            await Task.Delay(1000, stoppingToken);
        }
    }
}