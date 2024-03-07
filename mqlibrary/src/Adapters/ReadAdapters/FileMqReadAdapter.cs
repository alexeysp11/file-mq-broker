using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.Adapters.ReadAdapters;

/// <summary>
/// 
/// </summary>
public class FileMqReadAdapter : IReadAdapter
{
    private IReadMFQueue m_messageFileQueue;

    public FileMqReadAdapter(IReadMFQueue messageFileQueue)
    {
        m_messageFileQueue = messageFileQueue;
    }
    
    public async Task<string> ReadMessageAsync()
    {
        List<MessageFile> messages = m_messageFileQueue.DequeueMessages(1);
        if (messages.Count > 0)
        {
            return messages[0].Content;
        }
        return string.Empty;
    }
}