using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.ReadAdapters;

/// <summary>
/// 
/// </summary>
public class RabbitMqReadAdapter : IReadAdapter
{
    private IReadMFQueue m_messageFileQueue;

    public RabbitMqReadAdapter(IReadMFQueue messageFileQueue)
    {
        m_messageFileQueue = messageFileQueue;
    }
    
    public async Task<string> ReadMessageAsync()
    {
        return string.Empty;
    }
}