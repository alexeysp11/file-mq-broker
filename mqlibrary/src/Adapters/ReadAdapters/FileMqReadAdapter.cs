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
    private Action<MessageFile> m_continuationDelegate;
    private IReadMFQueue m_messageFileQueue;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public FileMqReadAdapter(
        AppInitConfigs appInitConfigs,
        IReadMFQueue messageFileQueue)
    {
        m_oneTimeProcQueueElements = appInitConfigs.OneTimeProcQueueElements;
        m_continuationDelegate = appInitConfigs.BackendContinuationDelegate;
        m_messageFileQueue = messageFileQueue;
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