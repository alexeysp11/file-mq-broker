using FileMqBroker.MqLibrary.Adapters.ReadAdapters;
using FileMqBroker.MqLibrary.Adapters.WriteAdapters;
using FileMqBroker.MqLibrary.BackendService.FileContentGenerators;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.BackendService;

/// <summary>
/// A worker who processes messages on the backend.
/// </summary>
public class BackendServiceWorker : BackgroundService
{
    private readonly ILogger<BackendServiceWorker> m_logger;
    private IReadAdapter m_readAdapter;
    private IWriteAdapter m_writeAdapter;
    private MessageFileResponseGen m_messageFileResponseGen;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BackendServiceWorker(
        ILogger<BackendServiceWorker> logger,
        IReadAdapter readAdapter,
        IWriteAdapter writeAdapter,
        MessageFileResponseGen messageFileResponseGen,
        AppInitConfigs appInitConfigs)
    {
        m_logger = logger;
        m_readAdapter = readAdapter;
        m_writeAdapter = writeAdapter;
        m_messageFileResponseGen = messageFileResponseGen;
        appInitConfigs.BackendContinuationDelegate = ContinuationMethod;
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

    /// <summary>
    /// Method that is used to process a message received from a message broker.
    /// </summary>
    public void ContinuationMethod(MessageFile messageFile)
    {
        if (messageFile == null)
            throw new System.ArgumentNullException(nameof(messageFile));

        System.Console.WriteLine("Message file name: {messageFileName}, {content}", messageFile.Name, messageFile.Content);

        var responseContent = m_messageFileResponseGen.GenerateResponseContent(messageFile.Name);
        m_writeAdapter.WriteMessage(messageFile.HttpMethod, messageFile.HttpPath, responseContent);
    }
}
