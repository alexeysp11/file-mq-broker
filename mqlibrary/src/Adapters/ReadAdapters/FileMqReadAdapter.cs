using System.Threading;
using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.Adapters.ReadAdapters;

/// <summary>
/// An adapter that is used on the client application side to read the message queue from the file broker.
/// </summary>
public class FileMqReadAdapter : IReadAdapter
{
    private int m_oneTimeProcQueueElements;
    private IReadMFQueue m_messageFileQueue;
    private Action<MessageFile> m_continuationDelegate;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public FileMqReadAdapter(
        AppInitConfigs appInitConfigs,
        IReadMFQueue messageFileQueue,
        Action<MessageFile> continuationDelegate)
    {
        m_oneTimeProcQueueElements = appInitConfigs.OneTimeProcQueueElements;
        m_messageFileQueue = messageFileQueue;
        m_continuationDelegate = continuationDelegate;
    }
    
    /// <summary>
    /// Method for reading messages from a queue.
    /// </summary>
    public void ReadMessageQueue()
    {
        var messages = m_messageFileQueue.DequeueMessages(m_oneTimeProcQueueElements);
        foreach (var message in messages)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                m_continuationDelegate(message);
            });
        }
    }
}