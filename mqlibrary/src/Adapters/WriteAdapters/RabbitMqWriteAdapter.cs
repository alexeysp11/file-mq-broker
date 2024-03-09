using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.Adapters.WriteAdapters;

/// <summary>
/// Provides functionality for writing to the RabbitMQ message queue.
/// </summary>
public class RabbitMqWriteAdapter : IWriteAdapter
{
    private IWriteMFQueue m_messageFileQueue;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public RabbitMqWriteAdapter(IWriteMFQueue messageFileQueue)
    {
        m_messageFileQueue = messageFileQueue;
    }
    
    /// <summary>
    /// Ensures that a message is written to the RabbitMQ message queue.
    /// </summary>
    public void WriteMessage(string method, string path, string content)
    {
        // 
    }
}