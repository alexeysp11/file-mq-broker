using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.WriteAdapters;

/// <summary>
/// 
/// </summary>
public class RabbitMqWriteAdapter : IWriteAdapter
{
    private IWriteMFQueue m_messageFileQueue;

    public RabbitMqWriteAdapter(IWriteMFQueue messageFileQueue)
    {
        m_messageFileQueue = messageFileQueue;
    }
    
    public async Task WriteMessageAsync(string message)
    {
        // 
    }
}