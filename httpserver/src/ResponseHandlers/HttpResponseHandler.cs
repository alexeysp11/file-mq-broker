using FileMqBroker.MqLibrary.Adapters.ReadAdapters;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.HttpService.ResponseHandlers;

/// <summary>
/// Processes responses from the backend and sends them to the client application via HTTP.
/// </summary>
public class HttpResponseHandler : BackgroundService
{
    private readonly ILogger<HttpResponseHandler> m_logger;
    private IReadAdapter m_readAdapter;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public HttpResponseHandler(
        ILogger<HttpResponseHandler> logger,
        IReadAdapter readAdapter,
        AppInitConfigs appInitConfigs)
    {
        m_logger = logger;
        m_readAdapter = readAdapter;
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

        // Send the message via HTTP.
    }
}