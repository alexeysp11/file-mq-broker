using FileMqBroker.MqLibrary.KeyCalculations.FileNameGeneration;
using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.Adapters.WriteAdapters;

/// <summary>
/// Provides functionality for writing to a file broker message queue.
/// </summary>
public class FileMqWriteAdapter : IWriteAdapter
{
    private readonly DuplicateRequestCollapseType m_collapseType;
    private IFileNameGeneration m_fileNameGeneration;
    private IWriteMFQueue m_messageFileQueue;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public FileMqWriteAdapter(
        AppInitConfigs appInitConfigs,
        IFileNameGeneration fileNameGeneration,
        IWriteMFQueue messageFileQueue)
    {
        m_collapseType = appInitConfigs.DuplicateRequestCollapseType;
        m_fileNameGeneration = fileNameGeneration;
        m_messageFileQueue = messageFileQueue;
    }
    
    /// <summary>
    /// Ensures that a message is written to the file broker message queue.
    /// </summary>
    public void WriteMessage(string method, string path, string content)
    {
        if (m_collapseType == DuplicateRequestCollapseType.Advanced)
        {
            var hash = m_fileNameGeneration.CalculateHash(method, path);
            if (m_messageFileQueue.IsMessageInQueue(hash))
            {
                return;
            }
        }

        var name = m_fileNameGeneration.GetFileName(method, path);
        var messageFile = new MessageFile
        {
            Name = name,
            Content = content,
            MessageFileType = MessageFileType.Request,
            MessageFileState = MessageFileState.Undefined
        };

        m_messageFileQueue.EnqueueMessage(messageFile);
    }
}