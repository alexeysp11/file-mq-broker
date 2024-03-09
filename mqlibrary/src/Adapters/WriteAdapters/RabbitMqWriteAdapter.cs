using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.Adapters.WriteAdapters;

/// <summary>
/// 
/// </summary>
public class RabbitMqWriteAdapter : IWriteAdapter
{
    private IWriteMFQueue m_messageFileQueue;

    /// <summary>
    /// 
    /// </summary>
    public RabbitMqWriteAdapter(IWriteMFQueue messageFileQueue)
    {
        m_messageFileQueue = messageFileQueue;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void WriteMessage(string method, string path, string content)
    {
        // 
    }
}