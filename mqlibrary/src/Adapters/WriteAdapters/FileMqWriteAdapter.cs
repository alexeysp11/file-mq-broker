using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.Adapters.WriteAdapters;

/// <summary>
/// 
/// </summary>
public class FileMqWriteAdapter : IWriteAdapter
{
    private IWriteMFQueue m_messageFileQueue;

    public FileMqWriteAdapter(IWriteMFQueue messageFileQueue)
    {
        m_messageFileQueue = messageFileQueue;
    }
    
    public async Task WriteMessageAsync(string message)
    {
        var messageFile = new MessageFile
        {
            Content = message
        };

        m_messageFileQueue.EnqueueMessage(messageFile);
    }
}